using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Modal para cálculo rápido de productos fraccionados por Peso (Kg) o por Monto ($).
    /// 100% programático y sin asignaciones GDI+ directas.
    /// </summary>
    public sealed class FormFraccionadoModal : Form
    {
        public decimal CantidadCalculada { get; private set; } = 1m;

        private readonly string _nombreProducto;
        private readonly decimal _precioKilo;

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Label _lblSeparadorTop = null!;

        private RadioButton _rdoPorPeso = null!;
        private RadioButton _rdoPorMonto = null!;
        private TextBox _txtEntrada = null!;
        private Label _lblResultadoEquivalente = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorBottom = null!;
        private Button _btnAceptar = null!;
        private Button _btnCancelar = null!;

        public FormFraccionadoModal(string nombreProducto, decimal precioKilo)
        {
            _nombreProducto = nombreProducto;
            _precioKilo = precioKilo;

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Venta de Producto Fraccionado";
                this.ClientSize = new Size(440, 330);
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

                _rdoPorPeso = new RadioButton();
                _rdoPorMonto = new RadioButton();
                _txtEntrada = new TextBox();
                _lblResultadoEquivalente = new Label();

                _pnlFooter = new Panel();
                _lblSeparadorBottom = new Label();
                _btnAceptar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 72;
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;

                _lblTitulo.Text = _nombreProducto;
                _lblTitulo.Font = RecursosUI.FuenteEncabezado;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(20, 14);

                _lblSubtitulo.Text = $"Precio por Unidad / Kilo: $ {_precioKilo:N2}";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(20, 42);

                _lblSeparadorTop.Height = 1;
                _lblSeparadorTop.Dock = DockStyle.Bottom;
                _lblSeparadorTop.BackColor = RecursosUI.ColorSeparador;

                _rdoPorPeso.Text = "Ingresar Peso (en Kg, ej: 0.250)";
                _rdoPorPeso.Checked = true;
                _rdoPorPeso.Font = RecursosUI.FuenteNegrita;
                _rdoPorPeso.ForeColor = RecursosUI.ColorTextoPrincipal;
                _rdoPorPeso.Location = new Point(28, 86);
                _rdoPorPeso.AutoSize = true;
                _rdoPorPeso.TabIndex = 0;

                _rdoPorMonto.Text = "Ingresar Monto en Dinero ($)";
                _rdoPorMonto.Font = RecursosUI.FuenteNegrita;
                _rdoPorMonto.ForeColor = RecursosUI.ColorTextoPrincipal;
                _rdoPorMonto.Location = new Point(28, 116);
                _rdoPorMonto.AutoSize = true;
                _rdoPorMonto.TabIndex = 1;

                _txtEntrada.Location = new Point(28, 150);
                _txtEntrada.Size = new Size(380, 32);
                _txtEntrada.Font = RecursosUI.FuenteInputGrande;
                _txtEntrada.ForeColor = RecursosUI.ColorTextoPrincipal;
                _txtEntrada.BorderStyle = BorderStyle.FixedSingle;
                _txtEntrada.Text = "0.250";
                _txtEntrada.TabIndex = 2;

                _lblResultadoEquivalente.Text = "Total a cobrar: $ 0,00";
                _lblResultadoEquivalente.Font = RecursosUI.FuenteGrillaHeader;
                _lblResultadoEquivalente.ForeColor = RecursosUI.ColorInventario;
                _lblResultadoEquivalente.Location = new Point(28, 196);
                _lblResultadoEquivalente.AutoSize = true;

                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 68;
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorBottom.Height = 1;
                _lblSeparadorBottom.Dock = DockStyle.Top;
                _lblSeparadorBottom.BackColor = RecursosUI.ColorSeparador;

                _btnAceptar.Text = "Aceptar (ENTER)";
                _btnAceptar.BackColor = RecursosUI.ColorPrimario;
                _btnAceptar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnAceptar.FlatStyle = FlatStyle.Flat;
                _btnAceptar.FlatAppearance.BorderSize = 0;
                _btnAceptar.Size = new Size(190, 40);
                _btnAceptar.Location = new Point(28, 14);
                _btnAceptar.Font = RecursosUI.FuenteNegrita;
                _btnAceptar.Cursor = Cursors.Hand;
                _btnAceptar.TabIndex = 3;

                _btnCancelar.Text = "Cancelar (ESC)";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Size = new Size(170, 40);
                _btnCancelar.Location = new Point(238, 14);
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 4;

                // 5. Suscripción a Eventos
                _txtEntrada.TextChanged += (s, e) => RecalcularValores();
                _rdoPorPeso.CheckedChanged += (s, e) =>
                {
                    if (_rdoPorPeso.Checked) _txtEntrada.Text = "0.250";
                    RecalcularValores();
                    _txtEntrada.Focus();
                    _txtEntrada.SelectAll();
                };
                _rdoPorMonto.CheckedChanged += (s, e) =>
                {
                    if (_rdoPorMonto.Checked) _txtEntrada.Text = "1000";
                    RecalcularValores();
                    _txtEntrada.Focus();
                    _txtEntrada.SelectAll();
                };

                _btnAceptar.Click += (s, e) => ValidarYAceptar();
                _btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                this.Load += (s, e) =>
                {
                    RecalcularValores();
                    this.ActiveControl = _txtEntrada;
                    _txtEntrada.SelectAll();
                };

                // 7. Agregado de Controles
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_lblSeparadorTop);

                _pnlFooter.Controls.Add(_lblSeparadorBottom);
                _pnlFooter.Controls.Add(_btnAceptar);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_rdoPorPeso);
                this.Controls.Add(_rdoPorMonto);
                this.Controls.Add(_txtEntrada);
                this.Controls.Add(_lblResultadoEquivalente);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            if (keyData == Keys.Enter)
            {
                ValidarYAceptar();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RecalcularValores()
        {
            string limpio = _txtEntrada.Text.Replace(",", ".").Trim();
            if (decimal.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor) && valor > 0)
            {
                if (_rdoPorPeso.Checked)
                {
                    decimal subtotal = Math.Round(valor * _precioKilo, 2);
                    _lblResultadoEquivalente.Text = $"Total a cobrar: $ {subtotal:N2}";
                    _lblResultadoEquivalente.ForeColor = RecursosUI.ColorInventario;
                    CantidadCalculada = valor;
                }
                else
                {
                    if (_precioKilo > 0)
                    {
                        decimal kilos = Math.Round(valor / _precioKilo, 3);
                        _lblResultadoEquivalente.Text = $"Peso equivalente: {kilos:0.000} Kg";
                        _lblResultadoEquivalente.ForeColor = RecursosUI.ColorInventario;
                        CantidadCalculada = kilos;
                    }
                }
            }
            else
            {
                CantidadCalculada = 0m;
                _lblResultadoEquivalente.Text = "Ingrese un valor numérico válido mayor a 0.";
                _lblResultadoEquivalente.ForeColor = RecursosUI.ColorPeligro;
            }
        }

        private void ValidarYAceptar()
        {
            RecalcularValores();
            if (CantidadCalculada <= 0)
            {
                MessageBox.Show(this, "La cantidad o peso debe ser mayor a cero.", "Valor inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtEntrada.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
