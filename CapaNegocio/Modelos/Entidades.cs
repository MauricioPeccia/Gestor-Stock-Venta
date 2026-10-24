using System;
using System.Collections.Generic;

namespace KioscoLite.CapaNegocio.Modelos
{
    public enum MetodoPago
    {
        EFECTIVO,
        TRANSFERENCIA
    }

    /// <summary>
    /// Entidad POCO que representa un artículo del catálogo e inventario.
    /// </summary>
    public class Producto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string CodigoInterno { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; } = 5m;
        public bool EsFraccionado { get; set; }
        public bool Activo { get; set; } = true;
    }

    /// <summary>
    /// Entidad POCO que representa un renglón del carrito de ventas.
    /// </summary>
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal PrecioCostoHistorico { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // Propiedades de soporte para reglas de negocio y alertas visuales
        public decimal StockActualProducto { get; set; }
        public bool EsFraccionado { get; set; }
        public bool DejaStockNegativo { get; private set; }

        public void VerificarStockNegativo(decimal stockActual)
        {
            StockActualProducto = stockActual;
            DejaStockNegativo = (stockActual - Cantidad) < 0;
        }
    }

    /// <summary>
    /// Entidad POCO que representa la cabecera y el detalle de una transacción de venta en Caja.
    /// </summary>
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string MedioPago { get; set; } = "EFECTIVO";
        public decimal Total { get; set; }
        public bool RequiereTicket { get; set; }
        public string? Observaciones { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }

    /// <summary>
    /// Entidad POCO para el registro de movimientos de caja (Gastos, Retiros, Ingresos y Compras de Mercadería).
    /// </summary>
    public class MovimientoCaja
    {
        public int IdMovimiento { get; set; }
        public string TipoMovimiento { get; set; } = "Gasto"; // 'Gasto', 'Retiro', 'Ingreso', 'Compra Mercaderia'
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Datos opcionales para sincronizar ingreso de stock en Compras de Mercadería
        public int? IdProductoCompra { get; set; }
        public decimal CantidadCompra { get; set; }
        public decimal? PrecioCostoUnitario { get; set; }
        public string? Proveedor { get; set; }
    }
}
