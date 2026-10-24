using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Módulo 3: Movimientos de Caja (Egresos, Retiros, Ingresos y Compras de Mercadería).
    /// Construido 100% de forma programática en InicializarComponentes() y consumiendo RecursosUI.
    /// </summary>
    public sealed class FormMovimientos : Form
    {
        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Label _lblSeparadorHeader = null!;

        private Panel _pnlFormularioCard = null!;
        private Label _lblTipo = null!;
        private ComboBox _cboTipo = null!;
        private Label _lblConcepto = null!;
        private TextBox _txtConcepto = null!;
        private Label _lblMonto = null!;
        private TextBox _txtMonto = null!;

        private Panel _pnlCompraMercaderia = null!;
        private Label _lblProductoCompra = null!;
        private ComboBox _cboProductoCompra = null!;
        private Label _lblCantidadCompra = null!;
        private TextBox _txtCantidadCompra = null!;

        private Label _lblAyudaImpacto = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorFooter = null!;
        private Button _btnRegistrar = null!;
        private Button _btnCancelar = null!;

        private readonly MovimientoNegocio _movimientoNegocio;
        private readonly ProductoNegocio _productoNegocio;
        private List<Producto> _catalogoProductos;

        public FormMovimientos()
        {
            _movimientoNegocio = new MovimientoNegocio();
            _productoNegocio = new ProductoNegocio();
            _catalogoProductos = new List<Producto>();

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "[ 3 ] Movimientos de Caja - Kiosco Lite";
                this.ClientSize = new Size(580, 560);
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;
                this.DoubleBuffered = true;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();
                _lblSeparadorHeader = new Label();

                _pnlFormularioCard = new Panel();
                _lblTipo = new Label();
                _cboTipo = new ComboBox();
                _lblConcepto = new Label();
                _txtConcepto = new TextBox();
                _lblMonto = new Label();
                _txtMonto = new TextBox();

                _pnlCompraMercaderia = new Panel();
                _lblProductoCompra = new Label();
                _cboProductoCompra = new ComboBox();
                _lblCantidadCompra = new Label();
                _txtCantidadCompra = new TextBox();

                _lblAyudaImpacto = new Label();

                _pnlFooter = new Panel();
                _lblSeparadorFooter = new Label();
                _btnRegistrar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades y Estilos
                // --- Header ---
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 82;
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;

                _lblTitulo.Text = "Movimientos de Caja";
                _lblTitulo.Font = RecursosUI.FuenteTituloModulo;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(25, 14);

                _lblSubtitulo.Text = "Registro de gastos, retiros, ingresos y compras de mercadería";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(27, 48);

                _lblSeparadorHeader.Height = 1;
                _lblSeparadorHeader.Dock = DockStyle.Bottom;
                _lblSeparadorHeader.BackColor = RecursosUI.ColorSeparador;

                // --- Tarjeta Central ---
                _pnlFormularioCard.Location = new Point(30, 98);
                _pnlFormularioCard.Size = new Size(520, 368);
                _pnlFormularioCard.BackColor = RecursosUI.ColorSuperficie;

                _lblTipo.Text = "Tipo de Movimiento *";
                _lblTipo.Font = RecursosUI.FuenteNegrita;
                _lblTipo.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblTipo.AutoSize = true;
                _lblTipo.Location = new Point(24, 16);

                _cboTipo.Location = new Point(24, 38);
                _cboTipo.Size = new Size(472, 30);
                _cboTipo.Font = RecursosUI.FuenteInput;
                _cboTipo.ForeColor = RecursosUI.ColorTextoPrincipal;
                _cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
                _cboTipo.FlatStyle = FlatStyle.Flat;
                _cboTipo.Items.AddRange(new object[]
                {
                    "Gasto",
                    "Retiro",
                    "Ingreso",
                    "Compra Mercaderia"
                });
                _cboTipo.SelectedIndex = 0;
                _cboTipo.TabIndex = 0;

                _lblConcepto.Text = "Concepto / Detalle *";
                _lblConcepto.Font = RecursosUI.FuenteNegrita;
                _lblConcepto.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblConcepto.AutoSize = true;
                _lblConcepto.Location = new Point(24, 80);

                _txtConcepto.Location = new Point(24, 102);
                _txtConcepto.Size = new Size(472, 30);
                _txtConcepto.Font = RecursosUI.FuenteInput;
                _txtConcepto.ForeColor = RecursosUI.ColorTextoPrincipal;
                _txtConcepto.BorderStyle = BorderStyle.FixedSingle;
                _txtConcepto.TabIndex = 1;

                _lblMonto.Text = "Monto en Dinero ($) *";
                _lblMonto.Font = RecursosUI.FuenteNegrita;
                _lblMonto.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblMonto.AutoSize = true;
                _lblMonto.Location = new Point(24, 144);

                _txtMonto.Location = new Point(24, 166);
                _txtMonto.Size = new Size(472, 34);
                _txtMonto.Font = RecursosUI.FuenteInputGrande;
                _txtMonto.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _txtMonto.BorderStyle = BorderStyle.FixedSingle;
                _txtMonto.TabIndex = 2;

                // Panel opcional para Compra de Mercadería (sincronización de stock)
                _pnlCompraMercaderia.Location = new Point(24, 210);
                _pnlCompraMercaderia.Size = new Size(472, 92);
                _pnlCompraMercaderia.BackColor = RecursosUI.ColorFondoVentana;
                _pnlCompraMercaderia.Visible = false;

                _lblProductoCompra.Text = "Producto a incrementar Stock (Opcional):";
                _lblProductoCompra.Font = RecursosUI.FuentePequenaNegrita;
                _lblProductoCompra.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblProductoCompra.AutoSize = true;
                _lblProductoCompra.Location = new Point(12, 10);

                _cboProductoCompra.Location = new Point(12, 30);
                _cboProductoCompra.Size = new Size(310, 28);
                _cboProductoCompra.Font = RecursosUI.FuenteEstandar;
                _cboProductoCompra.DropDownStyle = ComboBoxStyle.DropDownList;
                _cboProductoCompra.FlatStyle = FlatStyle.Flat;
                _cboProductoCompra.TabIndex = 3;

                _lblCantidadCompra.Text = "Cant. Ingresada:";
                _lblCantidadCompra.Font = RecursosUI.FuentePequenaNegrita;
                _lblCantidadCompra.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblCantidadCompra.AutoSize = true;
                _lblCantidadCompra.Location = new Point(336, 10);

                _txtCantidadCompra.Location = new Point(336, 30);
                _txtCantidadCompra.Size = new Size(124, 28);
                _txtCantidadCompra.Font = RecursosUI.FuenteInput;
                _txtCantidadCompra.BorderStyle = BorderStyle.FixedSingle;
                _txtCantidadCompra.Text = "0";
                _txtCantidadCompra.TabIndex = 4;

                _lblAyudaImpacto.Text = "• Gasto, Retiro o Compra restan dinero de la caja diaria.\n• Compra Mercadería sincroniza además el aumento de stock si elige un producto.";
                _lblAyudaImpacto.Font = RecursosUI.FuentePequena;
                _lblAyudaImpacto.ForeColor = RecursosUI.ColorTextoMuted;
                _lblAyudaImpacto.Location = new Point(24, 314);
                _lblAyudaImpacto.Size = new Size(472, 42);

                // --- Footer ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 74;
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorFooter.Height = 1;
                _lblSeparadorFooter.Dock = DockStyle.Top;
                _lblSeparadorFooter.BackColor = RecursosUI.ColorSeparador;

                _btnRegistrar.Text = "Registrar Movimiento (ENTER)";
                _btnRegistrar.BackColor = RecursosUI.ColorExito;
                _btnRegistrar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnRegistrar.FlatStyle = FlatStyle.Flat;
                _btnRegistrar.FlatAppearance.BorderSize = 0;
                _btnRegistrar.Size = new Size(255, 42);
                _btnRegistrar.Location = new Point(30, 16);
                _btnRegistrar.Font = RecursosUI.FuenteNegrita;
                _btnRegistrar.Cursor = Cursors.Hand;
                _btnRegistrar.TabIndex = 5;

                _btnCancelar.Text = "Cancelar (ESC)";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Size = new Size(185, 42);
                _btnCancelar.Location = new Point(365, 16);
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 6;

                // 5. Suscripción a Eventos
                _cboTipo.SelectedIndexChanged += (s, e) =>
                {
                    bool esCompra = string.Equals(_cboTipo.SelectedItem?.ToString(), "Compra Mercaderia", StringComparison.OrdinalIgnoreCase);
                    _pnlCompraMercaderia.Visible = esCompra;
                    if (esCompra && _cboProductoCompra.Items.Count == 0)
                    {
                        CargarProductosCombo();
                    }
                };

                _btnRegistrar.Click += (s, e) => EjecutarRegistro();
                _btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                this.Load += (s, e) => this.ActiveControl = _txtConcepto;

                // 7. Agregado de Controles
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_lblSeparadorHeader);

                _pnlCompraMercaderia.Controls.Add(_lblProductoCompra);
                _pnlCompraMercaderia.Controls.Add(_cboProductoCompra);
                _pnlCompraMercaderia.Controls.Add(_lblCantidadCompra);
                _pnlCompraMercaderia.Controls.Add(_txtCantidadCompra);

                _pnlFormularioCard.Controls.Add(_lblTipo);
                _pnlFormularioCard.Controls.Add(_cboTipo);
                _pnlFormularioCard.Controls.Add(_lblConcepto);
                _pnlFormularioCard.Controls.Add(_txtConcepto);
                _pnlFormularioCard.Controls.Add(_lblMonto);
                _pnlFormularioCard.Controls.Add(_txtMonto);
                _pnlFormularioCard.Controls.Add(_pnlCompraMercaderia);
                _pnlFormularioCard.Controls.Add(_lblAyudaImpacto);

                _pnlFooter.Controls.Add(_lblSeparadorFooter);
                _pnlFooter.Controls.Add(_btnRegistrar);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_pnlFormularioCard);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private void CargarProductosCombo()
        {
            try
            {
                _catalogoProductos = _productoNegocio.ObtenerCatalogoActivo();
                _cboProductoCompra.Items.Clear();
                _cboProductoCompra.Items.Add("-- Sin vincular producto específico --");
                foreach (var p in _catalogoProductos)
                {
                    _cboProductoCompra.Items.Add($"{p.Id} - {p.Nombre}");
                }
                _cboProductoCompra.SelectedIndex = 0;
            }
            catch
            {
                // Si no se puede poblar el combo opcional, permitir el registro contable normal
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
            if (keyData == Keys.Enter && !_btnCancelar.Focused && !_cboTipo.DroppedDown && !_cboProductoCompra.DroppedDown)
            {
                EjecutarRegistro();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EjecutarRegistro()
        {
            if (_cboTipo.SelectedItem == null)
            {
                MessageBox.Show(this, "Por favor seleccione el tipo de movimiento.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cboTipo.Focus();
                return;
            }

            string concepto = _txtConcepto.Text.Trim();
            if (string.IsNullOrWhiteSpace(concepto))
            {
                MessageBox.Show(this, "Por favor ingrese el concepto o detalle del movimiento (ej. 'Pago de Luz').",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConcepto.Focus();
                return;
            }

            string montoTexto = _txtMonto.Text.Replace(",", ".").Trim();
            if (!decimal.TryParse(montoTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal monto) || monto <= 0)
            {
                MessageBox.Show(this, "Ingrese un monto numérico mayor a cero.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtMonto.Focus();
                _txtMonto.SelectAll();
                return;
            }

            try
            {
                var movimiento = new MovimientoCaja
                {
                    TipoMovimiento = _cboTipo.SelectedItem.ToString() ?? "Gasto",
                    Concepto = concepto,
                    Monto = monto,
                    Fecha = DateTime.Now
                };

                if (_pnlCompraMercaderia.Visible && _cboProductoCompra.SelectedIndex > 0)
                {
                    int indexProd = _cboProductoCompra.SelectedIndex - 1;
                    if (indexProd >= 0 && indexProd < _catalogoProductos.Count)
                    {
                        var prod = _catalogoProductos[indexProd];
                        if (decimal.TryParse(_txtCantidadCompra.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cant) && cant > 0)
                        {
                            movimiento.IdProductoCompra = prod.Id;
                            movimiento.CantidadCompra = cant;
                            movimiento.PrecioCostoUnitario = Math.Round(monto / cant, 2);
                        }
                    }
                }

                bool exito = _movimientoNegocio.RegistrarMovimiento(movimiento);
                if (exito)
                {
                    MessageBox.Show(
                        this,
                        $"¡Movimiento registrado con éxito!\n\nTipo: {movimiento.TipoMovimiento}\nConcepto: {movimiento.Concepto}\nMonto: $ {movimiento.Monto:N2}",
                        "Kiosco Lite",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "No se pudo registrar el movimiento en la base de datos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al registrar movimiento: " + ex.Message,
                    "Error en Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
