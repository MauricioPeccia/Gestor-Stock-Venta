using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Módulo 2: Gestión de Productos e Inventario (Stock).
    /// Construido 100% de forma programática en InicializarComponentes() y consumiendo RecursosUI.
    /// </summary>
    public class FrmProductos : Form
    {
        private const string TextoPlaceholder = "Buscar producto...";

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Panel _pnlTotalHeader = null!;
        private Label _lblTotalHeaderTitulo = null!;
        private Label _lblTotalHeaderValor = null!;

        private Panel _pnlToolbar = null!;
        private Button _btnAgregar = null!;
        private Button _btnEditar = null!;
        private Button _btnEliminar = null!;
        private Button _btnLimpiar = null!;
        private Button _btnAjusteStock = null!;
        private TextBox _txtBuscador = null!;
        private Button btnBuscar = null!;

        private DataGridView _dgvProductos = null!;

        private Panel _pnlFooter = null!;
        private Label _lblTotalFooterValor = null!;

        private readonly ProductoNegocio _productoNegocio;
        private List<Producto> _catalogoProductos;

        public FrmProductos()
        {
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
                this.Text = "[ 2 ] Gestión de Productos e Inventario - Kiosco Lite";
                this.ClientSize = new Size(1080, 680);
                this.MinimumSize = new Size(940, 580);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.KeyPreview = true;
                this.DoubleBuffered = true;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();
                _pnlTotalHeader = new Panel();
                _lblTotalHeaderTitulo = new Label();
                _lblTotalHeaderValor = new Label();

                _pnlToolbar = new Panel();
                _btnAgregar = new Button();
                _btnEditar = new Button();
                _btnEliminar = new Button();
                _btnLimpiar = new Button();
                _btnAjusteStock = new Button();
                _txtBuscador = new TextBox();
                btnBuscar = new Button();

                _dgvProductos = new DataGridView();

                _pnlFooter = new Panel();
                _lblTotalFooterValor = new Label();

                // 3. Configuración de Propiedades y Estilos
                // --- Header ---
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 92;
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;
                _pnlHeader.Padding = new Padding(20);

                _lblTitulo.Text = "Productos e Inventario";
                _lblTitulo.Font = RecursosUI.FuenteTituloModulo;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(20, 14);

                _lblSubtitulo.Text = "Administre el catálogo ABM de mercadería y ajustes manuales por mermas o roturas [F6]";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(22, 52);

                _pnlTotalHeader.Size = new Size(190, 62);
                _pnlTotalHeader.Location = new Point(this.ClientSize.Width - 210, 15);
                _pnlTotalHeader.BackColor = RecursosUI.ColorFondoVentana;
                _pnlTotalHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                _lblTotalHeaderTitulo.Text = "TOTAL DE PRODUCTOS";
                _lblTotalHeaderTitulo.Font = RecursosUI.FuentePequenaNegrita;
                _lblTotalHeaderTitulo.ForeColor = RecursosUI.ColorTextoMuted;
                _lblTotalHeaderTitulo.AutoSize = true;
                _lblTotalHeaderTitulo.Location = new Point(12, 8);

                _lblTotalHeaderValor.Text = "0";
                _lblTotalHeaderValor.Font = RecursosUI.FuenteTituloApp;
                _lblTotalHeaderValor.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblTotalHeaderValor.AutoSize = true;
                _lblTotalHeaderValor.Location = new Point(12, 26);

                // --- Toolbar ---
                _pnlToolbar.Dock = DockStyle.Top;
                _pnlToolbar.Size = new Size(this.ClientSize.Width, 60);
                _pnlToolbar.BackColor = RecursosUI.ColorFondoVentana;
                _pnlToolbar.Padding = new Padding(20, 10, 20, 10);

                _btnAgregar = CrearBotonFlat("+ Agregar (F4)", RecursosUI.ColorPrimario, new Point(20, 12), new Size(125, 36), 0);
                _btnEditar = CrearBotonFlat("Editar (F2)", RecursosUI.ColorNeutro, new Point(155, 12), new Size(110, 36), 1);
                _btnEliminar = CrearBotonFlat("Eliminar (Supr)", RecursosUI.ColorPeligro, new Point(275, 12), new Size(125, 36), 2);
                _btnLimpiar = CrearBotonFlat("Refrescar", RecursosUI.ColorNeutro, new Point(410, 12), new Size(100, 36), 3);
                _btnAjusteStock = CrearBotonFlat("F6 Ajuste Stock", RecursosUI.ColorMovimientos, new Point(520, 12), new Size(145, 36), 4);

                _txtBuscador.Font = RecursosUI.FuenteInput;
                _txtBuscador.Size = new Size(250, 32);
                _txtBuscador.Location = new Point(_pnlToolbar.Width - 375, 14);
                _txtBuscador.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                _txtBuscador.Text = TextoPlaceholder;
                _txtBuscador.ForeColor = RecursosUI.ColorTextoMuted;
                _txtBuscador.BorderStyle = BorderStyle.FixedSingle;
                _txtBuscador.TabIndex = 5;

                btnBuscar = CrearBotonFlat("Buscar", RecursosUI.ColorPrimario, new Point(_pnlToolbar.Width - 115, 12), new Size(95, 36), 6);
                btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                // --- Grilla de Productos ---
                _dgvProductos.Dock = DockStyle.Fill;
                _dgvProductos.BackgroundColor = RecursosUI.ColorSuperficie;
                _dgvProductos.BorderStyle = BorderStyle.None;
                _dgvProductos.EnableHeadersVisualStyles = false;
                _dgvProductos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                _dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                _dgvProductos.GridColor = RecursosUI.ColorSeparador;
                _dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvProductos.ColumnHeadersDefaultCellStyle.Font = RecursosUI.FuenteGrillaHeader;
                _dgvProductos.ColumnHeadersHeight = 44;
                _dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                _dgvProductos.DefaultCellStyle.Font = RecursosUI.FuenteEstandar;
                _dgvProductos.DefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvProductos.DefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvProductos.DefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvProductos.DefaultCellStyle.Padding = new Padding(5);
                _dgvProductos.RowTemplate.Height = 36;
                _dgvProductos.AllowUserToAddRows = false;
                _dgvProductos.AllowUserToDeleteRows = false;
                _dgvProductos.ReadOnly = true;
                _dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _dgvProductos.MultiSelect = false;
                _dgvProductos.AllowUserToResizeColumns = false;
                _dgvProductos.AllowUserToResizeRows = false;
                _dgvProductos.RowHeadersVisible = false;
                _dgvProductos.TabIndex = 7;

                _dgvProductos.Columns.Add("Id", "ID");
                _dgvProductos.Columns.Add("Nombre", "Nombre del Producto");
                _dgvProductos.Columns.Add("Codigo", "Código de Barras / SKU");
                _dgvProductos.Columns.Add("Costo", "Precio Costo");
                _dgvProductos.Columns.Add("Venta", "Precio Venta");
                _dgvProductos.Columns.Add("Stock", "Stock Actual");
                _dgvProductos.Columns.Add("Fraccionado", "Fraccionado");

                _dgvProductos.Columns["Id"]!.Visible = false;
                _dgvProductos.Columns["Nombre"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                _dgvProductos.Columns["Codigo"]!.Width = 170;
                _dgvProductos.Columns["Costo"]!.Width = 135;
                _dgvProductos.Columns["Venta"]!.Width = 135;
                _dgvProductos.Columns["Stock"]!.Width = 130;
                _dgvProductos.Columns["Fraccionado"]!.Width = 110;

                _dgvProductos.Columns["Costo"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvProductos.Columns["Venta"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvProductos.Columns["Stock"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvProductos.Columns["Fraccionado"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                foreach (DataGridViewColumn columna in _dgvProductos.Columns)
                {
                    columna.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // --- Footer ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 40;
                _pnlFooter.BackColor = RecursosUI.ColorBarraEstado;

                _lblTotalFooterValor.Text = "[F4] Nuevo Producto   |   [F2] Editar   |   [F6] Ajuste Manual de Stock   |   [SUPR] Eliminar   |   [ESC] Volver al Menú Principal";
                _lblTotalFooterValor.Font = RecursosUI.FuentePequenaNegrita;
                _lblTotalFooterValor.ForeColor = RecursosUI.ColorTextoClaro;
                _lblTotalFooterValor.AutoSize = true;
                _lblTotalFooterValor.Location = new Point(20, 11);

                // 5. Suscripción a Eventos
                _txtBuscador.Enter += (s, e) =>
                {
                    if (_txtBuscador.Text == TextoPlaceholder)
                    {
                        _txtBuscador.Text = string.Empty;
                        _txtBuscador.ForeColor = RecursosUI.ColorTextoPrincipal;
                    }
                };

                _txtBuscador.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(_txtBuscador.Text))
                    {
                        _txtBuscador.Text = TextoPlaceholder;
                        _txtBuscador.ForeColor = RecursosUI.ColorTextoMuted;
                    }
                };

                _txtBuscador.TextChanged += (s, e) => EjecutarFiltroBusqueda();
                _txtBuscador.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        EjecutarFiltroBusqueda();
                        if (_dgvProductos.Rows.Count > 0)
                        {
                            _dgvProductos.Focus();
                        }
                    }
                    else if (e.KeyCode == Keys.Down && _dgvProductos.Rows.Count > 0)
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        _dgvProductos.Focus();
                    }
                };

                btnBuscar.Click += (s, e) => EjecutarFiltroBusqueda();
                _btnAgregar.Click += (s, e) => AbrirAltaProducto();
                _btnEditar.Click += (s, e) => AbrirEdicionProducto();
                _btnEliminar.Click += (s, e) => EliminarProductoSeleccionado();
                _btnLimpiar.Click += (s, e) =>
                {
                    _txtBuscador.Text = TextoPlaceholder;
                    _txtBuscador.ForeColor = RecursosUI.ColorTextoMuted;
                    CargarGrillaProductos();
                };
                _btnAjusteStock.Click += (s, e) => AbrirAjusteStockModal();
                _dgvProductos.CellDoubleClick += (s, e) => AbrirEdicionProducto();
                _dgvProductos.KeyDown += DgvProductos_KeyDown;

                this.Load += (s, e) =>
                {
                    CargarGrillaProductos();
                    this.ActiveControl = _dgvProductos;
                };

                // 7. Agregado de Controles
                _pnlTotalHeader.Controls.Add(_lblTotalHeaderTitulo);
                _pnlTotalHeader.Controls.Add(_lblTotalHeaderValor);
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_pnlTotalHeader);

                _pnlToolbar.Controls.Add(_btnAgregar);
                _pnlToolbar.Controls.Add(_btnEditar);
                _pnlToolbar.Controls.Add(_btnEliminar);
                _pnlToolbar.Controls.Add(_btnLimpiar);
                _pnlToolbar.Controls.Add(_btnAjusteStock);
                _pnlToolbar.Controls.Add(_txtBuscador);
                _pnlToolbar.Controls.Add(btnBuscar);

                _pnlFooter.Controls.Add(_lblTotalFooterValor);

                this.Controls.Add(_dgvProductos);
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

        private static Button CrearBotonFlat(string texto, Color fondo, Point ubicacion, Size tamano, int tabIndex)
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

        private void EjecutarFiltroBusqueda()
        {
            string filtro = _txtBuscador.Text == TextoPlaceholder ? string.Empty : _txtBuscador.Text;
            RenderizarGrilla(filtro);
        }

        /// <summary>
        /// Intercepta la tecla Enter en el DataGridView del catálogo para anular el salto de fila
        /// y abrir inmediatamente el modal de edición con el ID del producto seleccionado.
        /// </summary>
        private void DgvProductos_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (_dgvProductos.SelectedRows.Count > 0)
                {
                    int idProducto = Convert.ToInt32(_dgvProductos.SelectedRows[0].Cells["Id"].Value);
                    AbrirEdicionProductoPorId(idProducto);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return true;

                case Keys.F6:
                    AbrirAjusteStockModal();
                    return true;

                case Keys.F4:
                    AbrirAltaProducto();
                    return true;

                case Keys.F2:
                    AbrirEdicionProducto();
                    return true;

                case Keys.Delete:
                    if (!_txtBuscador.Focused)
                    {
                        EliminarProductoSeleccionado();
                        return true;
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void CargarGrillaProductos()
        {
            try
            {
                _catalogoProductos = _productoNegocio.ObtenerCatalogoActivo();
                string filtro = _txtBuscador.Text == TextoPlaceholder ? string.Empty : _txtBuscador.Text;
                RenderizarGrilla(filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error cargando productos: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderizarGrilla(string filtro)
        {
            _dgvProductos.Rows.Clear();
            string termino = filtro?.Trim() ?? string.Empty;

            var lista = string.IsNullOrWhiteSpace(termino)
                ? _catalogoProductos
                : _catalogoProductos.Where(p =>
                    (p.Nombre != null && p.Nombre.IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.CodigoBarras != null && p.CodigoBarras.IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.CodigoInterno != null && p.CodigoInterno.IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();

            foreach (var p in lista)
            {
                int rowIndex = _dgvProductos.Rows.Add(
                    p.Id,
                    p.Nombre,
                    !string.IsNullOrWhiteSpace(p.CodigoBarras) ? p.CodigoBarras : p.CodigoInterno,
                    $"$ {p.PrecioCosto:N2}",
                    $"$ {p.PrecioVenta:N2}",
                    p.EsFraccionado ? $"{p.StockActual:0.000} Kg" : p.StockActual.ToString("0.##"),
                    p.EsFraccionado ? "Sí" : "No"
                );

                if (p.StockActual < 0)
                {
                    _dgvProductos.Rows[rowIndex].DefaultCellStyle.BackColor = RecursosUI.ColorAlertaStockFondo;
                    _dgvProductos.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = RecursosUI.ColorAlertaStockSel;
                }
            }

            _lblTotalHeaderValor.Text = lista.Count.ToString();
        }

        private Producto? ObtenerProductoSeleccionado()
        {
            if (_dgvProductos.SelectedRows.Count == 0) return null;
            int id = Convert.ToInt32(_dgvProductos.SelectedRows[0].Cells["Id"].Value);
            return _catalogoProductos.FirstOrDefault(p => p.Id == id);
        }

        private void AbrirAltaProducto()
        {
            using (var frmNuevo = new FrmNuevoProducto())
            {
                if (frmNuevo.ShowDialog(this) == DialogResult.OK)
                {
                    CargarGrillaProductos();
                }
            }
        }

        private void AbrirEdicionProducto()
        {
            var seleccionado = ObtenerProductoSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show(this, "Seleccione un producto de la grilla para editar.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AbrirEdicionProductoPorId(seleccionado.Id);
        }

        private void AbrirEdicionProductoPorId(int idProducto)
        {
            Producto? producto = _catalogoProductos.FirstOrDefault(p => p.Id == idProducto)
                ?? _productoNegocio.ObtenerProductoPorId(idProducto);

            if (producto == null) return;

            using (var frmEditar = new FrmNuevoProducto(producto))
            {
                if (frmEditar.ShowDialog(this) == DialogResult.OK)
                {
                    CargarGrillaProductos();
                }
            }
        }

        private void EliminarProductoSeleccionado()
        {
            var seleccionado = ObtenerProductoSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show(this, "Seleccione un producto de la grilla para dar de baja.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                this,
                $"¿Está seguro de que desea eliminar '{seleccionado.Nombre}' del catálogo activo?",
                "Confirmar Baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (_productoNegocio.EliminarProducto(seleccionado.Id))
                    {
                        CargarGrillaProductos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error al eliminar el producto: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AbrirAjusteStockModal()
        {
            var seleccionado = ObtenerProductoSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show(this, "Por favor, seleccione un producto de la grilla para ajustar su stock.",
                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var modal = new FormAjusteStockModal(
                seleccionado.Id,
                seleccionado.Nombre,
                seleccionado.StockActual,
                seleccionado.EsFraccionado))
            {
                if (modal.ShowDialog(this) == DialogResult.OK)
                {
                    CargarGrillaProductos();
                }
            }
        }
    }
}
