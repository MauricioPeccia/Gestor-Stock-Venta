using Entidades;
using Servicios;
using System.ComponentModel;
using System.Globalization;

namespace UI
{
    public partial class FrmVenta : Form
    {
        private readonly VentaServicio _ventaServicio = new();
        private readonly ProductoServicio _productoServicio = new();
        private readonly CajaServicio _cajaServicio = new();
        private readonly BindingList<ItemCarrito> _items = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");

        public FrmVenta()
        {
            InitializeComponent();

            ConfigurarGrilla();

            txtBusqueda.KeyDown += TxtBusqueda_KeyDown;
            btnCobrar.Click += async (s, e) => await ProcesarCobroAsync();
            btnEliminar.Click += (s, e) => QuitarItemSeleccionado();
            btnCancelar.Click += (s, e) => CancelarVenta();

            KeyDown += FrmVenta_KeyDown;
            Load += FrmVenta_Load;
        }

        private async void FrmVenta_Load(object? sender, EventArgs e)
        {
            txtBusqueda.Focus();
            await VerificarCajaAsync();
        }

        private async Task<bool> VerificarCajaAsync()
        {
            try
            {
                var caja = await _cajaServicio.ObtenerAbiertaAsync();
                if (caja == null)
                {
                    lblUltimoProducto.Text = "⚠️ ATENCIÓN: No hay una caja abierta. Debe abrir la caja para poder cobrar.";
                    lblUltimoProducto.ForeColor = Color.DarkRed;
                    return false;
                }
                else
                {
                    lblUltimoProducto.Text = $"Caja abierta ({caja.Fecha:dd/MM/yyyy}). Listo para escanear productos. (F3: Buscar en catálogo)";
                    lblUltimoProducto.ForeColor = Color.DimGray;
                    return true;
                }
            }
            catch (Exception ex)
            {
                lblUltimoProducto.Text = $"Error al verificar estado de caja: {ex.Message}";
                lblUltimoProducto.ForeColor = Color.Red;
                return false;
            }
        }

