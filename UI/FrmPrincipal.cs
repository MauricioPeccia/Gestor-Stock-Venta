using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            var caja = await new CajaServicio().ObtenerAbiertaAsync();

            if (caja == null)
            {
                lblEstadoCaja.Text = "CAJA CERRADA";
                btnVender.Enabled = false;     // o dejarlo habilitado y ofrecer abrir caja al intentar vender
            }
            else if (caja.Fecha.Date < DateTime.Today)
            {
                MessageBox.Show($"La caja del {caja.Fecha:dd/MM/yyyy} quedó sin cerrar. " +
                                "Hay que cerrarla antes de seguir.", "Caja pendiente");
                new FrmCaja().ShowDialog();    // abre la pantalla de cierre
            }
            else
            {
                lblEstadoCaja.Text = $"CAJA ABIERTA - {caja.Fecha:dd/MM/yyyy}";
            }
        }
    }
}
