using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class JornadaCaja
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime AperturaEn { get; set; }
        public decimal MontoApertura { get; set; }
        public DateTime? CierreEn { get; set; }
        public decimal? EfectivoEsperado { get; set; }
        public decimal? EfectivoContado { get; set; }
        public decimal? Diferencia { get; set; }
        public string Estado { get; set; } = "ABIERTA";
        public string? Observaciones { get; set; }
    }
}
