using Dapper;

namespace Servicios;

public class ReporteResumenPeriodo
{
    public decimal TotalVentas { get; set; }
    public decimal VentasEfectivo { get; set; }
    public decimal VentasTransferencia { get; set; }
    public int CantidadVentas { get; set; }
    public decimal GananciaBruta { get; set; }
    public decimal TotalCompras { get; set; }
}

public class ProductoTopVenta
{
    public string Nombre { get; set; } = "";
    public decimal CantidadTotal { get; set; }
    public decimal MontoTotal { get; set; }
}

public class ReporteServicio
{
    public async Task<ReporteResumenPeriodo> ObtenerResumenPeriodoAsync(DateTime desde, DateTime hasta)
    {
        await using var cn = await Conexion.AbrirAsync();

        var sql = @"
            SELECT
                COALESCE(SUM(v.total), 0) AS TotalVentas,
                COALESCE((SELECT SUM(vp.monto) FROM venta_pagos vp
                          JOIN ventas v2 ON v2.id = vp.venta_id
                          WHERE v2.estado = 'COMPLETADA' AND vp.metodo = 'EFECTIVO'
                            AND v2.fecha BETWEEN @desde AND @hasta), 0) AS VentasEfectivo,
                COALESCE((SELECT SUM(vp.monto) FROM venta_pagos vp
                          JOIN ventas v2 ON v2.id = vp.venta_id
                          WHERE v2.estado = 'COMPLETADA' AND vp.metodo = 'TRANSFERENCIA'
                            AND v2.fecha BETWEEN @desde AND @hasta), 0) AS VentasTransferencia,
                COUNT(v.id) AS CantidadVentas,
                COALESCE((SELECT SUM((vd.precio_unitario - vd.costo_unitario) * vd.cantidad)
                          FROM venta_detalle vd
                          JOIN ventas v3 ON v3.id = vd.venta_id
                          WHERE v3.estado = 'COMPLETADA'
                            AND v3.fecha BETWEEN @desde AND @hasta), 0) AS GananciaBruta,
                COALESCE((SELECT SUM(total) FROM compras
                          WHERE fecha BETWEEN @desde AND @hasta), 0) AS TotalCompras
            FROM ventas v
            WHERE v.estado = 'COMPLETADA'
              AND v.fecha BETWEEN @desde AND @hasta";

        var res = await cn.QuerySingleOrDefaultAsync<ReporteResumenPeriodo>(sql, new
        {
            desde = desde.Date,
            hasta = hasta.Date.AddDays(1).AddTicks(-1)
        });

        return res ?? new ReporteResumenPeriodo();
    }

    public async Task<List<ProductoTopVenta>> ObtenerTopProductosAsync(DateTime desde, DateTime hasta, int limite = 10)
    {
        await using var cn = await Conexion.AbrirAsync();

        var sql = @"
            SELECT
                p.nombre AS Nombre,
                SUM(vd.cantidad) AS CantidadTotal,
                SUM(vd.subtotal) AS MontoTotal
            FROM venta_detalle vd
            JOIN ventas v ON v.id = vd.venta_id
            JOIN productos p ON p.id = vd.producto_id
            WHERE v.estado = 'COMPLETADA'
              AND v.fecha BETWEEN @desde AND @hasta
            GROUP BY p.id, p.nombre
            ORDER BY CantidadTotal DESC
            LIMIT @limite";

        var list = await cn.QueryAsync<ProductoTopVenta>(sql, new
        {
            desde = desde.Date,
            hasta = hasta.Date.AddDays(1).AddTicks(-1),
            limite
        });

        return list.ToList();
    }
}

