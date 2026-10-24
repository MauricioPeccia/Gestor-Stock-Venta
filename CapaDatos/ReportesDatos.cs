using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace KioscoLite.CapaDatos
{
    /// <summary>
    /// DAO de solo lectura para consultar las vistas analíticas preexistentes en kiosco_db.
    /// </summary>
    public class ReportesDatos
    {
        /// <summary>
        /// Obtiene el resumen del cierre de caja diario (vista SQL: v_cierre_caja_diario).
        /// </summary>
        public DataTable ObtenerCierreDiario()
        {
            return EjecutarVista("SELECT * FROM v_cierre_caja_diario;");
        }

        /// <summary>
        /// Obtiene el ranking de los 10 productos más vendidos del mes (vista SQL: v_top10_productos_mes).
        /// </summary>
        public DataTable ObtenerTop10Productos()
        {
            return EjecutarVista("SELECT * FROM v_top10_productos_mes;");
        }

        /// <summary>
        /// Obtiene el reporte de ganancias mensuales (vista SQL: v_reporte_ganancia_mensual).
        /// </summary>
        public DataTable ObtenerGananciasMensuales()
        {
            return EjecutarVista("SELECT * FROM v_reporte_ganancia_mensual;");
        }

        private static DataTable EjecutarVista(string sql)
        {
            var tabla = new DataTable();

            using (MySqlConnection conn = ConexionDB.ObtenerConexion())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                da.Fill(tabla);
            }

            return tabla;
        }
    }
}
