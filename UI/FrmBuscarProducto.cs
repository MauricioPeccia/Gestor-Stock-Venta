using Entidades;
using Servicios;
using System.Globalization;

namespace UI
{
    public partial class FrmBuscarProducto : Form
    {
        private readonly ProductoServicio _productoServicio = new();
        private List<Producto> _productos = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");

        public Producto? ProductoSeleccionado { get; private set; }

        public FrmBuscarProducto(string filtroInicial = "")
        {
            InitializeComponent();

            ConfigurarGrilla();

            txtBuscar.Text = filtroInicial;
            txtBuscar.TextChanged += async (s, e) => await BuscarAsync(txtBuscar.Text.Trim());
            txtBuscar.KeyDown += TxtBuscar_KeyDown;

            dgvProductos.CellDoubleClick += (s, e) => SeleccionarActual();
            dgvProductos.KeyDown += DgvProductos_KeyDown;

            btnSeleccionar.Click += (s, e) => SeleccionarActual();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            KeyDown += FrmBuscarProducto_KeyDown;

            Load += async (s, e) =>
            {
                txtBuscar.Select(txtBuscar.Text.Length, 0);
                await BuscarAsync(txtBuscar.Text.Trim());
            };
        }

        private void ConfigurarGrilla()
        {
            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.Columns.Clear();

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Visible = false
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Codigo",
                HeaderText = "Código",
                Width = 130
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Unidad",
                HeaderText = "Unidad",
                Width = 70
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioVenta",
                HeaderText = "Precio",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockActual",
                HeaderText = "Stock",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async Task BuscarAsync(string texto)
        {
            try
            {
                _productos = await _productoServicio.BuscarAsync(texto);
                dgvProductos.DataSource = _productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SeleccionarActual()
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is Producto p)
            {
                ProductoSeleccionado = p;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && dgvProductos.Rows.Count > 0)
            {
                e.SuppressKeyPress = true;
                dgvProductos.Focus();
                dgvProductos.Rows[0].Selected = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (dgvProductos.Rows.Count > 0)
                {
                    SeleccionarActual();
                }
            }
        }

        private void DgvProductos_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SeleccionarActual();
            }
        }

        private void FrmBuscarProducto_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}

