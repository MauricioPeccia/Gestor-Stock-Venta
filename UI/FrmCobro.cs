using Entidades;
using System.Globalization;

namespace UI
{
    public partial class FrmCobro : Form
    {
        private readonly decimal _subtotalOriginal;
        private readonly CultureInfo _ci = new CultureInfo("es-AR");

        public List<PagoVenta> Pagos { get; private set; } = new();
        public decimal Descuento { get; private set; }
        public decimal TotalFinal { get; private set; }
        public decimal Vuelto { get; private set; }

        public FrmCobro(decimal subtotal)
        {
            InitializeComponent();
            _subtotalOriginal = subtotal;

            cboMetodo.SelectedIndex = 0; // EFECTIVO por defecto
            CalcularTotales();

            txtDescuento.TextChanged += (s, e) => CalcularTotales();
            txtPagaCon.TextChanged += (s, e) => CalcularVuelto();
            txtTransferencia.TextChanged += (s, e) => CalcularVuelto();
            cboMetodo.SelectedIndexChanged += CboMetodo_SelectedIndexChanged;

            btnConfirmar.Click += BtnConfirmar_Click;
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            KeyDown += FrmCobro_KeyDown;

            txtPagaCon.Focus();
        }

        private void CboMetodo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var metodo = cboMetodo.SelectedItem?.ToString() ?? "EFECTIVO";

            if (metodo == "EFECTIVO")
            {
                lblPagaCon.Visible = true;
                txtPagaCon.Visible = true;
                lblPagaCon.Text = "Paga con (Efectivo) ($):";
                lblTransferencia.Visible = false;
                txtTransferencia.Visible = false;
                lblVueltoTitulo.Visible = true;
                lblVuelto.Visible = true;
                txtPagaCon.Focus();
            }
            else if (metodo == "TRANSFERENCIA")
            {
                lblPagaCon.Visible = false;
                txtPagaCon.Visible = false;
                lblTransferencia.Visible = false;
                txtTransferencia.Visible = false;
                lblVueltoTitulo.Visible = false;
                lblVuelto.Visible = false;
                btnConfirmar.Focus();
            }
            else // MIXTO
            {
                lblPagaCon.Visible = true;
                txtPagaCon.Visible = true;
                lblPagaCon.Text = "Monto Efectivo ($):";
                lblTransferencia.Visible = true;
                txtTransferencia.Visible = true;
                lblVueltoTitulo.Visible = true;
                lblVuelto.Visible = true;
                txtPagaCon.Focus();
            }

            CalcularVuelto();
        }

        private void CalcularTotales()
        {
            if (!decimal.TryParse(txtDescuento.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var desc) || desc < 0)
            {
                desc = 0;
            }

            if (desc > _subtotalOriginal)
            {
                desc = _subtotalOriginal;
            }

            Descuento = desc;
            TotalFinal = _subtotalOriginal - Descuento;
            lblTotal.Text = TotalFinal.ToString("C2", _ci);

            CalcularVuelto();
        }

        private void CalcularVuelto()
        {
            var metodo = cboMetodo.SelectedItem?.ToString() ?? "EFECTIVO";

            decimal pagaCon = 0;
            if (decimal.TryParse(txtPagaCon.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var valPaga))
            {
                pagaCon = valPaga;
            }

            if (metodo == "EFECTIVO")
            {
                Vuelto = pagaCon >= TotalFinal ? pagaCon - TotalFinal : 0;
                lblVuelto.Text = Vuelto.ToString("C2", _ci);
            }
            else if (metodo == "TRANSFERENCIA")
            {
                Vuelto = 0;
                lblVuelto.Text = "$ 0,00";
            }
            else // MIXTO
            {
                decimal montoTransf = 0;
                if (decimal.TryParse(txtTransferencia.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var valTransf))
                {
                    montoTransf = valTransf;
                }

                var pagadoTotal = pagaCon + montoTransf;
                Vuelto = pagadoTotal >= TotalFinal ? pagadoTotal - TotalFinal : 0;
                lblVuelto.Text = Vuelto.ToString("C2", _ci);
            }
        }

        private void BtnConfirmar_Click(object? sender, EventArgs e)
        {
            var metodo = cboMetodo.SelectedItem?.ToString() ?? "EFECTIVO";
            Pagos.Clear();

            if (metodo == "EFECTIVO")
            {
                decimal pagaCon = 0;
                if (!string.IsNullOrWhiteSpace(txtPagaCon.Text) &&
                    decimal.TryParse(txtPagaCon.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var pc))
                {
                    pagaCon = pc;
                }
                else
                {
                    // Si no ingresó monto, asumimos pago exacto
                    pagaCon = TotalFinal;
                }

                if (pagaCon < TotalFinal)
                {
                    MessageBox.Show($"El monto ingresado (${pagaCon:N2}) es menor al total a cobrar (${TotalFinal:N2}).",
                        "Monto Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPagaCon.Focus();
                    return;
                }

                Vuelto = pagaCon - TotalFinal;
                Pagos.Add(new PagoVenta { Metodo = "EFECTIVO", Monto = TotalFinal });
            }
            else if (metodo == "TRANSFERENCIA")
            {
                Vuelto = 0;
                Pagos.Add(new PagoVenta { Metodo = "TRANSFERENCIA", Monto = TotalFinal });
            }
            else // MIXTO
            {
                decimal ef = 0;
                decimal tr = 0;

                if (!decimal.TryParse(txtPagaCon.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out ef) || ef < 0)
                {
                    MessageBox.Show("Ingrese un monto en efectivo válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPagaCon.Focus();
                    return;
                }

                if (!decimal.TryParse(txtTransferencia.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out tr) || tr < 0)
                {
                    MessageBox.Show("Ingrese un monto en transferencia válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTransferencia.Focus();
                    return;
                }

                if (ef + tr < TotalFinal)
                {
                    MessageBox.Show($"La suma de Efectivo + Transferencia (${(ef + tr):N2}) es menor al total (${TotalFinal:N2}).",
                        "Monto Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Si pagó de más en efectivo, se da vuelto sobre el efectivo
                var efectivoReal = TotalFinal - tr;
                Vuelto = ef > efectivoReal ? ef - efectivoReal : 0;

                if (efectivoReal > 0)
                {
                    Pagos.Add(new PagoVenta { Metodo = "EFECTIVO", Monto = efectivoReal });
                }
                if (tr > 0)
                {
                    Pagos.Add(new PagoVenta { Metodo = "TRANSFERENCIA", Monto = tr });
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void FrmCobro_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (e.KeyCode == Keys.Enter && !btnCancelar.Focused)
            {
                e.SuppressKeyPress = true;
                BtnConfirmar_Click(sender, EventArgs.Empty);
            }
        }
    }
}

