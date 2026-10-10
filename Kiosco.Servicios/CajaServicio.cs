using Dapper;
using Kiosco.Entidades;
using MySqlConnector;

namespace Kiosco.Servicios;

public class CajaServicio
{
    public async Task<JornadaCaja?> ObtenerAbiertaAsync()
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryFirstOrDefaultAsync<JornadaCaja>(
            "SELECT * FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1");
    }

    public async Task<JornadaCaja?> ObtenerPorFechaAsync(DateTime fecha)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryFirstOrDefaultAsync<JornadaCaja>(
            "SELECT * FROM jornadas_caja WHERE fecha=@f", new { f = fecha.Date });
    }

    public async Task<long> AbrirAsync(decimal montoApertura)
    {
        if (montoApertura < 0) throw new NegocioException("El monto inicial no puede ser negativo.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var abierta = await cn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM jornadas_caja WHERE estado='ABIERTA' FOR UPDATE", transaction: tx);
            if (abierta > 0) throw new NegocioException("Ya hay una caja abierta. Cerrala primero.");

            var hoy = DateTime.Today;
            var existe = await cn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM jornadas_caja WHERE fecha=@hoy", new { hoy }, tx);
            if (existe > 0)
                throw new NegocioException("La caja de hoy ya fue abierta y cerrada. " +
                    "Si necesitás seguir vendiendo, reabrila desde Caja.");

            var id = await cn.ExecuteScalarAsync<long>(
                @"INSERT INTO jornadas_caja (fecha,apertura_en,monto_apertura)
                  VALUES (@hoy,@ahora,@montoApertura);
                  SELECT LAST_INSERT_ID();",
                new { hoy, ahora = DateTime.Now, montoApertura }, tx);
            await tx.CommitAsync();
            return id;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task RegistrarMovimientoAsync(string tipo, string concepto, decimal monto)
    {
        if (tipo != "INGRESO" && tipo != "EGRESO") throw new NegocioException("Tipo inválido.");
        if (string.IsNullOrWhiteSpace(concepto)) throw new NegocioException("El concepto es obligatorio.");
        if (monto <= 0) throw new NegocioException("El monto debe ser mayor a cero.");

        await using var cn = await Conexion.AbrirAsync();
        var jornadaId = await cn.ExecuteScalarAsync<long?>(
            "SELECT id FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1")
            ?? throw new NegocioException("No hay una caja abierta.");

        await cn.ExecuteAsync(
            @"INSERT INTO movimientos_caja (jornada_id,tipo,concepto,monto,fecha)
              VALUES (@jornadaId,@tipo,@concepto,@monto,@fecha)",
            new { jornadaId, tipo, concepto = concepto.Trim(), monto, fecha = DateTime.Now });
    }

    public async Task<ResumenJornada> ObtenerResumenAsync(long jornadaId)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await ResumenAsync(cn, null, jornadaId);
    }

    private static Task<ResumenJornada> ResumenAsync(MySqlConnection cn, MySqlTransaction? tx, long id) =>
        cn.QuerySingleAsync<ResumenJornada>(
            @"SELECT
                j.monto_apertura AS MontoApertura,
                (SELECT COALESCE(SUM(vp.monto),0) FROM venta_pagos vp
                   JOIN ventas v ON v.id=vp.venta_id
                  WHERE v.jornada_id=j.id AND v.estado='COMPLETADA' AND vp.metodo='EFECTIVO') AS VentasEfectivo,
                (SELECT COALESCE(SUM(vp.monto),0) FROM venta_pagos vp
                   JOIN ventas v ON v.id=vp.venta_id
                  WHERE v.jornada_id=j.id AND v.estado='COMPLETADA' AND vp.metodo='TRANSFERENCIA') AS VentasTransferencia,
                (SELECT COUNT(*) FROM ventas WHERE jornada_id=j.id AND estado='COMPLETADA') AS CantidadVentas,
                (SELECT COUNT(*) FROM ventas WHERE jornada_id=j.id AND estado='ANULADA') AS CantidadAnuladas,
                (SELECT COALESCE(SUM(monto),0) FROM movimientos_caja
                   WHERE jornada_id=j.id AND tipo='INGRESO') AS Ingresos,
                (SELECT COALESCE(SUM(monto),0) FROM movimientos_caja
                   WHERE jornada_id=j.id AND tipo='EGRESO') AS Egresos,
                (SELECT COALESCE(SUM((d.precio_unitario-d.costo_unitario)*d.cantidad),0)
                   FROM venta_detalle d JOIN ventas v ON v.id=d.venta_id
                  WHERE v.jornada_id=j.id AND v.estado='COMPLETADA') AS GananciaBruta
              FROM jornadas_caja j WHERE j.id=@id",
            new { id }, tx);

    /// Arqueo ciego: el dueño cuenta el efectivo y lo ingresa SIN ver el esperado.
    public async Task<ResumenJornada> CerrarAsync(long jornadaId, decimal efectivoContado, string? observaciones)
    {
        if (efectivoContado < 0) throw new NegocioException("El efectivo contado no puede ser negativo.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var estado = await cn.ExecuteScalarAsync<string>(
                "SELECT estado FROM jornadas_caja WHERE id=@jornadaId FOR UPDATE", new { jornadaId }, tx);
            if (estado != "ABIERTA") throw new NegocioException("Esa caja ya está cerrada.");

            var r = await ResumenAsync(cn, tx, jornadaId);
            await cn.ExecuteAsync(
                @"UPDATE jornadas_caja SET estado='CERRADA', cierre_en=@ahora,
                    efectivo_esperado=@esp, efectivo_contado=@cont,
                    diferencia=@dif, observaciones=@obs
                  WHERE id=@jornadaId",
                new { ahora = DateTime.Now, esp = r.EfectivoEsperado, cont = efectivoContado,
                      dif = efectivoContado - r.EfectivoEsperado, obs = observaciones, jornadaId }, tx);
            await tx.CommitAsync();
            return r;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    /// Para corregir una jornada cerrada por error (no puede haber otra abierta).
    public async Task ReabrirAsync(long jornadaId)
    {
        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            if (await cn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM jornadas_caja WHERE estado='ABIERTA' FOR UPDATE", transaction: tx) > 0)
                throw new NegocioException("Hay otra caja abierta. Cerrala antes de reabrir.");

            await cn.ExecuteAsync(
                @"UPDATE jornadas_caja SET estado='ABIERTA', cierre_en=NULL, efectivo_esperado=NULL,
                  efectivo_contado=NULL, diferencia=NULL WHERE id=@jornadaId", new { jornadaId }, tx);
            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }
}

