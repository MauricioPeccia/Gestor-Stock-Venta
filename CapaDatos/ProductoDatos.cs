using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaDatos
{
    /// <summary>
    /// DAO para gestión del catálogo de productos y ajustes manuales de inventario en MySQL.
    /// </summary>
    public class ProductoDatos
    {
        /// <summary>
        /// Obtiene todo el catálogo activo para poblar la grilla de inventario o selectores.
        /// </summary>
        public List<Producto> ObtenerCatalogoActivo()
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
        /// Inserta un nuevo producto en la base de datos.
        /// </summary>
        public int InsertarProducto(Producto producto)
        {
            const string sql = @"
                INSERT INTO productos 
                    (codigo_barra, codigo_interno, nombre, precio_costo, precio_venta, stock_actual, stock_minimo, es_fraccionado, activo)
                VALUES 
                    (@codigo_barra, @codigo_interno, @nombre, @precio_costo, @precio_venta, @stock_actual, @stock_minimo, @es_fraccionado, 1);
                SELECT LAST_INSERT_ID();";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codigo_barra", MySqlDbType.VarChar, 50).Value =
                    string.IsNullOrWhiteSpace(producto.CodigoBarras) ? DBNull.Value : producto.CodigoBarras.Trim();
                cmd.Parameters.Add("@codigo_interno", MySqlDbType.VarChar, 20).Value = producto.CodigoInterno.Trim();
                cmd.Parameters.Add("@nombre", MySqlDbType.VarChar, 120).Value = producto.Nombre.Trim();
                cmd.Parameters.Add("@precio_costo", MySqlDbType.Decimal).Value = producto.PrecioCosto;
                cmd.Parameters.Add("@precio_venta", MySqlDbType.Decimal).Value = producto.PrecioVenta;
                cmd.Parameters.Add("@stock_actual", MySqlDbType.Decimal).Value = producto.StockActual;
                cmd.Parameters.Add("@stock_minimo", MySqlDbType.Decimal).Value = producto.StockMinimo;
                cmd.Parameters.Add("@es_fraccionado", MySqlDbType.Bit).Value = producto.EsFraccionado ? 1 : 0;

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Actualiza los datos comerciales de un producto existente.
        /// </summary>
        public bool ActualizarProducto(Producto producto)
        {
            const string sql = @"
                UPDATE productos
                SET codigo_barra = @codigo_barra,
                    codigo_interno = @codigo_interno,
                    nombre = @nombre,
                    precio_costo = @precio_costo,
                    precio_venta = @precio_venta,
                    stock_actual = @stock_actual,
                    stock_minimo = @stock_minimo,
                    es_fraccionado = @es_fraccionado
                WHERE id_producto = @id_producto;";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codigo_barra", MySqlDbType.VarChar, 50).Value =
                    string.IsNullOrWhiteSpace(producto.CodigoBarras) ? DBNull.Value : producto.CodigoBarras.Trim();
                cmd.Parameters.Add("@codigo_interno", MySqlDbType.VarChar, 20).Value = producto.CodigoInterno.Trim();
                cmd.Parameters.Add("@nombre", MySqlDbType.VarChar, 120).Value = producto.Nombre.Trim();
                cmd.Parameters.Add("@precio_costo", MySqlDbType.Decimal).Value = producto.PrecioCosto;
                cmd.Parameters.Add("@precio_venta", MySqlDbType.Decimal).Value = producto.PrecioVenta;
                cmd.Parameters.Add("@stock_actual", MySqlDbType.Decimal).Value = producto.StockActual;
                cmd.Parameters.Add("@stock_minimo", MySqlDbType.Decimal).Value = producto.StockMinimo;
                cmd.Parameters.Add("@es_fraccionado", MySqlDbType.Bit).Value = producto.EsFraccionado ? 1 : 0;
                cmd.Parameters.Add("@id_producto", MySqlDbType.Int32).Value = producto.Id;

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Realiza la baja lógica de un producto para preservar la integridad histórica de ventas.
        /// </summary>
        public bool EliminarProducto(int idProducto)
        {
            const string sql = "UPDATE productos SET activo = 0 WHERE id_producto = @idProducto;";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@idProducto", MySqlDbType.Int32).Value = idProducto;
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Actualiza manualmente el stock de un producto por mermas, roturas o conteo físico (sin afectar la caja).
        /// </summary>
        public bool ActualizarStockManual(int idProducto, decimal nuevaCantidad)
        {
            const string sql = @"
                UPDATE productos 
                SET stock_actual = @nuevaCantidad 
                WHERE id_producto = @idProducto;";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@nuevaCantidad", MySqlDbType.Decimal).Value = nuevaCantidad;
                cmd.Parameters.Add("@idProducto", MySqlDbType.Int32).Value = idProducto;

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Obtiene un producto específico por su ID primario mediante lectura directa por ordinal.
        /// Utilizado para verificar la evolución del stock en tiempo real.
        /// </summary>
        public Producto? ObtenerProductoPorId(int idProducto)
        {
            const string sql = @"
                SELECT id_producto, codigo_barra, codigo_interno, nombre, 
                       precio_costo, precio_venta, stock_actual, stock_minimo, es_fraccionado, activo
                FROM productos 
                WHERE id_producto = @idProducto
                LIMIT 1;";

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@idProducto", MySqlDbType.Int32).Value = idProducto;

                using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        return new Producto
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
                            Activo = reader.GetBoolean(9)
                        };
                    }
                }
            }

            return null;
        }
    }
}
