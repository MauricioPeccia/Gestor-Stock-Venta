using Dapper;
using Entidades;

namespace Servicios;

public class ProveedorServicio
{
    public async Task<List<Proveedor>> ListarAsync()
    {
        await using var cn = await Conexion.AbrirAsync();
        var lista = await cn.QueryAsync<Proveedor>(
            "SELECT * FROM proveedores WHERE activo=1 ORDER BY nombre");
        return lista.ToList();
    }

    public async Task<int> CrearAsync(Proveedor p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre))
            throw new NegocioException("El nombre o razón social del proveedor es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        return await cn.ExecuteScalarAsync<int>(
            @"INSERT INTO proveedores (nombre, telefono, email, direccion, activo)
              VALUES (@Nombre, @Telefono, @Email, @Direccion, 1);
              SELECT LAST_INSERT_ID();",
            new
            {
                Nombre = p.Nombre.Trim(),
                Telefono = string.IsNullOrWhiteSpace(p.Telefono) ? null : p.Telefono.Trim(),
                Email = string.IsNullOrWhiteSpace(p.Email) ? null : p.Email.Trim(),
                Direccion = string.IsNullOrWhiteSpace(p.Direccion) ? null : p.Direccion.Trim()
            });
    }

    public async Task ActualizarAsync(Proveedor p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre))
            throw new NegocioException("El nombre o razón social del proveedor es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        await cn.ExecuteAsync(
            @"UPDATE proveedores
              SET nombre=@Nombre, telefono=@Telefono, email=@Email, direccion=@Direccion
              WHERE id=@Id",
            new
            {
                Nombre = p.Nombre.Trim(),
                Telefono = string.IsNullOrWhiteSpace(p.Telefono) ? null : p.Telefono.Trim(),
                Email = string.IsNullOrWhiteSpace(p.Email) ? null : p.Email.Trim(),
                Direccion = string.IsNullOrWhiteSpace(p.Direccion) ? null : p.Direccion.Trim(),
                p.Id
            });
    }

    public async Task DesactivarAsync(int id)
    {
        await using var cn = await Conexion.AbrirAsync();
        await cn.ExecuteAsync("UPDATE proveedores SET activo=0 WHERE id=@id", new { id });
    }
}

