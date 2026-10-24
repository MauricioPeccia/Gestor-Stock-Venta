using System.Data;
using KioscoLite.CapaDatos;

namespace KioscoLite.CapaNegocio
{
    /// <summary>
    /// Servicio de negocio para la consulta de Reportes analíticos.
    /// </summary>
    public class ReportesNegocio
    {
        private readonly ReportesDatos _reportesDatos;

        public ReportesNegocio()
        {
            _reportesDatos = new ReportesDatos();
        }

        public DataTable ObtenerCierreDiario() => _reportesDatos.ObtenerCierreDiario();

        public DataTable ObtenerTop10Productos() => _reportesDatos.ObtenerTop10Productos();

        public DataTable ObtenerGananciasMensuales() => _reportesDatos.ObtenerGananciasMensuales();
    }
}
