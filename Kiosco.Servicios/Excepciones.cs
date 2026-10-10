namespace Kiosco.Servicios;

public class StockInsuficienteException : Exception
{
    public string Producto { get; }
    public decimal Disponible { get; }
    public StockInsuficienteException(string producto, decimal disponible)
        : base($"Stock insuficiente de '{producto}'. Disponible: {disponible}")
    {
        Producto = producto;
        Disponible = disponible;
    }
}

public class NegocioException : Exception
{
    public NegocioException(string mensaje) : base(mensaje) { }
}

