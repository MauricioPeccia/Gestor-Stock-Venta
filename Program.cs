using System;
using System.Windows.Forms;
using KioscoLite.CapaDatos;
using KioscoLite.CapaPresentacion;

namespace KioscoLite
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para Kiosco Lite.
        /// Inicializa los recursos GDI+ compartidos, ejecuta el menú radial y libera toda la memoria al cerrar.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Pre-carga única de fuentes, colores y pinceles GDI+ en memoria
            RecursosUI.Inicializar();

            try
            {
                // 2. Arranque de la ventana principal con disposición garantizada al salir
                using (var formPrincipal = new FormPrincipal())
                {
                    Application.Run(formPrincipal);
                }
            }
            finally
            {
                // 3. Liberación determinista de conexiones MySQL, objetos GDI+ y compactación de memoria RAM
                ConexionDB.CerrarPoolConexiones();
                RecursosUI.LiberarRecursos();
                Optimizador.LiberarMemoria();
            }
        }
    }
}