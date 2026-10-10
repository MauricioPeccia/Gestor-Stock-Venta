namespace Kiosco.UI;

public partial class FrmCobro : Form
{
    public decimal Total { get; }
    public decimal EfectivoRecibido => numEfectivo.Value;
    public decimal Transferencia => numTransferencia.Value;
    public decimal AplicadoEfectivo => Math.Min(EfectivoRecibido, Math.Max(0, Total - Transferencia));
    public decimal Vuelto => EfectivoRecibido - AplicadoEfectivo;

    public FrmCobro(decimal total)
    {
        InitializeComponent();
        Total = total;
        lblTotalValor.Text = total.ToString("C2");
        numEfectivo.Value = total;
        numTransferencia.Value = 0;
        ActualizarCalculos();
    }

    private void Calculos_ValueChanged(object sender, EventArgs e)
    {
        ActualizarCalculos();
    }

    private void ActualizarCalculos()
    {
        var vuelto = Vuelto;
        lblVueltoValor.Text = vuelto.ToString("C2");

        if (AplicadoEfectivo + Transferencia < Total)
        {
            var restante = Total - (AplicadoEfectivo + Transferencia);
            lblVueltoTitulo.Text = $"FALTA COBRAR: {restante:C2}";
            lblVueltoTitulo.ForeColor = Color.DarkRed;
            lblVueltoValor.ForeColor = Color.DarkRed;
        }
        else
        {
            lblVueltoTitulo.Text = "VUELTO";
            lblVueltoTitulo.ForeColor = Color.FromArgb(21, 87, 36);
            lblVueltoValor.ForeColor = Color.FromArgb(21, 87, 36);
        }
    }

    private void num_Enter(object sender, EventArgs e)
    {
        if (sender is NumericUpDown nud)
        {
            nud.Select(0, nud.Text.Length);
        }
    }

    private void btnConfirmar_Click(object sender, EventArgs e)
    {
        if (AplicadoEfectivo + Transferencia < Total)
        {
            var falta = Total - (AplicadoEfectivo + Transferencia);
            MessageBox.Show($"El monto ingresado es insuficiente. Falta abonar {falta:C2}.", "Monto Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            numEfectivo.Focus();
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void FrmCobro_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5 || (e.KeyCode == Keys.Enter && !btnCancelar.Focused))
        {
            e.SuppressKeyPress = true;
            btnConfirmar.PerformClick();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            btnCancelar.PerformClick();
        }
    }
}

