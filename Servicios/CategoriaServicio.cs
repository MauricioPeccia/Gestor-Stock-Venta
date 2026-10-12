using Dapper;
using Entidades;

namespace Servicios;

public class CategoriaServicio
{
    public async Task<List<Categoria>> ListarAsync()
    {
        await using var cn = await Conexion.AbrirAsync();
        var lista = await cn.QueryAsync<Categoria>(
            "SELECT * FROM categorias WHERE activo=1 ORDER BY nombre");
        return lista.ToList();
    }

    public async Task<int> CrearAsync(Categoria c)
    {
        if (string.IsNullOrWhiteSpace(c.Nombre))
            throw new NegocioException("El nombre de la categoría es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        return await cn.ExecuteScalarAsync<int>(
            "INSERT INTO categorias (nombre, activo) VALUES (@Nombre, 1); SELECT LAST_INSERT_ID();",
            new { Nombre = c.Nombre.Trim() });
    }

    public async Task ActualizarAsync(Categoria c)
    {
        if (string.IsNullOrWhiteSpace(c.Nombre))
            throw new NegocioException("El nombre de la categoría es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        await cn.ExecuteAsync(
            "UPDATE categorias SET nombre=@Nombre WHERE id=@Id",
            new { Nombre = c.Nombre.Trim(), c.Id });
    }

    public async Task DesactivarAsync(int id)
    {
        await using var cn = await Conexion.AbrirAsync();
        await cn.ExecuteAsync("UPDATE categorias SET activo=0 WHERE id=@id", new { id });
    }
}

