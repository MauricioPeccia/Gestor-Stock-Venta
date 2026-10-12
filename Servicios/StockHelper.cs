using Dapper;
using Entidades;
using MySqlConnector;

namespace Servicios;

internal static class StockHelper
{
    /// delta > 0 suma stock, delta < 0 resta. Devuelve el producto con su estado ANTERIOR al cambio.
    public static async Task<Producto> AplicarAsync(
        MySqlConnection cn, MySqlTransaction tx,
        int productoId, string tipo, decimal delta,
        string? refTipo, long? refId, string? motivo, bool permitirNegativo)
    {
        // FOR UPDATE bloquea la fila hasta terminar la transacción
        var prod = await cn.QuerySingleAsync<Producto>(
            "SELECT * FROM productos WHERE id=@productoId FOR UPDATE",
            new { productoId }, tx);

        var nuevo = prod.StockActual + delta;
        if (nuevo < 0 && !permitirNegativo)
            throw new StockInsuficienteException(prod.Nombre, prod.StockActual);

        await cn.ExecuteAsync(
            "UPDATE productos SET stock_actual=@nuevo WHERE id=@productoId",
            new { nuevo, productoId }, tx);

        await cn.ExecuteAsync(
            @"INSERT INTO movimientos_stock
              (producto_id,tipo,cantidad,stock_anterior,stock_nuevo,
               referencia_tipo,referencia_id,motivo,fecha)
              VALUES (@productoId,@tipo,@cantidad,@anterior,@nuevo,
                      @refTipo,@refId,@motivo,@fecha)",
            new
            {
                productoId,
                tipo,
                cantidad = Math.Abs(delta),
                anterior = prod.StockActual,
                nuevo,
                refTipo,
                refId,
                motivo,
                fecha = DateTime.Now
            }, tx);

        return prod;
    }
}