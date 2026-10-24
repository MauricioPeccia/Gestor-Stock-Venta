using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Modal de Ajuste Manual de Stock (F6).
    /// Corrige cantidades por mermas, roturas o conteo físico sin afectar el dinero de caja.
    /// 100% programático y consumiendo exclusivamente RecursosUI.
    /// </summary>
    public sealed class FormAjusteStockModal : Form
    {
        private readonly int _idProducto;
        private readonly string _nombreProducto;
        private readonly decimal _stockActual;
        private readonly bool _esFraccionado;
        private readonly ProductoNegocio _productoNegocio;

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Label _lblSeparadorTop = null!;

        private Panel _pnlDetalleProducto = null!;
        private Label _lblNombreProducto = null!;
        private Label _lblStockActualTitulo = null!;
        private Label _lblStockActualValor = null!;

        private Label _lblNuevoStockLabel = null!;
        private TextBox _txtNuevoStock = null!;
        private Label _lblDiferenciaCalculada = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorBottom = null!;
        private Button _btnConfirmar = null!;
        private Button _btnCancelar = null!;

        public FormAjusteStockModal(int idProducto, string nombreProducto, decimal stockActual, bool esFraccionado = false)
        {
            _idProducto = idProducto;
            _nombreProducto = nombreProducto;
            _stockActual = stockActual;
            _esFraccionado = esFraccionado;
            _productoNegocio = new ProductoNegocio();

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Ajuste Manual de Stock (F6)";
                this.ClientSize = new Size(490, 390);
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

                _pnlDetalleProducto = new Panel();
                _lblNombreProducto = new Label();
                _lblStockActualTitulo = new Label();
                _lblStockActualValor = new Label();

                _lblNuevoStockLabel = new Label();
                _txtNuevoStock = new TextBox();
                _lblDiferenciaCalculada = new Label();

                _pnlFooter = new Panel();
                _lblSeparadorBottom = new Label();
                _btnConfirmar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 76;
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;

                _lblTitulo.Text = "Ajuste de Stock (F6)";
                _lblTitulo.Font = RecursosUI.FuenteTituloApp;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(24, 14);

                _lblSubtitulo.Text = "Modificación directa por mermas, roturas o conteo físico (no afecta caja)";
                _lblSubtitulo.Font = RecursosUI.FuentePequena;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(26, 46);

                _lblSeparadorTop.Height = 1;
                _lblSeparadorTop.Dock = DockStyle.Bottom;
                _lblSeparadorTop.BackColor = RecursosUI.ColorSeparador;

                // Tarjeta resumen del producto
                _pnlDetalleProducto.Location = new Point(28, 94);
                _pnlDetalleProducto.Size = new Size(434, 76);
                _pnlDetalleProducto.BackColor = RecursosUI.ColorFondoVentana;

                _lblNombreProducto.Text = _nombreProducto;
                _lblNombreProducto.Font = RecursosUI.FuenteEncabezado;
                _lblNombreProducto.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblNombreProducto.Location = new Point(16, 12);
                _lblNombreProducto.Size = new Size(402, 26);
                _lblNombreProducto.AutoEllipsis = true;

                string formatoStock = _esFraccionado
                    ? $"{_stockActual:0.000} Kg"
                    : $"{_stockActual:0.##} unidades";

                _lblStockActualTitulo.Text = "Stock actual registrado:";
                _lblStockActualTitulo.Font = RecursosUI.FuenteEstandar;
                _lblStockActualTitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblStockActualTitulo.Location = new Point(16, 44);
                _lblStockActualTitulo.AutoSize = true;

                _lblStockActualValor.Text = formatoStock;
                _lblStockActualValor.Font = RecursosUI.FuenteNegrita;
                _lblStockActualValor.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblStockActualValor.Location = new Point(166, 44);
                _lblStockActualValor.AutoSize = true;

                // Entrada de Nuevo Stock
                _lblNuevoStockLabel.Text = "Nuevo Stock Real (Físico) *";
                _lblNuevoStockLabel.Font = RecursosUI.FuenteNegrita;
                _lblNuevoStockLabel.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblNuevoStockLabel.Location = new Point(28, 186);
                _lblNuevoStockLabel.AutoSize = true;

                _txtNuevoStock.Location = new Point(28, 212);
                _txtNuevoStock.Size = new Size(434, 34);
                _txtNuevoStock.Font = RecursosUI.FuenteInputGrande;
                _txtNuevoStock.ForeColor = RecursosUI.ColorTextoPrincipal;
                _txtNuevoStock.BorderStyle = BorderStyle.FixedSingle;
                _txtNuevoStock.Text = _esFraccionado
                    ? _stockActual.ToString("0.000", CultureInfo.InvariantCulture)
                    : _stockActual.ToString("0.##", CultureInfo.InvariantCulture);
                _txtNuevoStock.TabIndex = 0;

                _lblDiferenciaCalculada.Text = "Diferencia: 0 (Sin cambios)";
                _lblDiferenciaCalculada.Font = RecursosUI.FuenteNegrita;
                _lblDiferenciaCalculada.ForeColor = RecursosUI.ColorTextoMuted;
                _lblDiferenciaCalculada.Location = new Point(28, 258);
                _lblDiferenciaCalculada.AutoSize = true;

                // Footer
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 72;
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorBottom.Height = 1;
                _lblSeparadorBottom.Dock = DockStyle.Top;
                _lblSeparadorBottom.BackColor = RecursosUI.ColorSeparador;

                _btnConfirmar.Text = "Confirmar Ajuste (ENTER)";
                _btnConfirmar.BackColor = RecursosUI.ColorExito;
                _btnConfirmar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnConfirmar.FlatStyle = FlatStyle.Flat;
                _btnConfirmar.FlatAppearance.BorderSize = 0;
                _btnConfirmar.Size = new Size(230, 42);
                _btnConfirmar.Location = new Point(28, 15);
                _btnConfirmar.Font = RecursosUI.FuenteNegrita;
                _btnConfirmar.Cursor = Cursors.Hand;
                _btnConfirmar.TabIndex = 1;

                _btnCancelar.Text = "Cancelar (ESC)";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Size = new Size(184, 42);
                _btnCancelar.Location = new Point(278, 15);
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 2;

                // 5. Suscripción a Eventos
                _txtNuevoStock.TextChanged += (s, e) => RecalcularDiferencia();
                _btnConfirmar.Click += (s, e) => GuardarAjuste();
                _btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                this.Load += (s, e) =>
                {
                    this.ActiveControl = _txtNuevoStock;
                    _txtNuevoStock.SelectAll();
                };

                // 7. Agregado de Controles
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_lblSeparadorTop);

                _pnlDetalleProducto.Controls.Add(_lblNombreProducto);
                _pnlDetalleProducto.Controls.Add(_lblStockActualTitulo);
                _pnlDetalleProducto.Controls.Add(_lblStockActualValor);

                _pnlFooter.Controls.Add(_lblSeparadorBottom);
                _pnlFooter.Controls.Add(_btnConfirmar);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_pnlDetalleProducto);
                this.Controls.Add(_lblNuevoStockLabel);
                this.Controls.Add(_txtNuevoStock);
                this.Controls.Add(_lblDiferenciaCalculada);
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
            if (keyData == Keys.Enter && !_btnCancelar.Focused)
            {
                GuardarAjuste();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RecalcularDiferencia()
        {
            string texto = _txtNuevoStock.Text.Replace(",", ".").Trim();
            if (decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal nuevoStock))
            {
                decimal dif = nuevoStock - _stockActual;
                if (dif > 0)
                {
                    _lblDiferenciaCalculada.Text = $"Diferencia: +{dif:0.###} (Aumento por conteo físico)";
                    _lblDiferenciaCalculada.ForeColor = RecursosUI.ColorInventario;
                }
                else if (dif < 0)
                {
                    _lblDiferenciaCalculada.Text = $"Diferencia: {dif:0.###} (Merma, rotura o faltante)";
                    _lblDiferenciaCalculada.ForeColor = RecursosUI.ColorPeligro;
                }
                else
                {
                    _lblDiferenciaCalculada.Text = "Diferencia: 0 (Sin cambios)";
                    _lblDiferenciaCalculada.ForeColor = RecursosUI.ColorTextoMuted;
                }
            }
            else
            {
                _lblDiferenciaCalculada.Text = "Ingrese un valor numérico válido.";
                _lblDiferenciaCalculada.ForeColor = RecursosUI.ColorPeligro;
            }
        }

        private void GuardarAjuste()
        {
            string texto = _txtNuevoStock.Text.Replace(",", ".").Trim();
            if (!decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal nuevoStock))
            {
                MessageBox.Show(this, "Por favor ingrese un número válido para el nuevo stock.",
                    "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNuevoStock.Focus();
                return;
            }

            try
            {
                bool exito = _productoNegocio.ActualizarStockManual(_idProducto, nuevoStock);
                if (exito)
                {
                    MessageBox.Show(
                        this,
                        $"Stock de '{_nombreProducto}' actualizado correctamente a {nuevoStock:0.###}.",
                        "Ajuste Confirmado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "No se pudo actualizar el registro en la base de datos.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al actualizar el stock: " + ex.Message,
                    "Error en Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
