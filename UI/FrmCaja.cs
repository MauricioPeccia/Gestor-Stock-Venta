using Entidades;
using Servicios;
using System.Globalization;

namespace UI
{
    public partial class FrmCaja : Form
    {
        private readonly CajaServicio _cajaServicio = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");
        private JornadaCaja? _jornadaActual;

        public FrmCaja()
        {
            InitializeComponent();

            cboTipoMov.SelectedIndex = 0; // EGRESO

            btnAbrirCaja.Click += async (s, e) => await AbrirCajaAsync();
            btnRegistrarMov.Click += async (s, e) => await RegistrarMovimientoAsync();
            btnCerrarCaja.Click += async (s, e) => await CerrarCajaAsync();
            btnReabrirCaja.Click += async (s, e) => await ReabrirCajaAsync();
            btnCerrarVentana.Click += (s, e) => Close();

            Load += async (s, e) => await CargarEstadoAsync();
        }

        private async Task CargarEstadoAsync()
        {
            try
            {
                _jornadaActual = await _cajaServicio.ObtenerAbiertaAsync();

                if (_jornadaActual == null)
                {
                    // Intentar buscar si hoy hubo una caja que ya fue cerrada
                    var cerradaHoy = await _cajaServicio.ObtenerPorFechaAsync(DateTime.Today);
                    if (cerradaHoy != null && cerradaHoy.Estado == "CERRADA")
                    {
                        MostrarEstadoCerrada(cerradaHoy);
                        await MostrarResumenAsync(cerradaHoy.Id, cerradaHoy);
                        return;
                    }

                    MostrarEstadoSinCaja();
                }
                else
                {
                    MostrarEstadoAbierta(_jornadaActual);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar estado de caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarEstadoSinCaja()
        {
            lblEstado.Text = "● ESTADO: CAJA CERRADA (Sin jornada activa)";
            lblEstado.ForeColor = Color.DarkRed;
            lblDetalleApertura.Text = "No hay ninguna caja abierta actualmente. Ingrese el monto inicial y presione Abrir Caja.";

            grpApertura.Enabled = true;
            txtMontoApertura.Focus();
            txtMontoApertura.SelectAll();

            grpMovimientos.Enabled = false;
            grpCierre.Enabled = false;
            btnReabrirCaja.Visible = false;

            LimpiarResumen();
        }

        private void MostrarEstadoAbierta(JornadaCaja j)
        {
            lblEstado.Text = $"● ESTADO: CAJA ABIERTA (Jornada #{j.Id} - {j.Fecha:dd/MM/yyyy})";
            lblEstado.ForeColor = Color.DarkGreen;
            lblDetalleApertura.Text = $"Abierta el {j.AperturaEn:dd/MM/yyyy HH:mm} con un monto inicial de {j.MontoApertura.ToString("C2", _ci)}";

            grpApertura.Enabled = false;
            txtMontoApertura.Text = j.MontoApertura.ToString("N2", _ci);

            grpMovimientos.Enabled = true;
            grpCierre.Enabled = true;
            btnReabrirCaja.Visible = false;

            txtEfectivoContado.Clear();
            txtObsCierre.Clear();

            LimpiarResumen();
            lblResumenApertura.Text = $"Monto Apertura: {j.MontoApertura.ToString("C2", _ci)}";
            lblResumenVentas.Text = "(El arqueo detallado se mostrará al realizar el cierre ciego)";
        }

        private void MostrarEstadoCerrada(JornadaCaja j)
        {
            lblEstado.Text = $"● ESTADO: CAJA CERRADA (Jornada #{j.Id} del {j.Fecha:dd/MM/yyyy})";
            lblEstado.ForeColor = Color.DarkRed;
            lblDetalleApertura.Text = $"Cerrada el {j.CierreEn:dd/MM/yyyy HH:mm}. Observaciones: {j.Observaciones ?? "Ninguna"}";

            grpApertura.Enabled = false;
            grpMovimientos.Enabled = false;
            grpCierre.Enabled = false;
            btnReabrirCaja.Visible = true;
            btnReabrirCaja.Tag = j.Id;
        }

        private void LimpiarResumen()
        {
            lblResumenApertura.Text = "Monto Apertura: -";
            lblResumenVentas.Text = "Ventas: -";
            lblResumenIngresos.Text = "Ingresos: -";
            lblResumenEgresos.Text = "Egresos: -";
            lblResumenGanancia.Text = "Ganancia Bruta: -";
            lblResumenEfectivoEsperado.Text = "Efectivo Esperado: -";
            lblResumenEfectivoContado.Text = "Efectivo Contado: -";
            lblResumenDiferencia.Text = "Diferencia: -";
            lblResumenDiferencia.ForeColor = Color.Black;
        }

        private async Task AbrirCajaAsync()
        {
            if (!decimal.TryParse(txtMontoApertura.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var monto) || monto < 0)
            {
                MessageBox.Show("Ingrese un monto inicial de apertura válido (mayor o igual a cero).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMontoApertura.Focus();
                return;
            }

            try
            {
                var id = await _cajaServicio.AbrirAsync(monto);
                MessageBox.Show($"Jornada #{id} de caja abierta con éxito.", "Caja Abierta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarEstadoAsync();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RegistrarMovimientoAsync()
        {
            var tipo = cboTipoMov.SelectedItem?.ToString() ?? "EGRESO";
            var concepto = txtConcepto.Text.Trim();

            if (string.IsNullOrWhiteSpace(concepto))
            {
                MessageBox.Show("El concepto o motivo del movimiento es obligatorio.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConcepto.Focus();
                return;
            }

            if (!decimal.TryParse(txtMontoMov.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto válido mayor a cero.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMontoMov.Focus();
                return;
            }

            try
            {
                await _cajaServicio.RegistrarMovimientoAsync(tipo, concepto, monto);
                MessageBox.Show($"Movimiento de {tipo} registrado correctamente (${monto:N2}).",
                    "Movimiento Registrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtConcepto.Clear();
                txtMontoMov.Clear();
                txtConcepto.Focus();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar movimiento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CerrarCajaAsync()
        {
            if (_jornadaActual == null) return;

            if (!decimal.TryParse(txtEfectivoContado.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var contado) || contado < 0)
            {
                MessageBox.Show("Ingrese el efectivo total contado en el cajón físico.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEfectivoContado.Focus();
                return;
            }

            var confirm = MessageBox.Show(
                "¿Está seguro de que desea cerrar la caja?\n\nAl cerrar se guardará el arqueo de caja y finalizará la jornada.",
                "Confirmar Cierre de Caja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var resumen = await _cajaServicio.CerrarAsync(_jornadaActual.Id, contado, txtObsCierre.Text.Trim());

                MessageBox.Show("La caja ha sido cerrada correctamente. A continuación se detalla el arqueo final.",
                    "Caja Cerrada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var jornadaCerrada = await _cajaServicio.ObtenerPorFechaAsync(_jornadaActual.Fecha);
                if (jornadaCerrada != null)
                {
                    MostrarEstadoCerrada(jornadaCerrada);
                    ActualizarLabelsResumen(resumen, contado, contado - resumen.EfectivoEsperado);
                }
                else
                {
                    await CargarEstadoAsync();
                }
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task MostrarResumenAsync(long jornadaId, JornadaCaja j)
        {
            try
            {
                var resumen = await _cajaServicio.ObtenerResumenAsync(jornadaId);
                ActualizarLabelsResumen(resumen, j.EfectivoContado ?? 0, j.Diferencia ?? 0);
            }
            catch (Exception ex)
            {
                lblResumenVentas.Text = $"Error al cargar resumen: {ex.Message}";
            }
        }

        private void ActualizarLabelsResumen(ResumenJornada r, decimal contado, decimal diferencia)
        {
            lblResumenApertura.Text = $"Monto Apertura: {r.MontoApertura.ToString("C2", _ci)}";
            lblResumenVentas.Text = $"Ventas Ef: {r.VentasEfectivo.ToString("C2", _ci)} | Transf: {r.VentasTransferencia.ToString("C2", _ci)} | Total: {r.TotalVendido.ToString("C2", _ci)} ({r.CantidadVentas} ventas, {r.CantidadAnuladas} anuladas)";
            lblResumenIngresos.Text = $"Ingresos Manuales: {r.Ingresos.ToString("C2", _ci)}";
            lblResumenEgresos.Text = $"Egresos Manuales: {r.Egresos.ToString("C2", _ci)}";
            lblResumenGanancia.Text = $"Ganancia Bruta Estimada: {r.GananciaBruta.ToString("C2", _ci)}";
            lblResumenEfectivoEsperado.Text = $"Efectivo Esperado: {r.EfectivoEsperado.ToString("C2", _ci)}";
            lblResumenEfectivoContado.Text = $"Efectivo Contado: {contado.ToString("C2", _ci)}";

            if (diferencia == 0)
            {
                lblResumenDiferencia.Text = "Diferencia: $ 0,00 (Caja Exacta ✔)";
                lblResumenDiferencia.ForeColor = Color.DarkGreen;
            }
            else if (diferencia > 0)
            {
                lblResumenDiferencia.Text = $"Diferencia: +{diferencia.ToString("C2", _ci)} (SOBRANTE)";
                lblResumenDiferencia.ForeColor = Color.SteelBlue;
            }
            else
            {
                lblResumenDiferencia.Text = $"Diferencia: {diferencia.ToString("C2", _ci)} (FALTANTE ⚠️)";
                lblResumenDiferencia.ForeColor = Color.Red;
            }
        }

        private async Task ReabrirCajaAsync()
        {
            if (btnReabrirCaja.Tag is not long jornadaId) return;

            var conf = MessageBox.Show(
                "¿Desea reabrir esta jornada de caja para continuar registrando operaciones?",
                "Reabrir Caja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (conf != DialogResult.Yes) return;

            try
            {
                await _cajaServicio.ReabrirAsync(jornadaId);
                MessageBox.Show("La caja ha sido reabierta con éxito.", "Caja Reabierta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarEstadoAsync();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reabrir la caja: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

