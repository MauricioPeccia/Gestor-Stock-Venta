using Dapper;
using Kiosco.Entidades;

namespace Kiosco.Servicios;

public class CategoriaServicio
{
    public async Task<List<Categoria>> ObtenerTodasAsync()
    {
        try
        {
            await using var cn = await Conexion.AbrirAsync();
            var res = await cn.QueryAsync<Categoria>("SELECT * FROM categorias WHERE activo=1 ORDER BY nombre");
            return res.ToList();
        }
        catch
        {
            return new List<Categoria>();
        }
    }
}

