using Kiosco.Entidades;
using Kiosco.Servicios;

namespace Kiosco.UI;

public partial class FrmProductoEdit : Form
{
    private readonly ProductoServicio _productoServicio = new();
    private readonly CategoriaServicio _categoriaServicio = new();
    private readonly Producto? _producto;

    public FrmProductoEdit(Producto? producto = null)
    {
        InitializeComponent();
        _producto = producto;
    }

    private async void FrmProductoEdit_Load(object sender, EventArgs e)
    {
        await CargarCategoriasAsync();

        if (_producto != null)
        {
            lblTitulo.Text = "Editar Producto";
            txtCodigo.Text = _producto.Codigo ?? "";
            txtNombre.Text = _producto.Nombre;
            cboUnidad.SelectedItem = _producto.Unidad;
            numPrecioCosto.Value = _producto.PrecioCosto;
            numPrecioVenta.Value = _producto.PrecioVenta;
            numStockMinimo.Value = _producto.StockMinimo;
            numStockInicial.Value = _producto.StockActual;
            numStockInicial.Enabled = false;

            if (_producto.CategoriaId.HasValue)
            {
                cboCategoria.SelectedValue = _producto.CategoriaId.Value;
            }
            else
            {
                cboCategoria.SelectedValue = 0;
            }
        }
        else
        {
            lblTitulo.Text = "Nuevo Producto";
            cboUnidad.SelectedItem = "UN";
            cboCategoria.SelectedValue = 0;
            numStockInicial.Enabled = true;
        }

        txtNombre.Focus();
    }

    private async Task CargarCategoriasAsync()
    {
        try
        {
            var categorias = await _categoriaServicio.ObtenerTodasAsync();
            var listaCategorias = new List<CategoriaItem>
            {
                new CategoriaItem(0, "[Sin categoría]")
            };

            foreach (var cat in categorias)
            {
                listaCategorias.Add(new CategoriaItem(cat.Id, cat.Nombre));
            }

            cboCategoria.DataSource = listaCategorias;
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "Id";
        }
        catch
        {
            cboCategoria.DataSource = new List<CategoriaItem> { new CategoriaItem(0, "[Sin categoría]") };
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "Id";
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            MessageBox.Show("El nombre del producto es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNombre.Focus();
            return;
        }

        var codigo = string.IsNullOrWhiteSpace(txtCodigo.Text) ? null : txtCodigo.Text.Trim();
        var unidad = cboUnidad.SelectedItem?.ToString() ?? "UN";
        var categoriaIdVal = (int)(cboCategoria.SelectedValue ?? 0);
        int? categoriaId = categoriaIdVal > 0 ? categoriaIdVal : null;
        var precioCosto = numPrecioCosto.Value;
        var precioVenta = numPrecioVenta.Value;
        var stockMinimo = numStockMinimo.Value;

        try
        {
            btnGuardar.Enabled = false;

            if (_producto == null)
            {
                // Alta
                var nuevo = new Producto
                {
                    Codigo = codigo,
                    Nombre = nombre,
                    CategoriaId = categoriaId,
                    Unidad = unidad,
                    PrecioCosto = precioCosto,
                    PrecioVenta = precioVenta,
                    StockMinimo = stockMinimo,
                    Activo = true
                };

                await _productoServicio.CrearAsync(nuevo, numStockInicial.Value);
            }
            else
            {
                // Edición
                _producto.Codigo = codigo;
                _producto.Nombre = nombre;
                _producto.CategoriaId = categoriaId;
                _producto.Unidad = unidad;
                _producto.PrecioCosto = precioCosto;
                _producto.PrecioVenta = precioVenta;
                _producto.StockMinimo = stockMinimo;

                await _productoServicio.ActualizarAsync(_producto);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnGuardar.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error inesperado al guardar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnGuardar.Enabled = true;
        }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private record CategoriaItem(int Id, string Nombre);
}

