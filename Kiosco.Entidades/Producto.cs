namespace Kiosco.Entidades;

public class Producto
{
    public int Id { get; set; }
    public string? Codigo { get; set; }
    public string Nombre { get; set; } = "";
    public int? CategoriaId { get; set; }
    public string Unidad { get; set; } = "UN";   // UN, KG, LT
    public decimal PrecioCosto { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public bool Activo { get; set; } = true;
}

