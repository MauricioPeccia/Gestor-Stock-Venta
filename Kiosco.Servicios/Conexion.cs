using MySqlConnector;

namespace Kiosco.Servicios;

public static class Conexion
{
    public static string Cadena { get; set; } = "";

    public static async Task<MySqlConnection> AbrirAsync()
    {
        var cn = new MySqlConnection(Cadena);
        await cn.OpenAsync();
        return cn;
    }
}

