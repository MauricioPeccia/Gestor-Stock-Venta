using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Modal de Alta y Edición de Productos del Catálogo.
    /// 100% programático en InicializarComponentes() y libre de instanciaciones GDI+ directas.
    /// </summary>
    public sealed class FrmNuevoProducto : Form
    {
        private readonly ProductoNegocio _productoNegocio;
        private readonly Producto? _productoEdicion;

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Label _lblSeparadorTop = null!;

        private TextBox _txtCodigoBarras = null!;
        private TextBox _txtCodigoInterno = null!;
        private TextBox _txtNombre = null!;
        private TextBox _txtCosto = null!;
        private TextBox _txtVenta = null!;
        private TextBox _txtStock = null!;
        private TextBox _txtStockMinimo = null!;
        private CheckBox _chkFraccionado = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorBottom = null!;
        private Button _btnGuardar = null!;
        private Button _btnCancelar = null!;

        public FrmNuevoProducto(Producto? productoEdicion = null)
        {
            _productoNegocio = new ProductoNegocio();
            _productoEdicion = productoEdicion;

            InicializarComponentes();
        }

        public FrmNuevoProducto(int idProducto)
        {
            _productoNegocio = new ProductoNegocio();
            _productoEdicion = _productoNegocio.ObtenerProductoPorId(idProducto);

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                bool esEdicion = _productoEdicion != null;

                // 1. Configuración del Formulario
                this.Text = esEdicion ? "Editar Producto" : "Nuevo Producto";
                this.ClientSize = new Size(500, 520);
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

                _txtCodigoBarras = new TextBox();
                _txtCodigoInterno = new TextBox();
                _txtNombre = new TextBox();
                _txtCosto = new TextBox();
                _txtVenta = new TextBox();
                _txtStock = new TextBox();
                _txtStockMinimo = new TextBox();
                _chkFraccionado = new CheckBox();

                _pnlFooter = new Panel();
                _lblSeparadorBottom = new Label();
                _btnGuardar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 76;
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;

                _lblTitulo.Text = esEdicion ? "Editar Producto" : "Nuevo Producto";
                _lblTitulo.Font = RecursosUI.FuenteTituloApp;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(24, 14);

                _lblSubtitulo.Text = "Complete los datos comerciales de la mercadería  [ENTER: Guardar | ESC: Cancelar]";
                _lblSubtitulo.Font = RecursosUI.FuentePequena;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(26, 46);

                _lblSeparadorTop.Height = 1;
                _lblSeparadorTop.Dock = DockStyle.Bottom;
                _lblSeparadorTop.BackColor = RecursosUI.ColorSeparador;

                int xIzq = 28;
                int xDer = 264;
                int y = 94;

                // 6. Asignación secuencial de TabIndex (txtNombre = 0, precios, stock, códigos y botones)
                // Fila 1: Nombre / Descripción del Producto (Primer campo con TabIndex = 0)
                this.Controls.Add(CrearEtiqueta("Nombre / Descripción del Producto *", xIzq, y));
                ConfigurarCajaTexto(_txtNombre, xIzq, y + 22, 444, 0);

                // Fila 2: Precio Costo y Precio Venta
                y += 68;
                this.Controls.Add(CrearEtiqueta("Precio de Costo ($)", xIzq, y));
                ConfigurarCajaTexto(_txtCosto, xIzq, y + 22, 208, 1);
                _txtCosto.Text = "0.00";

                this.Controls.Add(CrearEtiqueta("Precio de Venta ($) *", xDer, y));
                ConfigurarCajaTexto(_txtVenta, xDer, y + 22, 208, 2);
                _txtVenta.Text = "0.00";

                // Fila 3: Stock Inicial/Actual y Stock Mínimo
                y += 68;
                this.Controls.Add(CrearEtiqueta(esEdicion ? "Stock Actual" : "Stock Inicial", xIzq, y));
                ConfigurarCajaTexto(_txtStock, xIzq, y + 22, 208, 3);
                _txtStock.Text = "0";

                this.Controls.Add(CrearEtiqueta("Stock Mínimo (Alerta)", xDer, y));
                ConfigurarCajaTexto(_txtStockMinimo, xDer, y + 22, 208, 4);
                _txtStockMinimo.Text = "5";

                // Fila 4: Código de Barras y Código Interno
                y += 68;
                this.Controls.Add(CrearEtiqueta("Código de Barras (opcional)", xIzq, y));
                ConfigurarCajaTexto(_txtCodigoBarras, xIzq, y + 22, 208, 5);

                this.Controls.Add(CrearEtiqueta("Código Interno / SKU", xDer, y));
                ConfigurarCajaTexto(_txtCodigoInterno, xDer, y + 22, 208, 6);

                // Fila 5: Fraccionado
                y += 68;
                _chkFraccionado.Text = "Se vende por peso o fracción (Kg / Monto)";
                _chkFraccionado.Font = RecursosUI.FuenteNegrita;
                _chkFraccionado.ForeColor = RecursosUI.ColorTextoPrincipal;
                _chkFraccionado.AutoSize = true;
                _chkFraccionado.Location = new Point(xIzq, y + 6);
                _chkFraccionado.TabIndex = 7;
                _chkFraccionado.TabStop = true;

                // Footer
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 72;
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorBottom.Height = 1;
                _lblSeparadorBottom.Dock = DockStyle.Top;
                _lblSeparadorBottom.BackColor = RecursosUI.ColorSeparador;

                _btnGuardar.Text = "Guardar Producto (ENTER)";
                _btnGuardar.BackColor = RecursosUI.ColorPrimario;
                _btnGuardar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnGuardar.FlatStyle = FlatStyle.Flat;
                _btnGuardar.FlatAppearance.BorderSize = 0;
                _btnGuardar.Size = new Size(230, 42);
                _btnGuardar.Location = new Point(28, 15);
                _btnGuardar.Font = RecursosUI.FuenteNegrita;
                _btnGuardar.Cursor = Cursors.Hand;
                _btnGuardar.TabIndex = 8;

                _btnCancelar.Text = "Cancelar (ESC)";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Size = new Size(195, 42);
                _btnCancelar.Location = new Point(277, 15);
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 9;

                if (_productoEdicion != null)
                {
                    _txtCodigoBarras.Text = _productoEdicion.CodigoBarras;
                    _txtCodigoInterno.Text = _productoEdicion.CodigoInterno;
                    _txtNombre.Text = _productoEdicion.Nombre;
                    _txtCosto.Text = _productoEdicion.PrecioCosto.ToString("0.00", CultureInfo.InvariantCulture);
                    _txtVenta.Text = _productoEdicion.PrecioVenta.ToString("0.00", CultureInfo.InvariantCulture);
                    _txtStock.Text = _productoEdicion.EsFraccionado
                        ? _productoEdicion.StockActual.ToString("0.000", CultureInfo.InvariantCulture)
                        : _productoEdicion.StockActual.ToString("0.##", CultureInfo.InvariantCulture);
                    _txtStockMinimo.Text = _productoEdicion.StockMinimo.ToString("0.##", CultureInfo.InvariantCulture);
                    _chkFraccionado.Checked = _productoEdicion.EsFraccionado;
                }

                // 5. Suscripción a Eventos y Foco Inicial
                _btnGuardar.Click += (s, e) => Guardar();
                _btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                this.Load += (s, e) =>
                {
                    _txtNombre.Focus();
                    _txtNombre.SelectAll();
                };

                // 7. Agregado de Controles
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_lblSeparadorTop);

                _pnlFooter.Controls.Add(_lblSeparadorBottom);
                _pnlFooter.Controls.Add(_btnGuardar);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_txtNombre);
                this.Controls.Add(_txtCosto);
                this.Controls.Add(_txtVenta);
                this.Controls.Add(_txtStock);
                this.Controls.Add(_txtStockMinimo);
                this.Controls.Add(_txtCodigoBarras);
                this.Controls.Add(_txtCodigoInterno);
                this.Controls.Add(_chkFraccionado);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);

                this.AcceptButton = _btnGuardar;
                this.CancelButton = _btnCancelar;
                this.ActiveControl = _txtNombre;
                _txtNombre.Focus();
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private static Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true,
                Font = RecursosUI.FuenteNegrita,
                ForeColor = RecursosUI.ColorTextoPrincipal
            };
        }

        private static void ConfigurarCajaTexto(TextBox txt, int x, int y, int width, int tabIndex)
        {
            txt.Location = new Point(x, y);
            txt.Width = width;
            txt.Font = RecursosUI.FuenteInput;
            txt.ForeColor = RecursosUI.ColorTextoPrincipal;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.TabIndex = tabIndex;
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
                Guardar();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(_txtNombre.Text))
            {
                MessageBox.Show(this, "El nombre del producto es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNombre.Focus();
                return;
            }

            if (!decimal.TryParse(_txtCosto.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal costo) || costo < 0)
            {
                MessageBox.Show(this, "Ingrese un precio de costo válido (>= 0).", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCosto.Focus();
                return;
            }

            if (!decimal.TryParse(_txtVenta.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal venta) || venta <= 0)
            {
                MessageBox.Show(this, "Ingrese un precio de venta válido (> 0).", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtVenta.Focus();
                return;
            }

            if (!decimal.TryParse(_txtStock.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal stock))
            {
                MessageBox.Show(this, "Ingrese una cantidad de stock numérica válida.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtStock.Focus();
                return;
            }

            if (!decimal.TryParse(_txtStockMinimo.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal stockMinimo))
            {
                stockMinimo = 5m;
            }

            try
            {
                var prod = new Producto
                {
                    Id = _productoEdicion?.Id ?? 0,
                    CodigoBarras = _txtCodigoBarras.Text.Trim(),
                    CodigoInterno = _txtCodigoInterno.Text.Trim(),
                    Nombre = _txtNombre.Text.Trim(),
                    PrecioCosto = costo,
                    PrecioVenta = venta,
                    StockActual = stock,
                    StockMinimo = stockMinimo,
                    EsFraccionado = _chkFraccionado.Checked,
                    Activo = true
                };

                if (_productoEdicion == null)
                {
                    _productoNegocio.CrearProducto(prod);
                }
                else
                {
                    _productoNegocio.ActualizarProducto(prod);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al guardar el producto: " + ex.Message,
                    "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
