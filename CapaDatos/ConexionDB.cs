using System;
using System.Configuration;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace KioscoLite.CapaDatos
{
    /// <summary>
    /// Gestor Singleton ligero y clase de acceso centralizado para la conexión a MySQL 8.x (kiosco_db).
    /// Externaliza el 100% de la cadena de conexión hacia App.config mediante ConfigurationManager,
    /// con manejo robusto de excepciones si el archivo de configuración no existe o es inválido.
    /// </summary>
    public sealed class ConexionDB
    {
        private const string NombreCadenaConexion = "KioscoDB";

        private static readonly Lazy<ConexionDB> _instancia =
            new Lazy<ConexionDB>(() => new ConexionDB());

        private readonly string _cadenaConexion;

        public static ConexionDB Instancia => _instancia.Value;

        public string CadenaConexion => _cadenaConexion;

        // Metadatos desglosados de solo lectura para utilidades de infraestructura (e.g., mysqldump)
        public string Servidor { get; }
        public uint Puerto { get; }
        public string BaseDatos { get; }
        public string Usuario { get; }
        public string Password { get; }
        public string DirectorioBackups { get; }
        public string RutaMySqlDumpConfigurada { get; }

        private ConexionDB()
        {
            try
            {
                string cadenaConfigurada = ObtenerCadenaDesdeAppConfig(
                    out string dirBackups,
                    out string rutaDump);

                DirectorioBackups = dirBackups;
                RutaMySqlDumpConfigurada = rutaDump;

                var builder = new MySqlConnectionStringBuilder(cadenaConfigurada);
                if (string.IsNullOrWhiteSpace(builder.CharacterSet))
                {
                    builder.CharacterSet = "utf8mb4";
                }

                Servidor = builder.Server;
                Puerto = builder.Port == 0 ? 3306u : builder.Port;
                BaseDatos = builder.Database;
                Usuario = builder.UserID;
                Password = builder.Password ?? string.Empty;

                _cadenaConexion = builder.ConnectionString;
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(
                    "Error crítico al inicializar la configuración de base de datos desde App.config: " + ex.Message,
                    ex);
            }
        }

        /// <summary>
        /// Lee la cadena de conexión 'KioscoDB' desde App.config (o KioscoLite.dll.config) usando ConfigurationManager.
        /// Valida explícitamente la existencia del archivo de configuración y de la sección connectionStrings.
        /// </summary>
        private static string ObtenerCadenaDesdeAppConfig(out string directorioBackups, out string rutaMySqlDump)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string rutaDllConfig = Path.Combine(baseDir, "KioscoLite.dll.config");
            string rutaExeConfig = Path.Combine(baseDir, "KioscoLite.exe.config");
            string rutaAppConfig = Path.Combine(baseDir, "App.config");

            // intentamos leer mediante ConfigurationManager estándar (.dll.config en .NET 8)
            ConnectionStringSettings? settings = ConfigurationManager.ConnectionStrings[NombreCadenaConexion];
            string? dirCfg = ConfigurationManager.AppSettings["DirectorioBackups"];
            string? dumpCfg = ConfigurationManager.AppSettings["RutaMySqlDump"];

            // Si el técnico editó directamente el archivo 'App.config' o 'KioscoLite.exe.config' en la carpeta instalada,
            // cargarlo explícitamente con OpenMappedExeConfiguration
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                string? archivoAlternativo = File.Exists(rutaAppConfig)
                    ? rutaAppConfig
                    : (File.Exists(rutaExeConfig) ? rutaExeConfig : (File.Exists(rutaDllConfig) ? rutaDllConfig : null));

                if (archivoAlternativo == null)
                {
                    throw new FileNotFoundException(
                        $"No se encontró el archivo de configuración 'App.config' (ni 'KioscoLite.dll.config') en la ruta de instalación: {baseDir}");
                }

                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = archivoAlternativo };
                Configuration configMapeada = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                settings = configMapeada.ConnectionStrings.ConnectionStrings[NombreCadenaConexion];
                dirCfg = configMapeada.AppSettings.Settings["DirectorioBackups"]?.Value;
                dumpCfg = configMapeada.AppSettings.Settings["RutaMySqlDump"]?.Value;
            }

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    $"No se encontró la cadena de conexión '{NombreCadenaConexion}' dentro de la sección <connectionStrings> de App.config.");
            }

            directorioBackups = string.IsNullOrWhiteSpace(dirCfg) ? "Backups" : dirCfg;
            rutaMySqlDump = dumpCfg ?? string.Empty;

            return settings.ConnectionString;
        }

        /// <summary>
        /// Crea, abre y retorna una nueva instancia de <see cref="MySqlConnection"/> lista para operar.
        /// Debe envolverse siempre en un bloque <c>using</c> para su devolución inmediata al pool.
        /// </summary>
        public static MySqlConnection ObtenerConexion()
        {
            var conexion = new MySqlConnection(Instancia._cadenaConexion);
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
                return conexion;
            }
            catch
            {
                conexion.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Crea y retorna una conexión cerrada para control manual de apertura dentro de un bloque <c>using</c>.
        /// </summary>
        public MySqlConnection CrearConexion()
        {
            return new MySqlConnection(_cadenaConexion);
        }

        /// <summary>
        /// Verifica rápidamente la disponibilidad del servidor MySQL sin dejar conexiones abiertas.
        /// </summary>
        public bool ProbarConexion(out string mensajeError)
        {
            try
            {
                using (var conexion = ObtenerConexion())
                {
                    mensajeError = string.Empty;
                    return conexion.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Limpia todos los pools de conexiones al cerrar la aplicación para evitar sesiones fantasma en MySQL.
        /// </summary>
        public static void CerrarPoolConexiones()
        {
            try
            {
                MySqlConnection.ClearAllPools();
            }
            catch
            {
                // Ignorar errores de red durante el cierre del proceso
            }
        }
    }
}
