using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Optimizador estático de memoria RAM y descriptores GDI+ para procesadores modestos (Core i3 / DDR3).
    /// Ejecuta recolección forzada tras cerrar ventanas modales y compacta el Working Set del proceso.
    /// </summary>
    public static class Optimizador
    {
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        public static void LiberarMemoria()
        {
            try
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    using (var procesoActual = Process.GetCurrentProcess())
                    {
                        SetProcessWorkingSetSize(procesoActual.Handle, -1, -1);
                    }
                }
            }
            catch
            {
                // Ignorar si el sistema operativo restringe el ajuste del Working Set
            }
        }
    }
}
