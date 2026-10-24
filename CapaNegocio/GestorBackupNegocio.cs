using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KioscoLite.CapaDatos;

namespace KioscoLite.CapaNegocio
{
    /// <summary>
    /// DTO ligero con el resultado de la operación de respaldo silencioso.
    /// </summary>
    public sealed class ResultadoBackup
    {
        public bool Exito { get; init; }
        public bool OmitidoPorExistirHoy { get; init; }
        public string RutaArchivo { get; init; } = string.Empty;
        public string Mensaje { get; init; } = string.Empty;
    }

    /// <summary>
    /// Servicio de negocio encargado de ejecutar respaldos automatizados y silenciosos de 'kiosco_db'
    /// invocando 'mysqldump.exe' en un hilo secundario sin bloquear la interfaz gráfica.
    /// </summary>
    public class GestorBackupNegocio
    {
        private static int _backupEnCurso;

        /// <summary>
        /// Ejecuta un respaldo completo (.sql) de la base de datos en segundo plano (Task.Run)
        /// utilizando mysqldump.exe de forma 100% silenciosa (CreateNoWindow = true, UseShellExecute = false).
        /// </summary>
        /// <param name="soloSiNoExisteHoy">
        /// Si es true, verifica primero si ya existe un backup del día actual para evitar ejecuciones redundantes
        /// al abrir repetidamente el Cierre de Caja Diario.
        /// </param>
        public Task<ResultadoBackup> EjecutarRespaldoSilencioso(bool soloSiNoExisteHoy = false)
        {
            return Task.Run(() => EjecutarDumpInterno(soloSiNoExisteHoy));
        }

        private static ResultadoBackup EjecutarDumpInterno(bool soloSiNoExisteHoy)
        {
            // Evitar colisiones si ya hay un proceso mysqldump corriendo en segundo plano
            if (Interlocked.CompareExchange(ref _backupEnCurso, 1, 0) != 0)
            {
                return new ResultadoBackup
                {
                    Exito = false,
                    Mensaje = "Ya hay un respaldo en ejecución en segundo plano."
                };
            }

            try
            {
                ConexionDB cfg = ConexionDB.Instancia;

                string carpetaBase = Path.IsPathRooted(cfg.DirectorioBackups)
                    ? cfg.DirectorioBackups
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfg.DirectorioBackups);

                if (!Directory.Exists(carpetaBase))
                {
                    Directory.CreateDirectory(carpetaBase);
                }

                // Si es disparo automático por Cierre de Caja Diario, comprobar si ya se respaldó hoy
                string prefijoHoy = $"kiosco_db_{DateTime.Now:yyyyMMdd}_";
                if (soloSiNoExisteHoy)
                {
                    string[] existentesHoy = Directory.GetFiles(carpetaBase, $"{prefijoHoy}*.sql");
                    if (existentesHoy.Length > 0)
                    {
                        return new ResultadoBackup
                        {
                            Exito = true,
                            OmitidoPorExistirHoy = true,
                            RutaArchivo = existentesHoy[existentesHoy.Length - 1],
                            Mensaje = $"Backup diario al día ({Path.GetFileName(existentesHoy[existentesHoy.Length - 1])})"
                        };
                    }
                }

                string rutaEjecutableDump = ResolverRutaMySqlDump(cfg.RutaMySqlDumpConfigurada);
                string nombreArchivo = $"kiosco_db_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                string rutaArchivoDestino = Path.Combine(carpetaBase, nombreArchivo);

                // Construcción de argumentos para mysqldump.exe:
                // --single-transaction: dump consistente en InnoDB sin bloquear tablas de ventas
                // --routines --triggers --events: preserva los triggers de stock (trg_descontar_stock_venta / trg_sumar_stock_ingreso)
                string argumentos =
                    $"--host=\"{cfg.Servidor}\" " +
                    $"--port={cfg.Puerto} " +
                    $"--user=\"{cfg.Usuario}\" " +
                    $"--password=\"{cfg.Password}\" " +
                    "--single-transaction " +
                    "--routines " +
                    "--triggers " +
                    "--events " +
                    "--default-character-set=utf8mb4 " +
                    $"--result-file=\"{rutaArchivoDestino}\" " +
                    $"\"{cfg.BaseDatos}\"";

                var psi = new ProcessStartInfo
                {
                    FileName = rutaEjecutableDump,
                    Arguments = argumentos,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using (var proceso = new Process { StartInfo = psi })
                {
                    proceso.Start();

                    string stderr = proceso.StandardError.ReadToEnd();
                    bool terminoATiempo = proceso.WaitForExit(30000); // Timeout defensivo de 30s

                    if (!terminoATiempo)
                    {
                        try { proceso.Kill(); } catch { /* Ignorar */ }
                        return new ResultadoBackup
                        {
                            Exito = false,
                            Mensaje = "Tiempo de espera agotado al ejecutar mysqldump.exe."
                        };
                    }

                    if (proceso.ExitCode != 0)
                    {
                        return new ResultadoBackup
                        {
                            Exito = false,
                            Mensaje = $"Error mysqldump (Código {proceso.ExitCode}): {LimpiarAdvertenciaPassword(stderr)}"
                        };
                    }
                }

                var infoArchivo = new FileInfo(rutaArchivoDestino);
                if (!infoArchivo.Exists || infoArchivo.Length == 0)
                {
                    return new ResultadoBackup
                    {
                        Exito = false,
                        Mensaje = "El archivo de respaldo se generó vacío o no pudo escribirse en disco."
                    };
                }

                double tamanoKb = Math.Round(infoArchivo.Length / 1024.0, 1);
                return new ResultadoBackup
                {
                    Exito = true,
                    RutaArchivo = rutaArchivoDestino,
                    Mensaje = $"Backup OK: {nombreArchivo} ({tamanoKb:N1} KB)"
                };
            }
            catch (Exception ex)
            {
                return new ResultadoBackup
                {
                    Exito = false,
                    Mensaje = $"No se pudo ejecutar mysqldump: {ex.Message}"
                };
            }
            finally
            {
                Interlocked.Exchange(ref _backupEnCurso, 0);
            }
        }

