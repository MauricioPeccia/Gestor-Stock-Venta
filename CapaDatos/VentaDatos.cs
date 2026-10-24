using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaDatos
{
    /// <summary>
    /// DAO para transacciones de Caja (Ventas) y carga de caché de productos en memoria.
    /// Implementado en ADO.NET puro con lecturas por ordinal y transacciones ACID.
    /// </summary>
    public class VentaDatos
    {
        /// <summary>
        /// Obtiene todos los productos activos para la caché rápida en memoria de la terminal de ventas.
        /// </summary>
        public List<Producto> ObtenerTodosLosProductos()
        {
            var lista = new List<Producto>();

            const string sql = @"
                SELECT id_producto, codigo_barra, codigo_interno, nombre, 
                       precio_costo, precio_venta, stock_actual, stock_minimo, es_fraccionado
                FROM productos
                WHERE activo = 1
                ORDER BY nombre ASC;";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        Id = reader.GetInt32(0),
                        CodigoBarras = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        CodigoInterno = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Nombre = reader.GetString(3),
                        PrecioCosto = reader.GetDecimal(4),
                        PrecioVenta = reader.GetDecimal(5),
                        StockActual = reader.GetDecimal(6),
                        StockMinimo = reader.GetDecimal(7),
                        EsFraccionado = reader.GetBoolean(8),
                        Activo = true
                    });
                }
            }

            return lista;
        }

        /// <summary>
        /// Inserta la cabecera y el detalle de la venta dentro de una transacción atómica de MySQL.
        /// El trigger 'trg_descontar_stock_venta' de kiosco_db descuenta automáticamente el stock (permitiendo negativos).
        /// </summary>
        public int InsertarVenta(Venta venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlTransaction transaccion = conn.BeginTransaction())
            {
                try
                {
                    const string sqlVenta = @"
                        INSERT INTO ventas (fecha_hora, metodo_pago, total, requiere_ticket, observaciones)
                        VALUES (@fecha_hora, @metodo_pago, @total, @requiere_ticket, @observaciones);
                        SELECT LAST_INSERT_ID();";

                    int idVentaGenerada;
                    string metodoPagoEnum = string.Equals(venta.MedioPago, "Transferencia", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(venta.MedioPago, "TRANSFERENCIA", StringComparison.OrdinalIgnoreCase)
                        ? "TRANSFERENCIA"
                        : "EFECTIVO";

                    using (MySqlCommand cmdVenta = new MySqlCommand(sqlVenta, conn, transaccion))
                    {
                        cmdVenta.Parameters.Add("@fecha_hora", MySqlDbType.DateTime).Value = venta.Fecha;
                        cmdVenta.Parameters.Add("@metodo_pago", MySqlDbType.VarChar, 20).Value = metodoPagoEnum;
                        cmdVenta.Parameters.Add("@total", MySqlDbType.Decimal).Value = venta.Total;
                        cmdVenta.Parameters.Add("@requiere_ticket", MySqlDbType.Bit).Value = venta.RequiereTicket ? 1 : 0;
                        cmdVenta.Parameters.Add("@observaciones", MySqlDbType.VarChar, 255).Value =
                            string.IsNullOrWhiteSpace(venta.Observaciones) ? DBNull.Value : venta.Observaciones;

                        idVentaGenerada = Convert.ToInt32(cmdVenta.ExecuteScalar());
                    }

                    const string sqlDetalle = @"
                        INSERT INTO detalle_ventas 
                            (id_venta, id_producto, cantidad, precio_costo_historico, precio_venta_unitario, subtotal)
                        VALUES 
                            (@id_venta, @id_producto, @cantidad, @precio_costo_historico, @precio_venta_unitario, @subtotal);";

                    foreach (var det in venta.Detalles)
                    {
                        decimal stockPrevio = ObtenerStockEnTransaccion(conn, transaccion, det.IdProducto);

                        using (MySqlCommand cmdDetalle = new MySqlCommand(sqlDetalle, conn, transaccion))
                        {
                            cmdDetalle.Parameters.Add("@id_venta", MySqlDbType.Int32).Value = idVentaGenerada;
                            cmdDetalle.Parameters.Add("@id_producto", MySqlDbType.Int32).Value = det.IdProducto;
                            cmdDetalle.Parameters.Add("@cantidad", MySqlDbType.Decimal).Value = det.Cantidad;
                            cmdDetalle.Parameters.Add("@precio_costo_historico", MySqlDbType.Decimal).Value = det.PrecioCostoHistorico;
                            cmdDetalle.Parameters.Add("@precio_venta_unitario", MySqlDbType.Decimal).Value = det.PrecioUnitario;
                            cmdDetalle.Parameters.Add("@subtotal", MySqlDbType.Decimal).Value = det.Subtotal;

                            cmdDetalle.ExecuteNonQuery();
                        }

                        decimal stockPosterior = ObtenerStockEnTransaccion(conn, transaccion, det.IdProducto);

                        // Respaldo defensivo en caso de que el trigger trg_descontar_stock_venta no esté activo
                        if (stockPosterior == stockPrevio)
                        {
                            const string sqlDescontarStock = @"
                                UPDATE productos 
                                SET stock_actual = stock_actual - @cantidad 
                                WHERE id_producto = @id_producto;";

                            using (MySqlCommand cmdUpd = new MySqlCommand(sqlDescontarStock, conn, transaccion))
                            {
                                cmdUpd.Parameters.Add("@cantidad", MySqlDbType.Decimal).Value = det.Cantidad;
                                cmdUpd.Parameters.Add("@id_producto", MySqlDbType.Int32).Value = det.IdProducto;

                                if (cmdUpd.ExecuteNonQuery() <= 0)
                                    throw new InvalidOperationException("No se pudo descontar el stock del producto vendido.");
                            }
                        }
                    }

                    transaccion.Commit();
                    return idVentaGenerada;
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtiene el subtotal acumulado de ventas del día actual desde la vista v_cierre_caja_diario (o tabla ventas).
        /// </summary>
        public decimal ObtenerSubtotalVentasHoy()
        {
            const string sql = @"
                SELECT COALESCE(SUM(total_recaudado), 0.00)
                FROM v_cierre_caja_diario
                WHERE fecha = CURDATE();";

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
