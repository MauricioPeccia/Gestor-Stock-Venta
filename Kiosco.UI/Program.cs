using Kiosco.Servicios;
using Microsoft.Extensions.Configuration;

namespace Kiosco.UI;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Dapper: precio_venta -> PrecioVenta
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        Conexion.Cadena = config.GetConnectionString("Default")!;

        // Log de errores no controlados
        Application.ThreadException += (s, e) => RegistrarError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (s, e) => RegistrarError((Exception)e.ExceptionObject);

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        Application.Run(new FrmPrincipal());
    }

    static void RegistrarError(Exception ex)
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "errores.log");
        File.AppendAllText(ruta, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}\n\n");
        MessageBox.Show("Ocurrió un error inesperado. Los detalles quedaron guardados en errores.log.",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}