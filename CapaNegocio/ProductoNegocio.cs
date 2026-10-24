using System;
using System.Collections.Generic;
using KioscoLite.CapaDatos;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaNegocio
{
    /// <summary>
    /// Servicio de negocio para la gestión de Inventario, ABM de productos y ajustes manuales.
    /// </summary>
    public class ProductoNegocio
    {
        private readonly ProductoDatos _productoDatos;

        public ProductoNegocio()
        {
            _productoDatos = new ProductoDatos();
        }

        public List<Producto> ObtenerCatalogoActivo()
        {
            return _productoDatos.ObtenerCatalogoActivo();
        }

        public Producto? ObtenerProductoPorId(int idProducto)
        {
            if (idProducto <= 0) return null;
            return _productoDatos.ObtenerProductoPorId(idProducto);
        }

        public int CrearProducto(Producto producto)
        {
            ValidarProducto(producto);
            return _productoDatos.InsertarProducto(producto);
        }

        public bool ActualizarProducto(Producto producto)
        {
            if (producto == null || producto.Id <= 0)
                throw new ArgumentException("El identificador del producto no es válido.");

            ValidarProducto(producto);
            return _productoDatos.ActualizarProducto(producto);
        }

        public bool EliminarProducto(int idProducto)
        {
            if (idProducto <= 0)
                throw new ArgumentException("Seleccione un producto válido para eliminar.");

            return _productoDatos.EliminarProducto(idProducto);
        }

        public bool ActualizarStockManual(int idProducto, decimal nuevaCantidad)
        {
            if (idProducto <= 0)
                throw new ArgumentException("Producto inválido.");

            return _productoDatos.ActualizarStockManual(idProducto, nuevaCantidad);
        }

        private static void ValidarProducto(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new InvalidOperationException("El nombre del producto es obligatorio.");

            if (producto.PrecioCosto < 0)
                throw new InvalidOperationException("El precio de costo no puede ser negativo.");

            if (producto.PrecioVenta <= 0)
                throw new InvalidOperationException("El precio de venta debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(producto.CodigoInterno))
            {
                producto.CodigoInterno = !string.IsNullOrWhiteSpace(producto.CodigoBarras)
                    ? (producto.CodigoBarras.Length > 20 ? producto.CodigoBarras.Substring(0, 20) : producto.CodigoBarras)
                    : $"PRD-{DateTime.Now:MMddHHmmss}";
            }
        }
    }
}
