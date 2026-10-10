using Dapper;
using Kiosco.Entidades;

namespace Kiosco.Servicios;

public class CompraServicio
{
    public async Task<long> RegistrarAsync(Compra c)
    {
        if (c.Items.Count == 0) throw new NegocioException("La compra no tiene productos.");
        if (c.Items.Any(i => i.Cantidad <= 0 || i.CostoUnitario < 0))
            throw new NegocioException("Hay cantidades o costos inválidos.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            long? jornadaId = null;
            if (c.PagadaConCaja)
            {
                jornadaId = await cn.ExecuteScalarAsync<long?>(
                    "SELECT id FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1 FOR UPDATE", transaction: tx)
                    ?? throw new NegocioException("Para pagar con la caja, primero abrila.");
            }

            var compraId = await cn.ExecuteScalarAsync<long>(
                @"INSERT INTO compras (proveedor_id,nro_comprobante,fecha,total,pagada_con_caja,observaciones)
                  VALUES (@ProveedorId,@NroComprobante,@fecha,@total,@pagada,@Observaciones);
                  SELECT LAST_INSERT_ID();",
                new { c.ProveedorId,                 // null = sin proveedor
                      c.NroComprobante, fecha = DateTime.Now, total = c.Total,
                      pagada = c.PagadaConCaja, c.Observaciones }, tx);

            foreach (var it in c.Items)
            {
                var antes = await StockHelper.AplicarAsync(cn, tx, it.ProductoId, "COMPRA",
                    it.Cantidad, "COMPRA", compraId, null, true);

                await cn.ExecuteAsync(
                    @"INSERT INTO compra_detalle (compra_id,producto_id,cantidad,costo_unitario,subtotal)
                      VALUES (@compraId,@ProductoId,@Cantidad,@CostoUnitario,@sub)",
                    new { compraId, it.ProductoId, it.Cantidad, it.CostoUnitario, sub = it.Subtotal }, tx);

                // Política simple: el costo del producto pasa a ser el último costo de compra
                if (antes.PrecioCosto != it.CostoUnitario)
                {
                    await cn.ExecuteAsync(
                        @"INSERT INTO historial_precios
                          (producto_id,costo_anterior,costo_nuevo,venta_anterior,venta_nuevo,fecha)
                          VALUES (@pid,@ca,@cn,@pv,@pv,@f)",
                        new { pid = it.ProductoId, ca = antes.PrecioCosto, cn = it.CostoUnitario,
                              pv = antes.PrecioVenta, f = DateTime.Now }, tx);
                    await cn.ExecuteAsync(
                        "UPDATE productos SET precio_costo=@c WHERE id=@pid",
                        new { c = it.CostoUnitario, pid = it.ProductoId }, tx);
                }
            }

            if (c.PagadaConCaja)
                await cn.ExecuteAsync(
                    @"INSERT INTO movimientos_caja (jornada_id,tipo,concepto,monto,origen,compra_id,fecha)
                      VALUES (@jornadaId,'EGRESO','Compra de mercadería',@total,'COMPRA',@compraId,@f)",
                    new { jornadaId, total = c.Total, compraId, f = DateTime.Now }, tx);

            await tx.CommitAsync();
            return compraId;
        }
        catch { await tx.RollbackAsync(); throw; }
    }
}

