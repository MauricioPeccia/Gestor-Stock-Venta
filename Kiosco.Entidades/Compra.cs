namespace Kiosco.Entidades;

public class Compra
{
    public int? ProveedorId { get; set; }          // opcional
    public string? NroComprobante { get; set; }
    public string? Observaciones { get; set; }
    public bool PagadaConCaja { get; set; }
    public List<CompraItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
}

