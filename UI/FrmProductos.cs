using Entidades;
using Servicios;
using System.Globalization;

namespace UI
{
    public partial class FrmProductos : Form
    {
        private readonly ProductoServicio _productoServicio = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");
        private readonly bool _modoSeleccion;
        private List<Producto> _listaProductos = new();

        public Producto? ProductoSeleccionado { get; private set; }

        public FrmProductos(bool modoSeleccion = false)
        {
            InitializeComponent();
            _modoSeleccion = modoSeleccion;

            if (_modoSeleccion)
            {
                Text = "Seleccionar Producto para Ajuste de Stock (Doble Clic para Elegir)";
                btnNuevo.Visible = false;
                btnEditar.Visible = false;
                btnEliminar.Visible = false;
                btnAumentoPrecios.Visible = false;
                btnAjusteStock.Text = "Seleccionar (Enter)";
                btnAjusteStock.BackColor = Color.ForestGreen;
                btnAjusteStock.Click += (s, e) => SeleccionarProductoActual();
            }
            else
            {
                btnNuevo.Click += async (s, e) => await NuevoProductoAsync();
                btnEditar.Click += async (s, e) => await EditarProductoAsync();
                btnEliminar.Click += async (s, e) => await EliminarProductoAsync();
                btnAumentoPrecios.Click += async (s, e) => await AumentarPreciosAsync();
                btnAjusteStock.Click += async (s, e) => await AjustarStockAsync();
            }

            ConfigurarGrilla();

            txtBuscar.TextChanged += async (s, e) => await FiltrarAsync();
            chkSoloBajoStock.CheckedChanged += async (s, e) => await FiltrarAsync();

            btnCerrar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            dgvProductos.CellDoubleClick += async (s, e) =>
            {
                if (_modoSeleccion)
                {
                    SeleccionarProductoActual();
                }
                else
                {
                    // Al hacer doble clic en modo normal, lleva directo al ajuste de stock
                    await AjustarStockAsync();
                }
            };

            dgvProductos.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    if (_modoSeleccion)
                    {
                        SeleccionarProductoActual();
                    }
                    else
                    {
                        await AjustarStockAsync();
                    }
                }
            };

            dgvProductos.CellFormatting += DgvProductos_CellFormatting;

            Load += async (s, e) =>
            {
                await CargarProductosAsync();
                txtBuscar.Focus();
            };
        }

        private void SeleccionarProductoActual()
        {
            var p = ObtenerProductoSeleccionado();
            if (p != null)
            {
                ProductoSeleccionado = p;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ConfigurarGrilla()
        {
            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.Columns.Clear();

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.Id),
                HeaderText = "ID",
                Visible = false
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.Codigo),
                HeaderText = "Código",
                Width = 130
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.Nombre),
                HeaderText = "Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.Unidad),
                HeaderText = "Unidad",
                Width = 70
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.PrecioCosto),
                HeaderText = "Costo",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.PrecioVenta),
                HeaderText = "Venta",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.StockActual),
                HeaderText = "Stock Actual",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Producto.StockMinimo),
                HeaderText = "Stock Mínimo",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                await FiltrarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task FiltrarAsync()
        {
            try
            {
                if (chkSoloBajoStock.Checked)
                {
                    _listaProductos = await _productoServicio.BajoStockMinimoAsync();
                }
                else
                {
                    _listaProductos = await _productoServicio.BuscarAsync(txtBuscar.Text.Trim());
                }

                dgvProductos.DataSource = _listaProductos;

                var bajoStock = _listaProductos.Count(p => p.StockActual <= p.StockMinimo);
                lblInfo.Text = $"Mostrando {_listaProductos.Count} productos. (Productos bajo stock mínimo: {bajoStock})";
            }
            catch (Exception ex)
            {
                lblInfo.Text = $"Error al buscar: {ex.Message}";
            }
        }

        private void DgvProductos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProductos.Rows.Count) return;

            if (dgvProductos.Rows[e.RowIndex].DataBoundItem is Producto p)
            {
                if (p.StockActual <= p.StockMinimo)
                {
                    dgvProductos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.MistyRose;
                    dgvProductos.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.IndianRed;
                }
                else
                {
                    dgvProductos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvProductos.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                }
            }
        }

        private Producto? ObtenerProductoSeleccionado()
        {
            return dgvProductos.CurrentRow?.DataBoundItem as Producto;
        }

        private async Task NuevoProductoAsync()
        {
            using var modal = new FrmProductoEdit();
            if (modal.ShowDialog(this) == DialogResult.OK)
            {
                await FiltrarAsync();
            }
        }

        private async Task EditarProductoAsync()
        {
            var p = ObtenerProductoSeleccionado();
            if (p == null)
            {
                MessageBox.Show("Seleccione un producto de la lista para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var modal = new FrmProductoEdit(p);
            if (modal.ShowDialog(this) == DialogResult.OK)
            {
                await FiltrarAsync();
            }
        }

        private async Task EliminarProductoAsync()
        {
            var p = ObtenerProductoSeleccionado();
            if (p == null)
            {
                MessageBox.Show("Seleccione un producto para dar de baja.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Está seguro de que desea dar de baja el producto '{p.Nombre}'?\n\nEl producto se desactivará y dejará de figurar en ventas y catálogo.",
                "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                await _productoServicio.DesactivarAsync(p.Id);
                MessageBox.Show("Producto dado de baja con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await FiltrarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desactivar el producto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task AumentarPreciosAsync()
        {
            var prompt = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingrese el porcentaje de aumento de precios (ejemplo: 10 para 10%):",
                "Aumento Masivo de Precios", "10");

            if (string.IsNullOrWhiteSpace(prompt)) return;

            if (!decimal.TryParse(prompt.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var porcentaje) || porcentaje == 0)
            {
                MessageBox.Show("Ingrese un porcentaje válido distinto de cero.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Confirma el aumento de precios del {porcentaje:N2}% en todos los productos activos?",
                "Confirmar Aumento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var filas = await _productoServicio.AumentarPreciosAsync(porcentaje, null, 10);
                MessageBox.Show($"Se actualizaron los precios de venta de {filas} productos.",
                    "Precios Actualizados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await FiltrarAsync();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aumentar precios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task AjustarStockAsync()
        {
            var p = ObtenerProductoSeleccionado();
            if (p == null)
            {
                MessageBox.Show("Seleccione un producto para ajustar su stock.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var frmAjuste = new FrmAjusteStock(p);
            if (frmAjuste.ShowDialog(this) == DialogResult.OK)
            {
                await FiltrarAsync();
            }
        }
    }
}

