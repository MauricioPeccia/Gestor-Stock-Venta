using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Módulo 1: Terminal de Ventas (Caja).
    /// Construido 100% por código en InicializarComponentes() consumiendo exclusivamente RecursosUI.
    /// </summary>
    public sealed class FormCaja : Form
    {
        private const string TextoPlaceholder = "Escanear código o escribir nombre...";
        private const string NombreColumnaRestar = "ColRestar";
        private const string NombreColumnaSumar = "ColSumar";

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;

        private Panel _pnlToolbar = null!;
        private TextBox _txtBuscar = null!;
        private Button _btnBuscar = null!;
        private Button _btnEliminarItem = null!;
        private Button _btnLimpiarVenta = null!;
        private Button _btnCobrar = null!;

        private Panel pnlTotal = null!;
        private Label lblTituloTotal = null!;
        private Label lblMontoTotal = null!;

        private DataGridView _dgvDetalle = null!;

        private Panel _pnlFooter = null!;
        private Label _lblAtajosAyuda = null!;
        private Label _lblTotalItems = null!;

        private readonly AutoCompleteStringCollection _fuenteAutocompletado;
        private readonly VentaNegocio _ventaNegocio;
        private List<Producto> _catalogoMemoria;
        private readonly List<DetalleVenta> _itemsVenta;

        public FormCaja()
        {
            _fuenteAutocompletado = new AutoCompleteStringCollection();
            _ventaNegocio = new VentaNegocio();
            _catalogoMemoria = new List<Producto>();
            _itemsVenta = new List<DetalleVenta>();

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "[ 1 ] Terminal de Ventas - Kiosco Lite";
                this.ClientSize = new Size(1280, 720);
                this.MinimumSize = new Size(1024, 600);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Maximized;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.KeyPreview = true;
                this.DoubleBuffered = true;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();

                _pnlToolbar = new Panel();
                _txtBuscar = new TextBox();
                _btnBuscar = new Button();
                _btnEliminarItem = new Button();
                _btnLimpiarVenta = new Button();
                _btnCobrar = new Button();

                pnlTotal = new Panel();
                lblTituloTotal = new Label();
                lblMontoTotal = new Label();

                _dgvDetalle = new DataGridView();

                _pnlFooter = new Panel();
                _lblAtajosAyuda = new Label();
                _lblTotalItems = new Label();

                // 3. Configuración de Propiedades y Estilos (RecursosUI)
                // --- Header ---
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Size = new Size(this.ClientSize.Width, 76);
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;
                _pnlHeader.Padding = new Padding(20, 10, 20, 10);

                _lblTitulo.Text = "Caja Registradora";
                _lblTitulo.Font = RecursosUI.FuenteTituloModulo;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(20, 10);

                _lblSubtitulo.Text = "Escanee código o escriba el nombre del producto  |  Catálogo [F5]  |  Cobro rápido [F12]";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(22, 46);

                // --- Toolbar Superior (Buscador + Botones + Panel TOTAL A COBRAR alineado a la derecha) ---
                _pnlToolbar.Dock = DockStyle.Top;
                _pnlToolbar.Size = new Size(this.ClientSize.Width, 92);
                _pnlToolbar.BackColor = RecursosUI.ColorFondoVentana;
                _pnlToolbar.Padding = new Padding(20, 10, 20, 10);

                _txtBuscar.Font = RecursosUI.FuenteInput;
                _txtBuscar.Size = new Size(330, 34);
                _txtBuscar.Location = new Point(20, 29);
                _txtBuscar.Text = TextoPlaceholder;
                _txtBuscar.ForeColor = RecursosUI.ColorTextoMuted;
                _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
                _txtBuscar.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                _txtBuscar.AutoCompleteSource = AutoCompleteSource.CustomSource;
                _txtBuscar.AutoCompleteCustomSource = _fuenteAutocompletado;
                _txtBuscar.TabIndex = 0;

                _btnBuscar = CrearBotonToolbar("F5 Catálogo", RecursosUI.ColorPrimario, new Point(362, 27), new Size(115, 36), 1);
                _btnEliminarItem = CrearBotonToolbar("Quitar (Supr)", RecursosUI.ColorPeligro, new Point(487, 27), new Size(120, 36), 2);
                _btnLimpiarVenta = CrearBotonToolbar("Limpiar", RecursosUI.ColorNeutro, new Point(617, 27), new Size(100, 36), 3);
                _btnCobrar = CrearBotonToolbar("Cobrar Venta [F12]", RecursosUI.ColorExito, new Point(727, 27), new Size(180, 36), 4);

                // --- Panel Destacado "TOTAL A COBRAR" (Alineado horizontalmente con el buscador en la esquina superior derecha) ---
                pnlTotal.Size = new Size(320, 74);
                pnlTotal.Location = new Point(_pnlToolbar.Width - 340, 9);
                pnlTotal.BackColor = RecursosUI.ColorFondoPanelTotal;
                pnlTotal.BorderStyle = BorderStyle.FixedSingle;
                pnlTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                lblTituloTotal.Text = "TOTAL A COBRAR";
                lblTituloTotal.Font = RecursosUI.FuenteEstandar;
                lblTituloTotal.ForeColor = RecursosUI.ColorTextoSecundario;
                lblTituloTotal.Dock = DockStyle.Top;
                lblTituloTotal.Height = 22;
                lblTituloTotal.TextAlign = ContentAlignment.MiddleRight;
                lblTituloTotal.Padding = new Padding(0, 4, 12, 0);

                lblMontoTotal.Text = "$ 0,00";
                lblMontoTotal.Font = RecursosUI.FuenteTotalCaja;
                lblMontoTotal.ForeColor = RecursosUI.ColorTextoTotalCaja;
                lblMontoTotal.Dock = DockStyle.Fill;
                lblMontoTotal.TextAlign = ContentAlignment.MiddleRight;
                lblMontoTotal.Padding = new Padding(0, 0, 10, 2);

                // --- Grilla de Detalle de Venta con Filas Alternadas y Botones +/- ---
                _dgvDetalle.Dock = DockStyle.Fill;
                _dgvDetalle.BackgroundColor = RecursosUI.ColorSuperficie;
                _dgvDetalle.BorderStyle = BorderStyle.None;
                _dgvDetalle.EnableHeadersVisualStyles = false;
                _dgvDetalle.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                _dgvDetalle.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                _dgvDetalle.GridColor = RecursosUI.ColorBorde;

                _dgvDetalle.ColumnHeadersDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvDetalle.ColumnHeadersDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvDetalle.ColumnHeadersDefaultCellStyle.Font = RecursosUI.FuenteGrillaHeader;
                _dgvDetalle.ColumnHeadersHeight = 44;
                _dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                _dgvDetalle.DefaultCellStyle.BackColor = RecursosUI.ColorSuperficie;
                _dgvDetalle.DefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvDetalle.DefaultCellStyle.Font = RecursosUI.FuenteEstandar;
                _dgvDetalle.DefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvDetalle.DefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvDetalle.DefaultCellStyle.Padding = new Padding(6);

                _dgvDetalle.AlternatingRowsDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvDetalle.AlternatingRowsDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvDetalle.AlternatingRowsDefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvDetalle.AlternatingRowsDefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;

                _dgvDetalle.RowTemplate.Height = 40;
                _dgvDetalle.AllowUserToAddRows = false;
                _dgvDetalle.AllowUserToDeleteRows = false;
                _dgvDetalle.ReadOnly = true;
                _dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _dgvDetalle.MultiSelect = false;
                _dgvDetalle.AllowUserToResizeColumns = false;
                _dgvDetalle.AllowUserToResizeRows = false;
                _dgvDetalle.RowHeadersVisible = false;
                _dgvDetalle.TabIndex = 5;
                _dgvDetalle.TabStop = false;

                _dgvDetalle.Columns.Add("Descripcion", "Descripción del Producto");
                _dgvDetalle.Columns.Add("Cantidad", "Cantidad");

                var colRestar = new DataGridViewButtonColumn
                {
                    Name = NombreColumnaRestar,
                    HeaderText = "-",
                    Text = "-",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 52
                };
                colRestar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colRestar.DefaultCellStyle.Font = RecursosUI.FuenteNegrita;
                colRestar.DefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                colRestar.DefaultCellStyle.ForeColor = RecursosUI.ColorPeligro;

                var colSumar = new DataGridViewButtonColumn
                {
                    Name = NombreColumnaSumar,
                    HeaderText = "+",
                    Text = "+",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 52
                };
                colSumar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colSumar.DefaultCellStyle.Font = RecursosUI.FuenteNegrita;
                colSumar.DefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                colSumar.DefaultCellStyle.ForeColor = RecursosUI.ColorInventario;

                _dgvDetalle.Columns.Add(colRestar);
                _dgvDetalle.Columns.Add(colSumar);
                _dgvDetalle.Columns.Add("PrecioUnitario", "Precio Unitario");
                _dgvDetalle.Columns.Add("Subtotal", "Subtotal");

                _dgvDetalle.Columns["Descripcion"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                _dgvDetalle.Columns["Cantidad"]!.Width = 120;
                _dgvDetalle.Columns["PrecioUnitario"]!.Width = 160;
                _dgvDetalle.Columns["Subtotal"]!.Width = 170;

                _dgvDetalle.Columns["Cantidad"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvDetalle.Columns[NombreColumnaRestar]!.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _dgvDetalle.Columns[NombreColumnaSumar]!.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _dgvDetalle.Columns["PrecioUnitario"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvDetalle.Columns["Subtotal"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvDetalle.Columns["Subtotal"]!.DefaultCellStyle.Font = RecursosUI.FuenteNegrita;

                foreach (DataGridViewColumn col in _dgvDetalle.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // --- Footer ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Size = new Size(this.ClientSize.Width, 42);
                _pnlFooter.BackColor = RecursosUI.ColorBarraEstado;

                _lblAtajosAyuda.Text = "[ENTER] Agregar   |   [+/-] Ajustar cantidad   |   [F5] Buscador / Catálogo   |   [F12] Cobrar Venta   |   [SUPR] Quitar   |   [ESC] Foco / Volver";
                _lblAtajosAyuda.Font = RecursosUI.FuentePequenaNegrita;
                _lblAtajosAyuda.ForeColor = RecursosUI.ColorTextoClaro;
                _lblAtajosAyuda.AutoSize = true;
                _lblAtajosAyuda.Location = new Point(20, 12);

                _lblTotalItems.Text = "Artículos en carrito: 0";
                _lblTotalItems.Font = RecursosUI.FuenteNegrita;
                _lblTotalItems.ForeColor = RecursosUI.ColorTextoClaro;
                _lblTotalItems.AutoSize = true;
                _lblTotalItems.Location = new Point(_pnlFooter.Width - 200, 11);
                _lblTotalItems.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                // 5. Suscripción a Eventos
                _txtBuscar.Enter += (s, e) =>
                {
                    if (_txtBuscar.Text == TextoPlaceholder)
                    {
                        _txtBuscar.Text = string.Empty;
                        _txtBuscar.ForeColor = RecursosUI.ColorTextoPrincipal;
                    }
                };

                _txtBuscar.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(_txtBuscar.Text))
                    {
                        _txtBuscar.Text = TextoPlaceholder;
                        _txtBuscar.ForeColor = RecursosUI.ColorTextoMuted;
                    }
                };

                _txtBuscar.KeyDown += TxtBuscar_KeyDown;

                _dgvDetalle.CellContentClick += DgvDetalle_CellContentClick;
                _dgvDetalle.KeyDown += DgvDetalle_KeyDown;

                this.KeyDown += FormCaja_KeyDown;

                _btnBuscar.Click += (s, e) => AbrirBuscadorRapido();
                _btnEliminarItem.Click += (s, e) => QuitarItemSeleccionado();
                _btnLimpiarVenta.Click += (s, e) => LimpiarVenta();
                _btnCobrar.Click += (s, e) => AbrirModalCobro();

                this.Load += (s, e) =>
                {
                    RecargarCatalogoEnMemoria();
                    ActualizarTotalVenta();
                    EnfocarBuscador();
                };

                // 7. Agregado de Controles
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);

                pnlTotal.Controls.Add(lblMontoTotal);
                pnlTotal.Controls.Add(lblTituloTotal);

                _pnlToolbar.Controls.Add(_txtBuscar);
                _pnlToolbar.Controls.Add(_btnBuscar);
                _pnlToolbar.Controls.Add(_btnEliminarItem);
                _pnlToolbar.Controls.Add(_btnLimpiarVenta);
                _pnlToolbar.Controls.Add(_btnCobrar);
                _pnlToolbar.Controls.Add(pnlTotal);

                _pnlFooter.Controls.Add(_lblAtajosAyuda);
                _pnlFooter.Controls.Add(_lblTotalItems);

                this.Controls.Add(_dgvDetalle);
                this.Controls.Add(_pnlToolbar);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private static Button CrearBotonToolbar(string texto, Color fondo, Point ubicacion, Size tamano, int tabIndex)
        {
            var btn = new Button
            {
                Text = texto,
                BackColor = fondo,
                ForeColor = RecursosUI.ColorTextoClaro,
                FlatStyle = FlatStyle.Flat,
                Size = tamano,
                Location = ubicacion,
                Font = RecursosUI.FuenteNegrita,
                Cursor = Cursors.Hand,
                TabIndex = tabIndex
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        /// <summary>
        /// Devuelve inmediatamente el foco al cuadro de búsqueda evitando que el usuario quede atrapado en la grilla.
        /// </summary>
        private void EnfocarBuscador()
        {
            if (_txtBuscar.Text == TextoPlaceholder)
            {
                _txtBuscar.Text = string.Empty;
                _txtBuscar.ForeColor = RecursosUI.ColorTextoPrincipal;
            }
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        /// <summary>
        /// Maneja Enter (búsqueda sin acentos) y Flecha Abajo (transferencia directa de foco a la grilla del carrito).
        /// </summary>
        private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ProcesarEntradaCodigo();
            }
            else if (e.KeyCode == Keys.Down && _dgvDetalle.Rows.Count > 0)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _dgvDetalle.Focus();

                int targetRow = _dgvDetalle.SelectedRows.Count > 0
                    ? _dgvDetalle.SelectedRows[0].Index
                    : 0;

                _dgvDetalle.CurrentCell = _dgvDetalle.Rows[targetRow].Cells["Descripcion"];
                _dgvDetalle.Rows[targetRow].Selected = true;
            }
        }

        /// <summary>
        /// Intercepta eventos de teclado en la grilla para garantizar retorno inmediato al buscador con ESC o F5
        /// y ajuste rápido de cantidades con teclas + y -.
        /// </summary>
        private void DgvDetalle_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.F2 || e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                EnfocarBuscador();
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                int filaActual = _dgvDetalle.CurrentRow?.Index
                    ?? (_dgvDetalle.SelectedRows.Count > 0 ? _dgvDetalle.SelectedRows[0].Index : 0);

                if (filaActual <= 0)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    EnfocarBuscador();
                    return;
                }
            }

            if (_dgvDetalle.SelectedRows.Count > 0)
            {
                int rowIndex = _dgvDetalle.SelectedRows[0].Index;
                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    ModificarCantidadFila(rowIndex, 1m);
                }
                else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    ModificarCantidadFila(rowIndex, -1m);
                }
            }
        }

        /// <summary>
        /// Controla atajos de teclado a nivel formulario.
        /// </summary>
        private void FormCaja_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                EnfocarBuscador();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                    if (_dgvDetalle.Focused)
                    {
                        EnfocarBuscador();
                    }
                    else
                    {
                        AbrirBuscadorRapido();
                    }
                    return true;

                case Keys.F12:
                    AbrirModalCobro();
                    return true;

                case Keys.Delete:
                    if (!_txtBuscar.Focused)
                    {
                        QuitarItemSeleccionado();
                        return true;
                    }
                    break;

                case Keys.Escape:
                    if (_dgvDetalle.Focused)
                    {
                        EnfocarBuscador();
                        return true;
                    }

                    if (_itemsVenta.Count > 0)
                    {
                        if (MessageBox.Show(this, "¿Desea cancelar y vaciar la venta en curso?", "Cancelar Venta",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            LimpiarVenta();
                        }
                        else
                        {
                            EnfocarBuscador();
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Carga los productos activos en memoria y actualiza la colección de autocompletado nativo de txtBuscar.
        /// </summary>
        public void RecargarCatalogoEnMemoria()
        {
            try
            {
                _catalogoMemoria = _ventaNegocio.ObtenerCatalogoEnMemoria();
                ConfigurarAutocompletado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error cargando el catálogo en memoria: " + ex.Message,
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarAutocompletado()
        {
            _fuenteAutocompletado.Clear();
            if (_catalogoMemoria.Count == 0) return;

            string[] nombresUnicos = _catalogoMemoria
                .Where(p => !string.IsNullOrWhiteSpace(p.Nombre))
                .Select(p => p.Nombre.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            _fuenteAutocompletado.AddRange(nombresUnicos);
        }

        /// <summary>
        /// Limpia los signos diacríticos (acentos, diéresis) descomponiendo en FormD
        /// y descartando caracteres UnicodeCategory.NonSpacingMark.
        /// </summary>
        private static string QuitarAcentos(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(textoNormalizado.Length);

            foreach (char c in textoNormalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void ProcesarEntradaCodigo()
        {
            string texto = _txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(texto) || texto == TextoPlaceholder) return;

            string textoSinAcentos = QuitarAcentos(texto);

            var producto = _catalogoMemoria.FirstOrDefault(p =>
                (!string.IsNullOrEmpty(p.CodigoBarras) && QuitarAcentos(p.CodigoBarras).Equals(textoSinAcentos, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.CodigoInterno) && QuitarAcentos(p.CodigoInterno).Equals(textoSinAcentos, StringComparison.OrdinalIgnoreCase)) ||
                QuitarAcentos(p.Nombre).Equals(textoSinAcentos, StringComparison.OrdinalIgnoreCase));

            if (producto == null)
            {
                var coincidencias = _catalogoMemoria.Where(p =>
                    QuitarAcentos(p.Nombre).IndexOf(textoSinAcentos, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                if (coincidencias.Count == 1)
                {
                    producto = coincidencias[0];
                }
                else if (coincidencias.Count > 1)
                {
                    AbrirBuscadorRapido(texto);
                    return;
                }
            }

            if (producto != null)
            {
                AgregarProductoAlCarrito(producto);
                _txtBuscar.Clear();
                EnfocarBuscador();
            }
            else
            {
                MessageBox.Show(this, "No se encontró ningún producto con el código o nombre ingresado.",
                    "Producto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EnfocarBuscador();
            }
        }

        public void AgregarProductoAlCarrito(Producto producto)
        {
            decimal cantidad = 1m;
            decimal subtotalCalculado = Math.Round(cantidad * producto.PrecioVenta, 2);

            if (producto.EsFraccionado)
            {
                using (var frmFraccionado = new FormCantidadFraccionada(producto.Nombre, producto.PrecioVenta))
                {
                    if (frmFraccionado.ShowDialog(this) == DialogResult.OK)
                    {
                        cantidad = frmFraccionado.CantidadKilos;
                        subtotalCalculado = frmFraccionado.MontoCalculado > 0m
                            ? frmFraccionado.MontoCalculado
                            : Math.Round(cantidad * producto.PrecioVenta, 2);
                    }
                    else
                    {
                        EnfocarBuscador();
                        return;
                    }
                }
            }

            var itemExistente = _itemsVenta.FirstOrDefault(i => i.IdProducto == producto.Id);
            if (itemExistente != null && !producto.EsFraccionado)
            {
                itemExistente.Cantidad += cantidad;
                itemExistente.Subtotal = Math.Round(itemExistente.Cantidad * itemExistente.PrecioUnitario, 2);
                itemExistente.VerificarStockNegativo(producto.StockActual);
            }
            else
            {
                var nuevoDetalle = new DetalleVenta
                {
                    IdProducto = producto.Id,
                    Codigo = !string.IsNullOrWhiteSpace(producto.CodigoBarras) ? producto.CodigoBarras : producto.CodigoInterno,
                    Descripcion = producto.Nombre,
                    PrecioCostoHistorico = producto.PrecioCosto,
                    PrecioUnitario = producto.PrecioVenta,
                    Cantidad = cantidad,
                    Subtotal = subtotalCalculado,
                    StockActualProducto = producto.StockActual,
                    EsFraccionado = producto.EsFraccionado
                };
                nuevoDetalle.VerificarStockNegativo(producto.StockActual);
                _itemsVenta.Add(nuevoDetalle);
            }

            RefrescarGrilla();
            ActualizarTotalVenta();
        }

        /// <summary>
        /// Maneja el clic en las columnas de botón "+" y "-" para sumar o restar la cantidad de la fila directamente.
        /// </summary>
        private void DgvDetalle_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _itemsVenta.Count) return;

            string nombreColumna = _dgvDetalle.Columns[e.ColumnIndex].Name;
            if (nombreColumna == NombreColumnaSumar)
            {
                ModificarCantidadFila(e.RowIndex, 1m);
                ActualizarTotalVenta();
                EnfocarBuscador();
            }
            else if (nombreColumna == NombreColumnaRestar)
            {
                ModificarCantidadFila(e.RowIndex, -1m);
                ActualizarTotalVenta();
                EnfocarBuscador();
            }
        }

        private void ModificarCantidadFila(int rowIndex, decimal delta)
        {
            if (rowIndex < 0 || rowIndex >= _itemsVenta.Count) return;

            DetalleVenta item = _itemsVenta[rowIndex];
            decimal paso = item.EsFraccionado ? 0.100m * Math.Sign(delta) : delta;
            decimal nuevaCantidad = Math.Round(item.Cantidad + paso, 3);

            if (nuevaCantidad <= 0m)
            {
                _itemsVenta.RemoveAt(rowIndex);
            }
            else
            {
                item.Cantidad = nuevaCantidad;
                item.Subtotal = Math.Round(item.Cantidad * item.PrecioUnitario, 2);
                item.VerificarStockNegativo(item.StockActualProducto);
            }

            RefrescarGrilla(rowIndex);
            ActualizarTotalVenta();
        }

        private void RefrescarGrilla(int filaSeleccionadaPreferida = -1)
        {
            _dgvDetalle.Rows.Clear();

            foreach (var item in _itemsVenta)
            {
                int rowIndex = _dgvDetalle.Rows.Add(
                    item.Descripcion,
                    item.EsFraccionado ? item.Cantidad.ToString("0.000") : item.Cantidad.ToString("0.##"),
                    "-",
                    "+",
                    $"$ {item.PrecioUnitario:N2}",
                    $"$ {item.Subtotal:N2}"
                );

                _dgvDetalle.Rows[rowIndex].Cells["Subtotal"].Tag = item.Subtotal;

                if (item.DejaStockNegativo)
                {
                    _dgvDetalle.Rows[rowIndex].DefaultCellStyle.BackColor = RecursosUI.ColorAlertaStockFondo;
                    _dgvDetalle.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = RecursosUI.ColorAlertaStockSel;

                    foreach (DataGridViewCell celda in _dgvDetalle.Rows[rowIndex].Cells)
                    {
                        celda.ToolTipText = $"Alerta: Esta venta deja stock en negativo (Stock actual: {item.StockActualProducto:0.###})";
                    }
                }
            }

            ActualizarTotalVenta();

            if (_dgvDetalle.Rows.Count > 0)
            {
                int targetIndex = filaSeleccionadaPreferida >= 0 && filaSeleccionadaPreferida < _dgvDetalle.Rows.Count
                    ? filaSeleccionadaPreferida
                    : _dgvDetalle.Rows.Count - 1;

                _dgvDetalle.FirstDisplayedScrollingRowIndex = targetIndex;
                _dgvDetalle.Rows[targetIndex].Selected = true;
            }
        }

        /// <summary>
        /// Itera sobre las filas del DataGridView, suma los valores de la columna "Subtotal"
        /// y asigna el resultado formateado como moneda a lblMontoTotal.Text.
        /// </summary>
        private void ActualizarTotalVenta()
        {
            decimal total = 0m;

            foreach (DataGridViewRow fila in _dgvDetalle.Rows)
            {
                if (fila.IsNewRow) continue;

                DataGridViewCell celdaSubtotal = fila.Cells["Subtotal"];
                if (celdaSubtotal.Tag is decimal subtotalTag)
                {
                    total += subtotalTag;
                }
                else if (celdaSubtotal.Value != null)
                {
                    string textoValor = celdaSubtotal.Value.ToString()!
                        .Replace("$", string.Empty)
                        .Trim();

                    if (decimal.TryParse(textoValor, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal valorParseado))
                    {
                        total += valorParseado;
                    }
                }
            }

            lblMontoTotal.Text = $"$ {total:N2}";
            _lblTotalItems.Text = $"Artículos en carrito: {_dgvDetalle.Rows.Count}";
        }

        private void QuitarItemSeleccionado()
        {
            if (_dgvDetalle.SelectedRows.Count == 0) return;

            int index = _dgvDetalle.SelectedRows[0].Index;
            if (index >= 0 && index < _itemsVenta.Count)
            {
                _itemsVenta.RemoveAt(index);
                RefrescarGrilla();
                ActualizarTotalVenta();
            }
            EnfocarBuscador();
        }

        private void LimpiarVenta()
        {
            _itemsVenta.Clear();
            RefrescarGrilla();
            ActualizarTotalVenta();
            _txtBuscar.Clear();
            EnfocarBuscador();
        }

        private void AbrirBuscadorRapido(string filtroInicial = "")
        {
            using (var frmBuscar = new FormBuscarProductoModal(_catalogoMemoria, filtroInicial))
            {
                if (frmBuscar.ShowDialog(this) == DialogResult.OK && frmBuscar.ProductoSeleccionado != null)
                {
                    AgregarProductoAlCarrito(frmBuscar.ProductoSeleccionado);
                    _txtBuscar.Clear();
                }
            }
            EnfocarBuscador();
        }

        private void AbrirModalCobro()
        {
            if (_itemsVenta.Count == 0)
            {
                MessageBox.Show(this, "El carrito de ventas está vacío. Agregue al menos un producto.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnfocarBuscador();
                return;
            }

            decimal totalVenta = _itemsVenta.Sum(i => i.Subtotal);

            using (var frmCobro = new FormCobrarVenta(totalVenta, _itemsVenta, _ventaNegocio))
            {
                if (frmCobro.ShowDialog(this) == DialogResult.OK)
                {
                    RecargarCatalogoEnMemoria();
                    LimpiarVenta();
                }
            }
            EnfocarBuscador();
        }
    }
}
