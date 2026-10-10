using Kiosco.Entidades;
using Kiosco.Servicios;

namespace Kiosco.UI;

public partial class FrmPrincipal : Form
{
    private readonly CajaServicio _cajaServicio = new();
    private readonly ProductoServicio _productoServicio = new();

    public FrmPrincipal()
    {
        InitializeComponent();
    }

    private async void FrmPrincipal_Load(object sender, EventArgs e)
    {
        await VerificarYActualizarEstadoCajaAsync();
        await VerificarStockMinimoAsync();
    }

    private async Task VerificarYActualizarEstadoCajaAsync()
    {
        try
        {
            while (true)
            {
                var caja = await _cajaServicio.ObtenerAbiertaAsync();

                if (caja == null)
                {
                    lblEstadoCaja.Text = "CAJA CERRADA";
                    lblEstadoCaja.ForeColor = Color.Salmon;
                    btnVender.Enabled = false;
                    break;
                }
                else if (caja.Fecha.Date < DateTime.Today)
                {
                    MessageBox.Show(
                        $"La caja del {caja.Fecha:dd/MM/yyyy} quedó sin cerrar.\nHay que cerrarla antes de seguir.",
                        "Caja pendiente",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    using var frmCaja = new FrmCaja();
                    frmCaja.ShowDialog(this);

                    // Verificamos nuevamente si la caja de un día anterior sigue abierta
                    var cajaActual = await _cajaServicio.ObtenerAbiertaAsync();
                    if (cajaActual != null && cajaActual.Fecha.Date < DateTime.Today)
                    {
                        var reintentar = MessageBox.Show(
                            "La caja de la jornada anterior sigue sin cerrarse. No es posible continuar operando el sistema.\n\n¿Desea volver a intentar cerrarla?",
                            "Cierre de caja obligatorio",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Stop);

                        if (reintentar == DialogResult.Cancel)
                        {
                            Close();
                            return;
                        }
                        continue;
                    }
                }
                else
                {
                    lblEstadoCaja.Text = $"CAJA ABIERTA - {caja.Fecha:dd/MM/yyyy}";
                    lblEstadoCaja.ForeColor = Color.LightGreen;
                    btnVender.Enabled = true;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            lblEstadoCaja.Text = "ERROR DE CONEXIÓN";
            lblEstadoCaja.ForeColor = Color.Red;
            btnVender.Enabled = false;
            MessageBox.Show($"Error al verificar el estado de la caja: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task VerificarStockMinimoAsync()
    {
        try
        {
            var productosBajoMinimo = await _productoServicio.BajoStockMinimoAsync();
            if (productosBajoMinimo.Count > 0)
            {
                panelAlerta.Visible = true;
                lblAlertaStock.Text = $"⚠️ Alerta: Hay {productosBajoMinimo.Count} producto(s) con stock igual o inferior al mínimo configurado.";
            }
            else
            {
                panelAlerta.Visible = false;
            }
        }
        catch
        {
            // Evitar interrumpir el inicio si falla la consulta de alertas secundarias
            panelAlerta.Visible = false;
        }
    }

    private void btnVender_Click(object sender, EventArgs e)
    {
        using var frmVenta = new FrmVenta();
        frmVenta.ShowDialog(this);
        _ = VerificarStockMinimoAsync();
    }

    private async void btnCaja_Click(object sender, EventArgs e)
    {
        using var frm = new FrmCaja();
        frm.ShowDialog(this);
        await VerificarYActualizarEstadoCajaAsync();
    }

    private async void btnProductos_Click(object sender, EventArgs e)
    {
        using var frm = new FrmProductos();
        frm.ShowDialog(this);
        await VerificarStockMinimoAsync();
    }

    private void btnCompras_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Módulo de Compras en desarrollo.", "Compras", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnReportes_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Módulo de Reportes en desarrollo.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

