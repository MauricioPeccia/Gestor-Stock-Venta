using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Formulario modal programático para ingreso bidireccional en tiempo real de Peso (Kg) o Monto ($)
    /// en productos fraccionados, 100% operado por teclado y consumiendo RecursosUI.cs.
    /// </summary>
    public sealed class FormCantidadFraccionada : Form
    {
        public decimal CantidadKilos { get; private set; }
        public decimal MontoCalculado { get; private set; }

        private readonly string _nombreProducto;
        private readonly decimal _precioPorKilo;
        private bool _actualizandoInternamente;

        private Panel _pnlHeader = null!;
        private Label _lblProductoPrecio = null!;
        private Label _lblSubtituloAyuda = null!;
        private Label _lblSeparadorTop = null!;

        private Label _lblPeso = null!;
        private TextBox txtPeso = null!;
        private Label _lblAyudaPeso = null!;
        private Label _lblMonto = null!;
        private TextBox txtMonto = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorBottom = null!;
        private Button _btnConfirmar = null!;
        private Button _btnCancelar = null!;

        public FormCantidadFraccionada(string nombreProducto, decimal precioPorKilo)
        {
            _nombreProducto = nombreProducto;
            _precioPorKilo = precioPorKilo;

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Cantidad Fraccionada (Kg / Importe)";
                this.ClientSize = new Size(460, 345);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;
                this.DoubleBuffered = true;
                this.BackColor = RecursosUI.ColorSuperficie;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblProductoPrecio = new Label();
                _lblSubtituloAyuda = new Label();
                _lblSeparadorTop = new Label();

                _lblPeso = new Label();
                txtPeso = new TextBox();
                _lblAyudaPeso = new Label();
                _lblMonto = new Label();
                txtMonto = new TextBox();

                _pnlFooter = new Panel();
                _lblSeparadorBottom = new Label();
                _btnConfirmar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades y Estilos (RecursosUI)
                // --- Encabezado con Nombre del Producto y Precio por Kg ---
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Size = new Size(this.ClientSize.Width, 76);
                _pnlHeader.BackColor = RecursosUI.ColorSuperficieSuave;

                _lblProductoPrecio.Text = $"{_nombreProducto}  —  $ {_precioPorKilo:N2} / Kg";
                _lblProductoPrecio.Font = RecursosUI.FuenteEncabezado;
                _lblProductoPrecio.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblProductoPrecio.AutoSize = true;
                _lblProductoPrecio.Location = new Point(22, 14);

                _lblSubtituloAyuda.Text = "Escriba en Peso (Kg) o en Monto ($). Use [TAB] o flechas para alternar.";
                _lblSubtituloAyuda.Font = RecursosUI.FuentePequena;
                _lblSubtituloAyuda.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtituloAyuda.AutoSize = true;
                _lblSubtituloAyuda.Location = new Point(22, 46);

                _lblSeparadorTop.Dock = DockStyle.Bottom;
                _lblSeparadorTop.Height = 1;
                _lblSeparadorTop.BackColor = RecursosUI.ColorSeparador;

                // --- Campo 1: Peso (Kg / Gramos) + Etiqueta de Ayuda ---
                _lblPeso.Text = "Peso en Kilos:";
                _lblPeso.Font = RecursosUI.FuenteNegrita;
                _lblPeso.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblPeso.AutoSize = true;
                _lblPeso.Location = new Point(24, 92);

                txtPeso.Location = new Point(24, 114);
                txtPeso.Size = new Size(412, 34);
                txtPeso.Font = RecursosUI.FuenteInputGrande;
                txtPeso.ForeColor = RecursosUI.ColorTextoPrincipal;
                txtPeso.BackColor = RecursosUI.ColorSuperficie;
                txtPeso.BorderStyle = BorderStyle.FixedSingle;
                txtPeso.TabIndex = 0;

                _lblAyudaPeso.Text = "(Ej: 0.250 para 250 gramos)";
                _lblAyudaPeso.Font = RecursosUI.FuentePequena;
                _lblAyudaPeso.ForeColor = RecursosUI.ColorTextoMuted;
                _lblAyudaPeso.AutoSize = true;
                _lblAyudaPeso.Location = new Point(24, 151);

                // --- Campo 2: Monto en Dinero ($) ---
                _lblMonto.Text = "Monto en Dinero ($):";
                _lblMonto.Font = RecursosUI.FuenteNegrita;
                _lblMonto.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblMonto.AutoSize = true;
                _lblMonto.Location = new Point(24, 182);

                txtMonto.Location = new Point(24, 206);
                txtMonto.Size = new Size(412, 34);
                txtMonto.Font = RecursosUI.FuenteInputGrande;
                txtMonto.ForeColor = RecursosUI.ColorTextoTotalCaja;
                txtMonto.BackColor = RecursosUI.ColorSuperficie;
                txtMonto.BorderStyle = BorderStyle.FixedSingle;
                txtMonto.TabIndex = 1;

                // --- Pie de Ventana (Botones) ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Size = new Size(this.ClientSize.Width, 68);
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorBottom.Dock = DockStyle.Top;
                _lblSeparadorBottom.Height = 1;
                _lblSeparadorBottom.BackColor = RecursosUI.ColorSeparador;

                _btnConfirmar.Text = "Confirmar [ENTER]";
                _btnConfirmar.BackColor = RecursosUI.ColorExito;
                _btnConfirmar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnConfirmar.FlatStyle = FlatStyle.Flat;
                _btnConfirmar.FlatAppearance.BorderSize = 0;
                _btnConfirmar.Font = RecursosUI.FuenteNegrita;
                _btnConfirmar.Location = new Point(24, 14);
                _btnConfirmar.Size = new Size(210, 40);
                _btnConfirmar.Cursor = Cursors.Hand;
                _btnConfirmar.TabIndex = 2;

                _btnCancelar.Text = "Cancelar [ESC]";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Location = new Point(250, 14);
                _btnCancelar.Size = new Size(186, 40);
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 3;

                // 5. Suscripción a Eventos
                txtPeso.TextChanged += TxtPeso_TextChanged;
                txtMonto.TextChanged += TxtMonto_TextChanged;

                txtPeso.KeyDown += TextBoxes_KeyDown;
                txtMonto.KeyDown += TextBoxes_KeyDown;

                _btnConfirmar.Click += (s, e) => ConfirmarSeleccion();
                _btnCancelar.Click += (s, e) => CancelarYCerrar();

                this.Load += (s, e) =>
                {
                    txtPeso.Text = "0.";
                    this.ActiveControl = txtPeso;
                    txtPeso.Focus();
                    txtPeso.SelectionStart = txtPeso.Text.Length;
                    txtPeso.SelectionLength = 0;
                };

                // 7. Agregado de Controles al Contenedor
                _pnlHeader.Controls.Add(_lblProductoPrecio);
                _pnlHeader.Controls.Add(_lblSubtituloAyuda);
                _pnlHeader.Controls.Add(_lblSeparadorTop);

                _pnlFooter.Controls.Add(_lblSeparadorBottom);
                _pnlFooter.Controls.Add(_btnConfirmar);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_lblPeso);
                this.Controls.Add(txtPeso);
                this.Controls.Add(_lblAyudaPeso);
                this.Controls.Add(_lblMonto);
                this.Controls.Add(txtMonto);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        /// <summary>
        /// Si el usuario escribe en txtPeso (ej. 0.250), calcula automáticamente el importe en txtMonto (Peso * Precio Unitario).
        /// </summary>
        private void TxtPeso_TextChanged(object? sender, EventArgs e)
        {
            if (_actualizandoInternamente) return;

            _actualizandoInternamente = true;
            try
            {
                if (IntentarParsearDecimal(txtPeso.Text, out decimal peso) && peso > 0m)
                {
                    CantidadKilos = Math.Round(peso, 3);
                    MontoCalculado = Math.Round(CantidadKilos * _precioPorKilo, 2);
                    txtMonto.Text = MontoCalculado.ToString("0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    CantidadKilos = 0m;
                    MontoCalculado = 0m;
                    txtMonto.Text = string.Empty;
                }
            }
            finally
            {
                _actualizandoInternamente = false;
            }
        }

        /// <summary>
        /// Si el usuario escribe en txtMonto (ej. 1500), calcula automáticamente el peso en txtPeso (Monto / Precio Unitario).
        /// </summary>
        private void TxtMonto_TextChanged(object? sender, EventArgs e)
        {
            if (_actualizandoInternamente) return;

            _actualizandoInternamente = true;
            try
            {
                if (IntentarParsearDecimal(txtMonto.Text, out decimal monto) && monto > 0m && _precioPorKilo > 0m)
                {
                    MontoCalculado = Math.Round(monto, 2);
                    CantidadKilos = Math.Round(MontoCalculado / _precioPorKilo, 3);
                    txtPeso.Text = CantidadKilos.ToString("0.000", CultureInfo.InvariantCulture);
                }
                else
                {
                    CantidadKilos = 0m;
                    MontoCalculado = 0m;
                    txtPeso.Text = string.Empty;
                }
            }
            finally
            {
                _actualizandoInternamente = false;
            }
        }

        private static bool IntentarParsearDecimal(string? entrada, out decimal resultado)
        {
            if (string.IsNullOrWhiteSpace(entrada))
            {
                resultado = 0m;
                return false;
            }

            string normalizado = entrada.Replace(",", ".").Trim();
            return decimal.TryParse(normalizado, NumberStyles.Number, CultureInfo.InvariantCulture, out resultado);
        }

        private void TextBoxes_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ConfirmarSeleccion();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                CancelarYCerrar();
            }
            else if (e.KeyCode == Keys.Down && sender == txtPeso)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                txtMonto.Focus();
                txtMonto.SelectAll();
            }
            else if (e.KeyCode == Keys.Up && sender == txtMonto)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                txtPeso.Focus();
                txtPeso.SelectionStart = txtPeso.Text.Length;
                txtPeso.SelectionLength = 0;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                CancelarYCerrar();
                return true;
            }
            if (keyData == Keys.Enter)
            {
                ConfirmarSeleccion();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ConfirmarSeleccion()
        {
            if (CantidadKilos <= 0m)
            {
                MessageBox.Show(
                    this,
                    "Ingrese un peso en Kilos o un monto en dinero mayor a cero.",
                    "Valor inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtPeso.Focus();
                txtPeso.SelectionStart = txtPeso.Text.Length;
                txtPeso.SelectionLength = 0;
                return;
            }

            if (CantidadKilos > 10m)
            {
                DialogResult respuesta = MessageBox.Show(
                    this,
                    $"Atención: Está a punto de cargar {CantidadKilos:0.###} KILOS de este producto. ¿Es correcto?",
                    "Confirmar Cantidad Elevada",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                );

                if (respuesta == DialogResult.No)
                {
                    txtPeso.Focus();
                    txtPeso.SelectAll();
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelarYCerrar()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
