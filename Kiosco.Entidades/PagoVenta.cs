namespace Kiosco.Entidades;

public class PagoVenta
{
    public string Metodo { get; set; } = "EFECTIVO";   // EFECTIVO | TRANSFERENCIA
    public decimal Monto { get; set; }
}

