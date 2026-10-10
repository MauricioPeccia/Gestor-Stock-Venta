namespace Kiosco.Entidades;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public bool Activo { get; set; } = true;

    public override string ToString() => Nombre;
}

