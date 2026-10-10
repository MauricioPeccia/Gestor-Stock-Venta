namespace Kiosco.Entidades;

public class VentaItem
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = "";
    public string Unidad { get; set; } = "UN";
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Math.Round(Cantidad * PrecioUnitario, 2);
}