        private void ConfigurarGrilla()
        {
            dgvItems.AutoGenerateColumns = false;
            dgvItems.Columns.Clear();

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.ProductoId),
                HeaderText = "ID",
                Visible = false
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.Codigo),
                HeaderText = "Código",
                Width = 140
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.Nombre),
                HeaderText = "Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.Unidad),
                HeaderText = "Unidad",
                Width = 70
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.Cantidad),
                HeaderText = "Cantidad",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.PrecioUnitario),
                HeaderText = "Precio Unit.",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ItemCarrito.Subtotal),
                HeaderText = "Subtotal",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.DataSource = _items;
            ActualizarTotales();
        }

        private async void TxtBusqueda_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                var entrada = txtBusqueda.Text.Trim();
                if (string.IsNullOrWhiteSpace(entrada)) return;

                await ProcesarEntradaProductoAsync(entrada);
            }
        }

        private async Task ProcesarEntradaProductoAsync(string entrada)
        {
            decimal cantidad = 1;
            string termino = entrada;

            // Soporte para multiplicador: ej. 3*779123 o 0.5*queso
            var partes = entrada.Split(new[] { '*', 'x', 'X' }, 2);
            if (partes.Length == 2 &&
                decimal.TryParse(partes[0].Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var cant) &&
                cant > 0)
            {
                cantidad = cant;
                termino = partes[1].Trim();
            }

            try
            {
                // 1. Intentar buscar por código exacto (lector de barras)
                var producto = await _productoServicio.ObtenerPorCodigoAsync(termino);

                if (producto != null)
                {
                    AgregarOIncrementarProducto(producto, cantidad);
                    txtBusqueda.Clear();
                    txtBusqueda.Focus();
                    return;
                }

                // 2. Si no es un código de barras exacto, buscar por nombre/código
                var resultados = await _productoServicio.BuscarAsync(termino);

                if (resultados.Count == 1)
                {
                    AgregarOIncrementarProducto(resultados[0], cantidad);
                    txtBusqueda.Clear();
                    txtBusqueda.Focus();
                }
                else if (resultados.Count > 1)
                {
                    using var frmBuscar = new FrmBuscarProducto(termino);
                    if (frmBuscar.ShowDialog(this) == DialogResult.OK && frmBuscar.ProductoSeleccionado != null)
                    {
                        AgregarOIncrementarProducto(frmBuscar.ProductoSeleccionado, cantidad);
                    }
                    txtBusqueda.Clear();
                    txtBusqueda.Focus();
                }
                else
                {
                    MessageBox.Show($"No se encontró ningún producto con el código o nombre: '{termino}'.",
                        "Producto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBusqueda.SelectAll();
                    txtBusqueda.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar el producto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBusqueda.SelectAll();
            }
        }

        private void AgregarOIncrementarProducto(Producto prod, decimal cantidad)
        {
            if (prod.Unidad == "UN" && cantidad % 1 != 0)
            {
                MessageBox.Show("Este producto se vende por unidad (la cantidad debe ser entera).",
                    "Cantidad Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existente = _items.FirstOrDefault(i => i.ProductoId == prod.Id);
            if (existente != null)
            {
                existente.Cantidad += cantidad;
                // Refrescar grilla
                var index = _items.IndexOf(existente);
                _items.ResetItem(index);
                dgvItems.Rows[index].Selected = true;
            }
            else
            {
                var nuevo = new ItemCarrito
                {
                    ProductoId = prod.Id,
                    Codigo = prod.Codigo,
                    Nombre = prod.Nombre,
                    Unidad = prod.Unidad,
                    Cantidad = cantidad,
                    PrecioUnitario = prod.PrecioVenta
                };
                _items.Add(nuevo);
                dgvItems.Rows[dgvItems.Rows.Count - 1].Selected = true;
            }

            lblUltimoProducto.Text = $"Último agregado: {prod.Nombre} x {cantidad} ({prod.PrecioVenta.ToString("C2", _ci)})";
            lblUltimoProducto.ForeColor = Color.DarkGreen;

            ActualizarTotales();
        }

        private void QuitarItemSeleccionado()
        {
            if (dgvItems.CurrentRow?.DataBoundItem is ItemCarrito item)
            {
                _items.Remove(item);
                lblUltimoProducto.Text = $"Se quitó: {item.Nombre}";
                lblUltimoProducto.ForeColor = Color.DimGray;
                ActualizarTotales();
            }
            txtBusqueda.Focus();
        }

        private void CancelarVenta()
        {
            if (_items.Count == 0) return;

            var resp = MessageBox.Show("¿Está seguro de que desea cancelar la venta actual y limpiar la lista?",
                "Cancelar Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                _items.Clear();
                ActualizarTotales();
                lblUltimoProducto.Text = "Venta cancelada. Listo para escanear productos.";
                lblUltimoProducto.ForeColor = Color.DimGray;
                txtBusqueda.Clear();
                txtBusqueda.Focus();
            }
        }

        private void ActualizarTotales()
        {
            var subtotal = _items.Sum(i => i.Subtotal);
            lblTotal.Text = subtotal.ToString("C2", _ci);
            lblItemsCount.Text = $"Ítems: {_items.Count} | Total Unidades: {_items.Sum(i => i.Cantidad):N2}";
            btnCobrar.Enabled = _items.Count > 0;
            btnEliminar.Enabled = _items.Count > 0;
        }

        private async Task ProcesarCobroAsync()
        {
            if (_items.Count == 0)
            {
                MessageBox.Show("No hay productos cargados en la venta.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBusqueda.Focus();
                return;
            }

            // Verificar si hay caja abierta
            var caja = await _cajaServicio.ObtenerAbiertaAsync();
            if (caja == null)
            {
                var abrir = MessageBox.Show(
                    "No hay una jornada de caja abierta actualmente.\n\n¿Desea abrir la pantalla de Caja ahora para iniciar la jornada?",
                    "Caja Cerrada", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (abrir == DialogResult.Yes)
                {
                    using var frmCaja = new FrmCaja();
                    frmCaja.ShowDialog(this);
                    await VerificarCajaAsync();
                }
                return;
            }

            var subtotal = _items.Sum(i => i.Subtotal);
            using var frmCobro = new FrmCobro(subtotal);

            if (frmCobro.ShowDialog(this) == DialogResult.OK)
            {
                var venta = new Venta
                {
                    Descuento = frmCobro.Descuento,
                    Items = _items.Select(i => new VentaItem
                    {
                        ProductoId = i.ProductoId,
                        Nombre = i.Nombre,
                        Unidad = i.Unidad,
                        Cantidad = i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario
                    }).ToList(),
                    Pagos = frmCobro.Pagos
                };

                await GuardarVentaAsync(venta, frmCobro.TotalFinal, frmCobro.Vuelto);
            }
        }

        private async Task GuardarVentaAsync(Venta venta, decimal totalFinal, decimal vuelto, bool permitirSinStock = false)
        {
            try
            {
                var ventaId = await _ventaServicio.RegistrarAsync(venta, permitirSinStock);

                var mensaje = $"¡Venta #{ventaId} registrada con éxito!\n\n" +
                              $"Total: {totalFinal.ToString("C2", _ci)}\n";

                if (vuelto > 0)
                {
                    mensaje += $"Vuelto a entregar: {vuelto.ToString("C2", _ci)}\n";
                }

                MessageBox.Show(mensaje, "Venta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _items.Clear();
                ActualizarTotales();
                lblUltimoProducto.Text = $"Última venta: #{ventaId} ({totalFinal.ToString("C2", _ci)})";
                lblUltimoProducto.ForeColor = Color.DarkGreen;
                txtBusqueda.Clear();
                txtBusqueda.Focus();
            }
            catch (StockInsuficienteException ex)
            {
                var respuesta = MessageBox.Show(
                    $"{ex.Message}\n\n¿Desea registrar la venta de todos modos permitiendo stock negativo?",
                    "Stock Insuficiente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    await GuardarVentaAsync(venta, totalFinal, vuelto, permitirSinStock: true);
                }
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la venta: {ex.Message}", "Error Inesperado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirBusquedaCatalogo()
        {
            using var frmBuscar = new FrmBuscarProducto();
            if (frmBuscar.ShowDialog(this) == DialogResult.OK && frmBuscar.ProductoSeleccionado != null)
            {
                AgregarOIncrementarProducto(frmBuscar.ProductoSeleccionado, 1);
            }
            txtBusqueda.Clear();
            txtBusqueda.Focus();
        }

        private void FrmVenta_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                e.SuppressKeyPress = true;
                if (btnCobrar.Enabled)
                {
                    _ = ProcesarCobroAsync();
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                e.SuppressKeyPress = true;
                AbrirBusquedaCatalogo();
            }
            else if (e.KeyCode == Keys.Delete && !txtBusqueda.Focused)
            {
                e.SuppressKeyPress = true;
                QuitarItemSeleccionado();
            }
            else if (e.KeyCode == Keys.Escape && !txtBusqueda.Focused)
            {
                e.SuppressKeyPress = true;
                CancelarVenta();
            }
        }
    }

    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public string Unidad { get; set; } = "UN";
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Math.Round(Cantidad * PrecioUnitario, 2);
    }
}

