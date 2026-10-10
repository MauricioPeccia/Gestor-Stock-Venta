using Kiosco.Entidades;
using Kiosco.Servicios;

namespace Kiosco.UI;

public partial class FrmProductos : Form
{
    private readonly ProductoServicio _productoServicio = new();
    private readonly CategoriaServicio _categoriaServicio = new();
    private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 300 };

    private Dictionary<int, string> _categorias = new();
    private List<Producto> _productos = new();

    public FrmProductos()
    {
        InitializeComponent();
    }

    private async void FrmProductos_Load(object sender, EventArgs e)
    {
        dgvProductos.AutoGenerateColumns = false;

        _searchTimer.Tick += async (s, ev) =>
        {
            _searchTimer.Stop();
            await CargarProductosAsync(txtBuscar.Text.Trim());
        };

        await CargarCategoriasAsync();
        await CargarProductosAsync("");
    }

    private async Task CargarCategoriasAsync()
    {
        try
        {
            var cats = await _categoriaServicio.ObtenerTodasAsync();
            _categorias = cats.ToDictionary(c => c.Id, c => c.Nombre);
        }
        catch
        {
            _categorias = new Dictionary<int, string>();
        }
    }

    private async Task CargarProductosAsync(string texto)
    {
        try
        {
            _productos = await _productoServicio.BuscarAsync(texto);
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = _productos;

            lblTotalProductos.Text = string.IsNullOrWhiteSpace(texto)
                ? $"Mostrando {_productos.Count} producto(s) en catálogo."
                : $"Mostrando {_productos.Count} resultado(s) para '{texto}'.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void txtBuscar_TextChanged(object sender, EventArgs e)
    {
        _searchTimer.Stop();
        _searchTimer.Start();
    }

    private async void txtBuscar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            _searchTimer.Stop();
            await CargarProductosAsync(txtBuscar.Text.Trim());
        }
    }

    private void dgvProductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _productos.Count) return;

        var prod = _productos[e.RowIndex];

        // Formato para Categoría
        if (dgvProductos.Columns[e.ColumnIndex].Name == colCategoria.Name)
        {
            if (prod.CategoriaId.HasValue && _categorias.TryGetValue(prod.CategoriaId.Value, out var catNombre))
            {
                e.Value = catNombre;
            }
            else
            {
                e.Value = "-";
            }
            e.FormattingApplied = true;
        }
        // Formato para Estado
        else if (dgvProductos.Columns[e.ColumnIndex].Name == colEstado.Name)
        {
            if (prod.Activo)
            {
                e.Value = "Activo";
                if (e.CellStyle != null) e.CellStyle.ForeColor = Color.DarkGreen;
            }
            else
            {
                e.Value = "Inactivo";
                if (e.CellStyle != null) e.CellStyle.ForeColor = Color.DarkGray;
            }
            e.FormattingApplied = true;
        }
        // Resaltar stock bajo o negativo
        else if (dgvProductos.Columns[e.ColumnIndex].Name == colStockActual.Name)
        {
            if (prod.StockActual <= prod.StockMinimo && e.CellStyle != null)
            {
                e.CellStyle.ForeColor = Color.DarkRed;
                e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
            }
        }
    }

    private async void btnNuevo_Click(object sender, EventArgs e)
    {
        using var edit = new FrmProductoEdit();
        if (edit.ShowDialog(this) == DialogResult.OK)
        {
            await CargarProductosAsync(txtBuscar.Text.Trim());
        }
    }

    private async void btnEditar_Click(object sender, EventArgs e)
    {
        if (dgvProductos.CurrentRow?.DataBoundItem is not Producto prod)
        {
            MessageBox.Show("Seleccione un producto de la grilla para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var edit = new FrmProductoEdit(prod);
        if (edit.ShowDialog(this) == DialogResult.OK)
        {
            await CargarProductosAsync(txtBuscar.Text.Trim());
        }
    }

    private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            btnEditar.PerformClick();
        }
    }

    private async void btnDesactivar_Click(object sender, EventArgs e)
    {
        if (dgvProductos.CurrentRow?.DataBoundItem is not Producto prod)
        {
            MessageBox.Show("Seleccione un producto de la grilla para desactivar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = MessageBox.Show(
            $"¿Está seguro de desactivar el producto '{prod.Nombre}'?\n\nEl producto dejará de mostrarse en búsquedas y ventas activas.",
            "Confirmar Desactivación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmar != DialogResult.Yes) return;

        try
        {
            await _productoServicio.DesactivarAsync(prod.Id);
            MessageBox.Show("Producto desactivado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await CargarProductosAsync(txtBuscar.Text.Trim());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al desactivar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnAumento_Click(object sender, EventArgs e)
    {
        using var frmAumento = new FrmAumentoPrecios();
        if (frmAumento.ShowDialog(this) == DialogResult.OK)
        {
            await CargarProductosAsync(txtBuscar.Text.Trim());
        }
    }
}
