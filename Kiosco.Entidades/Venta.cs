namespace Kiosco.Entidades;

public class Venta
{
    public long Id { get; set; }
    public decimal Descuento { get; set; }
    public List<VentaItem> Items { get; set; } = new();
    public List<PagoVenta> Pagos { get; set; } = new();
    public decimal Subtotal => Items.Sum(i => i.Subtotal);
    public decimal Total => Subtotal - Descuento;
}

