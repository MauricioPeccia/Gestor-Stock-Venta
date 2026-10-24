using System;
using System.Data;
using MySql.Data.MySqlClient;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaDatos
{
    /// <summary>
    /// DAO para el registro de movimientos de caja (Gastos, Retiros, Ingresos y Compras de Mercadería).
    /// Sincroniza el aumento de stock oficial mediante 'ingresos_mercaderia' cuando corresponde.
    /// </summary>
    public class MovimientoDatos
    {
        /// <summary>
        /// Registra un movimiento de caja en MySQL dentro de una transacción atómica.
        /// Si es una Compra de Mercadería con producto asociado, inserta también en 'ingresos_mercaderia'
        /// para que el trigger 'trg_sumar_stock_ingreso' aumente el stock oficial.
        /// </summary>
        public bool RegistrarMovimiento(MovimientoCaja movimiento)
        {
            if (movimiento == null)
                throw new ArgumentNullException(nameof(movimiento), "El movimiento no puede ser nulo.");

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlTransaction transaccion = conn.BeginTransaction())
            {
                try
                {
                    const string sqlMov = @"
                        INSERT INTO movimientos_caja (tipo_movimiento, concepto, monto, fecha) 
                        VALUES (@tipo_movimiento, @concepto, @monto, @fecha);";

                    int filasAfectadas;
                    using (MySqlCommand cmd = new MySqlCommand(sqlMov, conn, transaccion))
                    {
                        cmd.Parameters.Add("@tipo_movimiento", MySqlDbType.VarChar, 50).Value = movimiento.TipoMovimiento;
                        cmd.Parameters.Add("@concepto", MySqlDbType.VarChar, 200).Value = movimiento.Concepto;
                        cmd.Parameters.Add("@monto", MySqlDbType.Decimal).Value = movimiento.Monto;
                        cmd.Parameters.Add("@fecha", MySqlDbType.DateTime).Value =
                            movimiento.Fecha == default ? DateTime.Now : movimiento.Fecha;

                        filasAfectadas = cmd.ExecuteNonQuery();
                    }

                    // Si es Compra de Mercadería y se vinculó un producto con cantidad > 0, sincronizar stock oficial
                    if (movimiento.IdProductoCompra.HasValue &&
                        movimiento.IdProductoCompra.Value > 0 &&
                        movimiento.CantidadCompra > 0)
                    {
                        decimal stockPrevio = ObtenerStockEnTransaccion(conn, transaccion, movimiento.IdProductoCompra.Value);

                        const string sqlIngreso = @"
                            INSERT INTO ingresos_mercaderia 
                                (id_producto, cantidad, precio_costo_unitario, proveedor, observaciones)
                            VALUES 
                                (@id_producto, @cantidad, @precio_costo_unitario, @proveedor, @observaciones);";

                        using (MySqlCommand cmdIng = new MySqlCommand(sqlIngreso, conn, transaccion))
                        {
                            cmdIng.Parameters.Add("@id_producto", MySqlDbType.Int32).Value = movimiento.IdProductoCompra.Value;
                            cmdIng.Parameters.Add("@cantidad", MySqlDbType.Decimal).Value = movimiento.CantidadCompra;
                            cmdIng.Parameters.Add("@precio_costo_unitario", MySqlDbType.Decimal).Value =
                                movimiento.PrecioCostoUnitario.HasValue && movimiento.PrecioCostoUnitario.Value > 0
                                    ? movimiento.PrecioCostoUnitario.Value
                                    : DBNull.Value;
                            cmdIng.Parameters.Add("@proveedor", MySqlDbType.VarChar, 120).Value =
                                string.IsNullOrWhiteSpace(movimiento.Proveedor) ? DBNull.Value : movimiento.Proveedor;
                            cmdIng.Parameters.Add("@observaciones", MySqlDbType.VarChar, 255).Value = movimiento.Concepto;

                            cmdIng.ExecuteNonQuery();
                        }

                        decimal stockPosterior = ObtenerStockEnTransaccion(conn, transaccion, movimiento.IdProductoCompra.Value);

                        // Respaldo defensivo en caso de que el trigger trg_sumar_stock_ingreso no esté activo
                        if (stockPosterior == stockPrevio)
                        {
                            const string sqlUpdateStock = @"
                                UPDATE productos 
                                SET stock_actual = stock_actual + @cantidad 
                                WHERE id_producto = @id_producto;";

                            using (MySqlCommand cmdUpd = new MySqlCommand(sqlUpdateStock, conn, transaccion))
                            {
                                cmdUpd.Parameters.Add("@cantidad", MySqlDbType.Decimal).Value = movimiento.CantidadCompra;
                                cmdUpd.Parameters.Add("@id_producto", MySqlDbType.Int32).Value = movimiento.IdProductoCompra.Value;

                                if (cmdUpd.ExecuteNonQuery() <= 0)
                                    throw new InvalidOperationException("No se pudo actualizar el stock del producto comprado.");
                            }
                        }
                    }

                    transaccion.Commit();
                    return filasAfectadas > 0;
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtiene el total acumulado de egresos de caja (Gastos, Retiros y Compras de Mercadería) del día actual.
        /// </summary>
        public decimal ObtenerTotalEgresosCajaHoy()
        {
            const string sql = @"
                SELECT COALESCE(SUM(monto), 0.00)
                FROM movimientos_caja
                WHERE DATE(fecha) = CURDATE()
                  AND tipo_movimiento IN ('Gasto', 'Retiro', 'Compra Mercaderia', 'Compra de Mercaderia', 'Compra de Mercadería');";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                object? resultado = cmd.ExecuteScalar();
                return resultado != null && resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0m;
            }
        }

        private static decimal ObtenerStockEnTransaccion(MySqlConnection conn, MySqlTransaction transaccion, int idProducto)
        {
            const string sql = "SELECT stock_actual FROM productos WHERE id_producto = @idProducto FOR UPDATE;";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaccion))
            {
                cmd.Parameters.Add("@idProducto", MySqlDbType.Int32).Value = idProducto;
                object? res = cmd.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                    throw new InvalidOperationException($"El producto con ID {idProducto} no existe en la base de datos.");

                return Convert.ToDecimal(res);
            }
        }
    }
}
