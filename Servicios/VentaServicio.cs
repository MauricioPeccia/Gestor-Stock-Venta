using Dapper;
using Entidades;

namespace Servicios;

public class VentaServicio
{
    public async Task<long> RegistrarAsync(Venta v, bool permitirSinStock = false)
    {
        if (v.Items.Count == 0) throw new NegocioException("La venta no tiene productos.");
        if (v.Items.Any(i => i.Cantidad <= 0)) throw new NegocioException("Hay cantidades inválidas.");
        if (v.Items.Any(i => i.Unidad == "UN" && i.Cantidad % 1 != 0))
            throw new NegocioException("Los productos por unidad deben venderse en cantidades enteras.");
        if (v.Descuento < 0 || v.Descuento > v.Subtotal) throw new NegocioException("Descuento inválido.");
        if (v.Pagos.Any(p => p.Monto <= 0)) throw new NegocioException("Hay pagos con monto inválido.");
        if (v.Pagos.Sum(p => p.Monto) != v.Total)
            throw new NegocioException("La suma de los pagos no coincide con el total.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var jornadaId = await cn.ExecuteScalarAsync<long?>(
                "SELECT id FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1 FOR UPDATE", transaction: tx)
                ?? throw new NegocioException("No hay una caja abierta. Abrila para poder vender.");

            var ventaId = await cn.ExecuteScalarAsync<long>(
                @"INSERT INTO ventas (jornada_id,fecha,subtotal,descuento,total)
                  VALUES (@jornadaId,@fecha,@sub,@desc,@total);
                  SELECT LAST_INSERT_ID();",
                new { jornadaId, fecha = DateTime.Now, sub = v.Subtotal, desc = v.Descuento, total = v.Total }, tx);

            foreach (var it in v.Items)
            {
                var prod = await StockHelper.AplicarAsync(cn, tx, it.ProductoId, "VENTA",
                    -it.Cantidad, "VENTA", ventaId, null, permitirSinStock);

                await cn.ExecuteAsync(
                    @"INSERT INTO venta_detalle
                      (venta_id,producto_id,cantidad,precio_unitario,costo_unitario,subtotal)
                      VALUES (@ventaId,@ProductoId,@Cantidad,@PrecioUnitario,@costo,@sub)",
                    new
                    {
                        ventaId,
                        it.ProductoId,
                        it.Cantidad,
                        it.PrecioUnitario,
                        costo = prod.PrecioCosto,
                        sub = it.Subtotal
                    }, tx);
            }

            foreach (var p in v.Pagos)
                await cn.ExecuteAsync(
                    "INSERT INTO venta_pagos (venta_id,metodo,monto) VALUES (@ventaId,@Metodo,@Monto)",
                    new { ventaId, p.Metodo, p.Monto }, tx);

            await tx.CommitAsync();
            return ventaId;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    private record InfoVenta(string Estado, string JornadaEstado);

    /// Solo se anulan ventas de la jornada ABIERTA (si ya cerró, hay que reabrirla primero:
    /// así nunca cambia el arqueo de un día ya cerrado sin que te des cuenta).
    public async Task AnularAsync(long ventaId, string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo)) throw new NegocioException("El motivo es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var info = await cn.QuerySingleOrDefaultAsync<InfoVenta>(
                @"SELECT v.estado AS Estado, j.estado AS JornadaEstado
                  FROM ventas v JOIN jornadas_caja j ON j.id=v.jornada_id
                  WHERE v.id=@ventaId FOR UPDATE", new { ventaId }, tx)
                ?? throw new NegocioException("La venta no existe.");

            if (info.Estado == "ANULADA") throw new NegocioException("La venta ya está anulada.");
            if (info.JornadaEstado != "ABIERTA")
                throw new NegocioException("Esa venta pertenece a una caja cerrada. Reabrí esa jornada para anularla.");

            var items = await cn.QueryAsync<(int producto_id, decimal cantidad)>(
                "SELECT producto_id, cantidad FROM venta_detalle WHERE venta_id=@ventaId",
                new { ventaId }, tx);

            foreach (var (productoId, cantidad) in items)
                await StockHelper.AplicarAsync(cn, tx, productoId, "ANULACION_VENTA",
                    cantidad, "VENTA", ventaId, motivo, true);

            await cn.ExecuteAsync(
                @"UPDATE ventas SET estado='ANULADA', fecha_anulacion=@f, motivo_anulacion=@motivo
                  WHERE id=@ventaId",
                new { f = DateTime.Now, motivo = motivo.Trim(), ventaId }, tx);

            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task<IEnumerable<dynamic>> VentasDeJornadaAsync(long jornadaId)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryAsync(
            "SELECT id, fecha, total, estado FROM ventas WHERE jornada_id=@jornadaId ORDER BY id DESC",
            new { jornadaId });
    }
}