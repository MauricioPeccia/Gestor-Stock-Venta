using System;
using System.Drawing;
using System.Windows.Forms;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Modal de confirmación de cobro (F12).
    /// Permite elegir Medio de Pago ([1] Efectivo / [2] Transferencia) y emisión opcional de ticket.
    /// </summary>
    public sealed class FormCobrarModal : Form
    {
        public string MedioPagoSeleccionado { get; private set; } = "EFECTIVO";
        public bool DeseaImprimirTicket { get; private set; }
        public decimal TotalVenta { get; }

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Label _lblSeparadorTop = null!;

        private Panel _pnlTotalCard = null!;
        private Label _lblTotalTexto = null!;
        private Label _lblTotalMonto = null!;

        private Label _lblMetodo = null!;
        private Button _btnEfectivo = null!;
        private Button _btnTransferencia = null!;
        private CheckBox _chkImprimirTicket = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorBottom = null!;
        private Button _btnConfirmar = null!;
        private Button _btnCancelar = null!;

        public FormCobrarModal(decimal totalVenta)
        {
            TotalVenta = totalVenta;
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Cobro de Venta (F12)";
                this.ClientSize = new Size(465, 430);
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackColor = RecursosUI.ColorSuperficie;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();
                _lblSeparadorTop = new Label();

                _pnlTotalCard = new Panel();
                _lblTotalTexto = new Label();
                _lblTotalMonto = new Label();

                _lblMetodo = new Label();
                _btnEfectivo = new Button();
                _btnTransferencia = new Button();
                _chkImprimirTicket = new CheckBox();

                _pnlFooter = new Panel();
                _lblSeparadorBottom = new Label();
                _btnConfirmar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 76;
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;

                _lblTitulo.Text = "Cobrar Venta";
                _lblTitulo.Font = RecursosUI.FuenteTituloApp;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(24, 14);

                _lblSubtitulo.Text = "Seleccione medio de pago:  [1] Efectivo   |   [2] Transferencia";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(26, 46);

                _lblSeparadorTop.Height = 1;
                _lblSeparadorTop.Dock = DockStyle.Bottom;
                _lblSeparadorTop.BackColor = RecursosUI.ColorSeparador;

                // Tarjeta de Monto Total
                _pnlTotalCard.Location = new Point(26, 94);
                _pnlTotalCard.Size = new Size(412, 82);
                _pnlTotalCard.BackColor = RecursosUI.ColorFondoVentana;

                _lblTotalTexto.Text = "MONTO TOTAL A COBRAR:";
                _lblTotalTexto.Font = RecursosUI.FuentePequenaNegrita;
                _lblTotalTexto.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblTotalTexto.AutoSize = true;
                _lblTotalTexto.Location = new Point(16, 12);

                _lblTotalMonto.Text = $"$ {TotalVenta:N2}";
                _lblTotalMonto.Font = RecursosUI.FuenteDisplayTotal;
                _lblTotalMonto.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTotalMonto.AutoSize = true;
                _lblTotalMonto.Location = new Point(14, 32);

                _lblMetodo.Text = "Método de Pago";
                _lblMetodo.Font = RecursosUI.FuenteNegrita;
                _lblMetodo.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblMetodo.AutoSize = true;
                _lblMetodo.Location = new Point(26, 194);

                _btnEfectivo.Text = "[ 1 ]   EFECTIVO";
                _btnEfectivo.Size = new Size(198, 54);
                _btnEfectivo.Location = new Point(26, 220);
                _btnEfectivo.Font = RecursosUI.FuenteGrillaHeader;
                _btnEfectivo.FlatStyle = FlatStyle.Flat;
                _btnEfectivo.FlatAppearance.BorderSize = 0;
                _btnEfectivo.BackColor = RecursosUI.ColorPrimario;
                _btnEfectivo.ForeColor = RecursosUI.ColorTextoClaro;
                _btnEfectivo.Cursor = Cursors.Hand;
                _btnEfectivo.TabIndex = 0;

                _btnTransferencia.Text = "[ 2 ]   TRANSFERENCIA";
                _btnTransferencia.Size = new Size(198, 54);
                _btnTransferencia.Location = new Point(240, 220);
                _btnTransferencia.Font = RecursosUI.FuenteGrillaHeader;
                _btnTransferencia.FlatStyle = FlatStyle.Flat;
                _btnTransferencia.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnTransferencia.BackColor = RecursosUI.ColorSuperficieSuave;
                _btnTransferencia.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnTransferencia.Cursor = Cursors.Hand;
                _btnTransferencia.TabIndex = 1;

                _chkImprimirTicket.Text = "Generar comprobante / Ticket (Opcional)";
                _chkImprimirTicket.Font = RecursosUI.FuenteNegrita;
                _chkImprimirTicket.ForeColor = RecursosUI.ColorTextoPrincipal;
                _chkImprimirTicket.AutoSize = true;
                _chkImprimirTicket.Location = new Point(28, 296);
                _chkImprimirTicket.Checked = false;
                _chkImprimirTicket.TabIndex = 2;

                // Footer
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 74;
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorBottom.Height = 1;
                _lblSeparadorBottom.Dock = DockStyle.Top;
                _lblSeparadorBottom.BackColor = RecursosUI.ColorSeparador;

                _btnConfirmar.Text = "Confirmar Cobro (ENTER)";
                _btnConfirmar.BackColor = RecursosUI.ColorExito;
                _btnConfirmar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnConfirmar.FlatStyle = FlatStyle.Flat;
                _btnConfirmar.FlatAppearance.BorderSize = 0;
                _btnConfirmar.Size = new Size(225, 42);
                _btnConfirmar.Location = new Point(26, 16);
                _btnConfirmar.Font = RecursosUI.FuenteNegrita;
                _btnConfirmar.Cursor = Cursors.Hand;
                _btnConfirmar.TabIndex = 3;

                _btnCancelar.Text = "Cancelar (ESC)";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Size = new Size(172, 42);
                _btnCancelar.Location = new Point(266, 16);
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 4;

                // 5. Suscripción a Eventos
                _btnEfectivo.Click += (s, e) => SeleccionarMedio("EFECTIVO");
                _btnTransferencia.Click += (s, e) => SeleccionarMedio("TRANSFERENCIA");
                _btnConfirmar.Click += (s, e) => ConfirmarCobro();
                _btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                // 7. Agregado de Controles
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_lblSeparadorTop);

                _pnlTotalCard.Controls.Add(_lblTotalTexto);
                _pnlTotalCard.Controls.Add(_lblTotalMonto);

                _pnlFooter.Controls.Add(_lblSeparadorBottom);
                _pnlFooter.Controls.Add(_btnConfirmar);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_pnlTotalCard);
                this.Controls.Add(_lblMetodo);
                this.Controls.Add(_btnEfectivo);
                this.Controls.Add(_btnTransferencia);
                this.Controls.Add(_chkImprimirTicket);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);

                this.ActiveControl = _btnConfirmar;
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    SeleccionarMedio("EFECTIVO");
                    return true;

                case Keys.D2:
                case Keys.NumPad2:
                    SeleccionarMedio("TRANSFERENCIA");
                    return true;

                case Keys.Enter:
                    ConfirmarCobro();
                    return true;

                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SeleccionarMedio(string medio)
        {
            MedioPagoSeleccionado = medio;

            if (medio == "EFECTIVO")
            {
                _btnEfectivo.BackColor = RecursosUI.ColorPrimario;
                _btnEfectivo.ForeColor = RecursosUI.ColorTextoClaro;
                _btnEfectivo.FlatAppearance.BorderSize = 0;

                _btnTransferencia.BackColor = RecursosUI.ColorSuperficieSuave;
                _btnTransferencia.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnTransferencia.FlatAppearance.BorderSize = 1;
            }
            else
            {
                _btnTransferencia.BackColor = RecursosUI.ColorPrimario;
                _btnTransferencia.ForeColor = RecursosUI.ColorTextoClaro;
                _btnTransferencia.FlatAppearance.BorderSize = 0;

                _btnEfectivo.BackColor = RecursosUI.ColorSuperficieSuave;
                _btnEfectivo.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnEfectivo.FlatAppearance.BorderSize = 1;
            }
        }

        private void ConfirmarCobro()
        {
            DeseaImprimirTicket = _chkImprimirTicket.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
