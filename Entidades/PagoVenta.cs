using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PagoVenta
    {
        public string Metodo { get; set; } = "EFECTIVO";   // EFECTIVO | TRANSFERENCIA
        public decimal Monto { get; set; }
    }
}
