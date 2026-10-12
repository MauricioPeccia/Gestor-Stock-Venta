using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ResumenJornada
    {
        public decimal MontoApertura { get; set; }
        public decimal VentasEfectivo { get; set; }
        public decimal VentasTransferencia { get; set; }
        public int CantidadVentas { get; set; }
        public int CantidadAnuladas { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal GananciaBruta { get; set; }

        public decimal EfectivoEsperado => MontoApertura + VentasEfectivo + Ingresos - Egresos;
        public decimal TotalVendido => VentasEfectivo + VentasTransferencia;
    }
}
