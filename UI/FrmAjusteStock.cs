using Entidades;
using Servicios;
using System.Globalization;

namespace UI
{
    public partial class FrmAjusteStock : Form
    {
        private readonly ProductoServicio _productoServicio = new();
        private Producto? _productoSeleccionado;

        public FrmAjusteStock(Producto? producto = null)
        {
            InitializeComponent();

            txtNuevoStock.TextChanged += (s, e) => CalcularDiferencia();
            btnBuscarProducto.Click += (s, e) => AbrirSeleccionProducto();

            btnGuardar.Click += async (s, e) => await GuardarAjusteAsync();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            if (producto != null)
            {
                CargarProducto(producto);
            }
        }

        public void CargarProducto(Producto p)
        {
            _productoSeleccionado = p;
            txtProducto.Text = $"[{p.Codigo ?? "S/C"}] {p.Nombre}";
            lblStockActual.Text = $"Stock Actual en Sistema: {p.StockActual:N3} {p.Unidad}";
            txtNuevoStock.Text = p.StockActual.ToString("N3", CultureInfo.InvariantCulture);
            CalcularDiferencia();
            txtNuevoStock.Focus();
            txtNuevoStock.SelectAll();
        }

        private void AbrirSeleccionProducto()
        {
            using var frmProductos = new FrmProductos(modoSeleccion: true);
            if (frmProductos.ShowDialog(this) == DialogResult.OK && frmProductos.ProductoSeleccionado != null)
            {
                CargarProducto(frmProductos.ProductoSeleccionado);
            }
        }

        private void CalcularDiferencia()
        {
            if (_productoSeleccionado == null)
            {
                lblDiferencia.Text = "Diferencia: -";
                lblDiferencia.ForeColor = Color.Black;
                return;
            }

            if (decimal.TryParse(txtNuevoStock.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var nuevoStock))
            {
                var delta = nuevoStock - _productoSeleccionado.StockActual;
                if (delta == 0)
                {
                    lblDiferencia.Text = "Diferencia: 0 (Sin cambios)";
                    lblDiferencia.ForeColor = Color.Black;
                }
                else if (delta > 0)
                {
                    lblDiferencia.Text = $"Diferencia: +{delta:N3} (Ingreso)";
                    lblDiferencia.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lblDiferencia.Text = $"Diferencia: {delta:N3} (Merma/Baja)";
                    lblDiferencia.ForeColor = Color.Crimson;
                }
            }
            else
            {
                lblDiferencia.Text = "Diferencia: -";
                lblDiferencia.ForeColor = Color.Black;
            }
        }

        private async Task GuardarAjusteAsync()
        {
            if (_productoSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un producto primero haciendo clic en 'Buscar'.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AbrirSeleccionProducto();
                return;
            }

            if (!decimal.TryParse(txtNuevoStock.Text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var nuevoStock) || nuevoStock < 0)
            {
                MessageBox.Show("Ingrese un nuevo stock válido mayor o igual a cero.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevoStock.Focus();
                return;
            }

            var motivo = txtMotivo.Text.Trim();
            if (string.IsNullOrWhiteSpace(motivo))
            {
                MessageBox.Show("El motivo del ajuste es obligatorio.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return;
            }

            var delta = nuevoStock - _productoSeleccionado.StockActual;
            if (delta == 0)
            {
                MessageBox.Show("El nuevo stock ingresado es idéntico al actual. No se aplicaron modificaciones.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Confirma ajustar el stock de '{_productoSeleccionado.Nombre}' de {_productoSeleccionado.StockActual:N3} a {nuevoStock:N3}?\n\nMotivo: {motivo}",
                "Confirmar Ajuste", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                await _productoServicio.AjustarStockAsync(_productoSeleccionado.Id, nuevoStock, motivo);
                MessageBox.Show("Stock ajustado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ajustar stock: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

