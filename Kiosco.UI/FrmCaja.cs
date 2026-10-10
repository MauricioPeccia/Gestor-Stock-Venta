using Kiosco.Entidades;
using Kiosco.Servicios;

namespace Kiosco.UI;

public partial class FrmCaja : Form
{
    private readonly CajaServicio _cajaServicio = new();
    private JornadaCaja? _jornadaAbierta;

    public FrmCaja()
    {
        InitializeComponent();
    }

    private async void FrmCaja_Load(object sender, EventArgs e)
    {
        await CargarEstadoCajaAsync();
    }

    private async Task CargarEstadoCajaAsync()
    {
        try
        {
            _jornadaAbierta = await _cajaServicio.ObtenerAbiertaAsync();
            if (_jornadaAbierta == null)
            {
                lblInfoCaja.Text = "No hay ninguna caja abierta actualmente.";
                lblInfoCaja.ForeColor = Color.DarkRed;
                btnCerrarCaja.Enabled = false;
                numEfectivoContado.Enabled = false;
                txtObservaciones.Enabled = false;
            }
            else
            {
                lblInfoCaja.Text = $"Caja abierta correspondiente al día {_jornadaAbierta.Fecha:dd/MM/yyyy}.\nApertura: {_jornadaAbierta.AperturaEn:HH:mm} hs. - Fondo inicial: {_jornadaAbierta.MontoApertura:C2}";
                lblInfoCaja.ForeColor = Color.Black;
                btnCerrarCaja.Enabled = true;
                numEfectivoContado.Enabled = true;
                txtObservaciones.Enabled = true;
                numEfectivoContado.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar información de caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnCerrarCaja_Click(object sender, EventArgs e)
    {
        if (_jornadaAbierta == null) return;

        var efectivoContado = numEfectivoContado.Value;
        var observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();

        var confirmar = MessageBox.Show(
            $"¿Confirma el cierre de caja con un efectivo contado de {efectivoContado:C2}?",
            "Confirmar Cierre",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmar != DialogResult.Yes) return;

        try
        {
            btnCerrarCaja.Enabled = false;
            var resumen = await _cajaServicio.CerrarAsync(_jornadaAbierta.Id, efectivoContado, observaciones);

            var diferencia = efectivoContado - resumen.EfectivoEsperado;
            var mensajeDiferencia = diferencia == 0
                ? "El arqueo coincide exactamente con lo esperado."
                : (diferencia > 0 ? $"Sobrante de caja: {diferencia:C2}" : $"Faltante de caja: {Math.Abs(diferencia):C2}");

            MessageBox.Show(
                $"Caja cerrada correctamente.\n\n" +
                $"Efectivo esperado: {resumen.EfectivoEsperado:C2}\n" +
                $"Efectivo contado: {efectivoContado:C2}\n" +
                $"{mensajeDiferencia}\n\n" +
                $"Total ventas: {resumen.TotalVendido:C2} ({resumen.CantidadVentas} ventas)\n" +
                $"Ganancia bruta: {resumen.GananciaBruta:C2}",
                "Resumen de Cierre",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnCerrarCaja.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error inesperado al cerrar caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnCerrarCaja.Enabled = true;
        }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}