        /// <summary>
        /// Localiza mysqldump.exe en la ruta configurada, instalaciones estándar de MySQL 8.x en Windows o variable PATH.
        /// </summary>
        private static string ResolverRutaMySqlDump(string rutaConfigurada)
        {
            if (!string.IsNullOrWhiteSpace(rutaConfigurada) && File.Exists(rutaConfigurada))
                return rutaConfigurada;

            string[] rutasComunes =
            {
                @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\MySQL Server 8.4\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\MySQL Server 9.0\bin\mysqldump.exe",
                @"C:\xampp\mysql\bin\mysqldump.exe"
            };

            foreach (string ruta in rutasComunes)
            {
                if (File.Exists(ruta))
                    return ruta;
            }

            // Búsqueda dinámica en C:\Program Files\MySQL\*\bin\mysqldump.exe
            try
            {
                string programFilesMySql = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "MySQL");

                if (Directory.Exists(programFilesMySql))
                {
                    string[] encontrados = Directory.GetFiles(programFilesMySql, "mysqldump.exe", SearchOption.AllDirectories);
                    if (encontrados.Length > 0)
                        return encontrados[0];
                }
            }
            catch
            {
                // Continuar al fallback en PATH
            }

            return "mysqldump.exe";
        }

        private static string LimpiarAdvertenciaPassword(string stderr)
        {
            if (string.IsNullOrWhiteSpace(stderr))
                return "Error desconocido en mysqldump.";

            string[] lineas = stderr.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string linea in lineas)
            {
                if (!linea.Contains("Using a password on the command line interface can be insecure", StringComparison.OrdinalIgnoreCase))
                {
                    return linea.Trim();
                }
            }

            return stderr.Trim();
        }
    }
}
