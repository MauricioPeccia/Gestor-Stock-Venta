using Entidades;
using Servicios;
using System.ComponentModel;
using System.Globalization;

namespace UI
{
    public partial class FrmCompras : Form
    {
        private readonly CompraServicio _compraServicio = new();
        private readonly ProveedorServicio _proveedorServicio = new();
        private readonly CajaServicio _cajaServicio = new();
        private readonly BindingList<ItemCompraViewModel> _items = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");
        private Producto? _productoSeleccionado;
        private List<Proveedor> _proveedores = new();

        public FrmCompras()
        {
            InitializeComponent();

            ConfigurarGrilla();

            btnBuscarProd.Click += (s, e) => BuscarProducto();
            btnAgregarItem.Click += (s, e) => AgregarItem();
            btnQuitarItem.Click += (s, e) => QuitarItem();
            btnRegistrarCompra.Click += async (s, e) => await RegistrarCompraAsync();
            btnCancelar.Click += (s, e) => Close();

            txtCostoUnitario.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    AgregarItem();
                }
            };

            Load += async (s, e) => await InicializarAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvItems.AutoGenerateColumns = false;
            dgvItems.Columns.Clear();

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCompraViewModel.ProductoId),
                HeaderText = "ID",
                Visible = false
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCompraViewModel.Codigo),
                HeaderText = "Código",
                Width = 140
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCompraViewModel.Nombre),
                HeaderText = "Producto / Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCompraViewModel.Cantidad),
                HeaderText = "Cantidad",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCompraViewModel.CostoUnitario),
                HeaderText = "Costo Unitario",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCompraViewModel.Subtotal),
                HeaderText = "Subtotal",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.DataSource = _items;
            ActualizarTotal();
        }

        private async Task InicializarAsync()
        {
            try
            {
                _proveedores = await _proveedorServicio.ListarAsync();
                var listaConVacio = new List<Proveedor>
                {
                    new Proveedor { Id = 0, Nombre = "-- Sin Proveedor / Compra Directa --" }
                };
                listaConVacio.AddRange(_proveedores);

                cboProveedor.DataSource = listaConVacio;
                cboProveedor.DisplayMember = nameof(Proveedor.Nombre);
                cboProveedor.ValueMember = nameof(Proveedor.Id);
                cboProveedor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarProducto()
        {
            using var frmProd = new FrmProductos(modoSeleccion: true);
            if (frmProd.ShowDialog(this) == DialogResult.OK && frmProd.ProductoSeleccionado != null)
            {
                _productoSeleccionado = frmProd.ProductoSeleccionado;
                txtProducto.Text = $"[{_productoSeleccionado.Codigo ?? "S/C"}] {_productoSeleccionado.Nombre}";
                txtCostoUnitario.Text = _productoSeleccionado.PrecioCosto.ToString("N2", CultureInfo.InvariantCulture);
                txtCantidad.Focus();
                txtCantidad.SelectAll();
            }
        }

        private void AgregarItem()
        {
            if (_productoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un producto para agregar a la compra.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BuscarProducto();
                return;
            }

            if (!decimal.TryParse(txtCantidad.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a cero.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            if (!decimal.TryParse(txtCostoUnitario.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var costo) || costo < 0)
            {
                MessageBox.Show("Ingrese un costo unitario válido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCostoUnitario.Focus();
                return;
            }

            var existente = _items.FirstOrDefault(i => i.ProductoId == _productoSeleccionado.Id);
            if (existente != null)
            {
                existente.Cantidad += cantidad;
                existente.CostoUnitario = costo; // actualiza al último costo
                var idx = _items.IndexOf(existente);
                _items.ResetItem(idx);
            }
            else
            {
                _items.Add(new ItemCompraViewModel
                {
                    ProductoId = _productoSeleccionado.Id,
                    Codigo = _productoSeleccionado.Codigo,
                    Nombre = _productoSeleccionado.Nombre,
                    Cantidad = cantidad,
                    CostoUnitario = costo
                });
            }

            ActualizarTotal();

            _productoSeleccionado = null;
            txtProducto.Clear();
            txtCantidad.Text = "1";
            txtCostoUnitario.Clear();
            btnBuscarProd.Focus();
        }

        private void QuitarItem()
        {
            if (dgvItems.CurrentRow?.DataBoundItem is ItemCompraViewModel it)
            {
                _items.Remove(it);
                ActualizarTotal();
            }
        }

        private void ActualizarTotal()
        {
            var total = _items.Sum(i => i.Subtotal);
            lblTotal.Text = total.ToString("C2", _ci);
            btnRegistrarCompra.Enabled = _items.Count > 0;
            btnQuitarItem.Enabled = _items.Count > 0;
        }

        private async Task RegistrarCompraAsync()
        {
            if (_items.Count == 0)
            {
                MessageBox.Show("No hay productos cargados en la compra.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (chkPagadaConCaja.Checked)
            {
                var caja = await _cajaServicio.ObtenerAbiertaAsync();
                if (caja == null)
                {
                    MessageBox.Show(
                        "Para pagar con la caja debe haber una jornada de caja abierta actualmente.\n\nAbra la caja o destilde la opción de pago por caja.",
                        "Caja requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            int? provId = (int?)cboProveedor.SelectedValue;
            if (provId == 0) provId = null;

            var compra = new Compra
            {
                ProveedorId = provId,
                NroComprobante = string.IsNullOrWhiteSpace(txtComprobante.Text) ? null : txtComprobante.Text.Trim(),
                Observaciones = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim(),
                PagadaConCaja = chkPagadaConCaja.Checked,
                Items = _items.Select(i => new CompraItem
                {
                    ProductoId = i.ProductoId,
                    Cantidad = i.Cantidad,
                    CostoUnitario = i.CostoUnitario
                }).ToList()
            };

            var confirm = MessageBox.Show(
                $"¿Confirma registrar la compra de {_items.Count} productos por un total de {compra.Total.ToString("C2", _ci)}?\n\nEsta acción sumará el stock de los productos y actualizará su costo.",
                "Confirmar Compra", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var id = await _compraServicio.RegistrarAsync(compra);
                MessageBox.Show($"¡Compra #{id} registrada con éxito!\nStock y costos actualizados correctamente.",
                    "Compra Registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _items.Clear();
                txtComprobante.Clear();
                txtObs.Clear();
                chkPagadaConCaja.Checked = false;
                ActualizarTotal();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la compra: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class ItemCompraViewModel
    {
        public int ProductoId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal => Math.Round(Cantidad * CostoUnitario, 2);
    }
}

