namespace Kiosco.Entidades;

public class CompraItem
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal Subtotal => Math.Round(Cantidad * CostoUnitario, 2);
}

