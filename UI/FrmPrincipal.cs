using Servicios;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmPrincipal : Form
    {
        private readonly CajaServicio _cajaServicio = new();

        public FrmPrincipal()
        {
            InitializeComponent();

            btnVender.Click += BtnVender_Click;
            btnCaja.Click += BtnCaja_Click;
            btnProductos.Click += BtnProductos_Click;
            btnAjusteStock.Click += BtnAjusteStock_Click;
            btnHistorial.Click += BtnHistorial_Click;
            btnCompras.Click += BtnCompras_Click;
            btnCategorias.Click += BtnCategorias_Click;
            btnProveedores.Click += BtnProveedores_Click;
            btnReportes.Click += BtnReportes_Click;

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F12)
                {
                    e.SuppressKeyPress = true;
                    BtnVender_Click(s, EventArgs.Empty);
                }
            };
        }

        private async void FrmPrincipal_Load(object sender, EventArgs e)
        {
            await ActualizarEstadoCajaAsync();
        }

        public async Task ActualizarEstadoCajaAsync()
        {
            try
            {
                var caja = await _cajaServicio.ObtenerAbiertaAsync();

                if (caja == null)
                {
                    lblEstadoCaja.Text = "● ESTADO: CAJA CERRADA — Debe abrir la jornada de caja para registrar ventas.";
                    lblEstadoCaja.ForeColor = Color.DarkRed;
                    pnlEstadoCaja.BackColor = Color.MistyRose;
                }
                else if (caja.Fecha.Date < DateTime.Today)
                {
                    lblEstadoCaja.Text = $"⚠️ CAJA PENDIENTE DE CIERRE DEL {caja.Fecha:dd/MM/yyyy} — Ciérrela para iniciar un nuevo día.";
                    lblEstadoCaja.ForeColor = Color.DarkOrange;
                    pnlEstadoCaja.BackColor = Color.FloralWhite;

                    var resp = MessageBox.Show(
                        $"La caja del {caja.Fecha:dd/MM/yyyy} quedó sin cerrar.\n¿Desea abrir la pantalla de Caja para cerrarla ahora?",
                        "Caja pendiente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (resp == DialogResult.Yes)
                    {
                        using var frmCaja = new FrmCaja();
                        frmCaja.ShowDialog(this);
                        await ActualizarEstadoCajaAsync();
                    }
                }
                else
                {
                    lblEstadoCaja.Text = $"● ESTADO: CAJA ABIERTA — Jornada #{caja.Id} ({caja.Fecha:dd/MM/yyyy}) | Monto Inicial: ${caja.MontoApertura:N2}";
                    lblEstadoCaja.ForeColor = Color.DarkGreen;
                    pnlEstadoCaja.BackColor = Color.Honeydew;
                }
            }
            catch (Exception ex)
            {
                lblEstadoCaja.Text = $"Error de conexión con la base de datos: {ex.Message}";
                lblEstadoCaja.ForeColor = Color.Red;
                pnlEstadoCaja.BackColor = Color.MistyRose;
            }
        }

        private async void BtnVender_Click(object? sender, EventArgs e)
        {
            var caja = await _cajaServicio.ObtenerAbiertaAsync();
            if (caja == null)
            {
                var abrir = MessageBox.Show(
                    "No hay una caja abierta actualmente.\n\n¿Desea abrir la caja ahora para comenzar a vender?",
                    "Caja Requerida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (abrir == DialogResult.Yes)
                {
                    using var frmCaja = new FrmCaja();
                    frmCaja.ShowDialog(this);
                    await ActualizarEstadoCajaAsync();
                }
                return;
            }

            using var frmVenta = new FrmVenta();
            frmVenta.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnCaja_Click(object? sender, EventArgs e)
        {
            using var frmCaja = new FrmCaja();
            frmCaja.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnProductos_Click(object? sender, EventArgs e)
        {
            using var frmProductos = new FrmProductos();
            frmProductos.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnAjusteStock_Click(object? sender, EventArgs e)
        {
            using var frmAjuste = new FrmAjusteStock();
            frmAjuste.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnHistorial_Click(object? sender, EventArgs e)
        {
            using var frmHistorial = new FrmHistorialVenta();
            frmHistorial.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnCompras_Click(object? sender, EventArgs e)
        {
            using var frmCompras = new FrmCompras();
            frmCompras.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnCategorias_Click(object? sender, EventArgs e)
        {
            using var frmCategorias = new FrmCategorias();
            frmCategorias.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnProveedores_Click(object? sender, EventArgs e)
        {
            using var frmProveedores = new FrmProovedores();
            frmProveedores.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }

        private async void BtnReportes_Click(object? sender, EventArgs e)
        {
            using var frmReportes = new FrmReportes();
            frmReportes.ShowDialog(this);
            await ActualizarEstadoCajaAsync();
        }
    }
}

