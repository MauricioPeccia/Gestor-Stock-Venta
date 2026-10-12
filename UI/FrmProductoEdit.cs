using Entidades;
using Servicios;
using System.Globalization;

namespace UI
{
    public partial class FrmProductoEdit : Form
    {
        private readonly ProductoServicio _productoServicio = new();
        private readonly Producto? _productoExistente;

        public FrmProductoEdit(Producto? producto = null)
        {
            InitializeComponent();
            _productoExistente = producto;

            cboUnidad.SelectedIndex = 0; // UN

            txtPrecioCosto.TextChanged += (s, e) => CalcularMargen();
            txtPrecioVenta.TextChanged += (s, e) => CalcularMargen();

            if (_productoExistente != null)
            {
                lblTitulo.Text = "Editar Producto";
                Text = "Editar Producto";
                txtCodigo.Text = _productoExistente.Codigo ?? "";
                txtNombre.Text = _productoExistente.Nombre;
                cboUnidad.SelectedItem = _productoExistente.Unidad;
                txtPrecioCosto.Text = _productoExistente.PrecioCosto.ToString("N2", CultureInfo.InvariantCulture);
                txtPrecioVenta.Text = _productoExistente.PrecioVenta.ToString("N2", CultureInfo.InvariantCulture);
                txtStockMinimo.Text = _productoExistente.StockMinimo.ToString("N2", CultureInfo.InvariantCulture);

                // No se edita stock actual acá (solo con ajustes o movimientos)
                lblStockInicial.Visible = false;
                txtStockInicial.Visible = false;
            }
            else
            {
                lblTitulo.Text = "Nuevo Producto";
                Text = "Nuevo Producto";
                lblStockInicial.Visible = true;
                txtStockInicial.Visible = true;
            }

            CalcularMargen();

            btnGuardar.Click += async (s, e) => await GuardarAsync();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            KeyDown += FrmProductoEdit_KeyDown;
        }

        private void CalcularMargen()
        {
            decimal costo = 0;
            decimal venta = 0;

            decimal.TryParse(txtPrecioCosto.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out costo);
            decimal.TryParse(txtPrecioVenta.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out venta);

            if (costo > 0 && venta >= costo)
            {
                var ganancia = venta - costo;
                var porcentaje = (ganancia / costo) * 100m;
                lblMargen.Text = $"Margen: +{porcentaje:N1}% (+${ganancia:N2})";
                lblMargen.ForeColor = Color.ForestGreen;
            }
            else if (venta < costo && venta > 0)
            {
                var perdida = costo - venta;
                lblMargen.Text = $"⚠️ Venta por debajo del costo (-${perdida:N2})";
                lblMargen.ForeColor = Color.Crimson;
            }
            else
            {
                lblMargen.Text = "Margen: 0% ($ 0,00)";
                lblMargen.ForeColor = Color.Gray;
            }
        }

        private async Task GuardarAsync()
        {
            var nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecioCosto.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var costo) || costo < 0)
            {
                MessageBox.Show("Ingrese un precio de costo válido mayor o igual a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCosto.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var venta) || venta < 0)
            {
                MessageBox.Show("Ingrese un precio de venta válido mayor o igual a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioVenta.Focus();
                return;
            }

            if (!decimal.TryParse(txtStockMinimo.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var stockMin) || stockMin < 0)
            {
                stockMin = 0;
            }

            decimal stockInicial = 0;
            if (_productoExistente == null)
            {
                if (!decimal.TryParse(txtStockInicial.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out stockInicial) || stockInicial < 0)
                {
                    stockInicial = 0;
                }
            }

            var codigo = string.IsNullOrWhiteSpace(txtCodigo.Text) ? null : txtCodigo.Text.Trim();
            var unidad = cboUnidad.SelectedItem?.ToString() ?? "UN";

            try
            {
                if (_productoExistente == null)
                {
                    var nuevo = new Producto
                    {
                        Codigo = codigo,
                        Nombre = nombre,
                        Unidad = unidad,
                        PrecioCosto = costo,
                        PrecioVenta = venta,
                        StockMinimo = stockMin
                    };

                    await _productoServicio.CrearAsync(nuevo, stockInicial);
                    MessageBox.Show("Producto creado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _productoExistente.Codigo = codigo;
                    _productoExistente.Nombre = nombre;
                    _productoExistente.Unidad = unidad;
                    _productoExistente.PrecioCosto = costo;
                    _productoExistente.PrecioVenta = venta;
                    _productoExistente.StockMinimo = stockMin;

                    await _productoServicio.ActualizarAsync(_productoExistente);
                    MessageBox.Show("Producto actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmProductoEdit_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }

    // Alias para compatibilidad con código existente
    public class FrmProductoEditar : FrmProductoEdit
    {
        public FrmProductoEditar(Producto? producto = null) : base(producto) { }
    }
}

