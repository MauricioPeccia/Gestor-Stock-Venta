using Kiosco.Servicios;

namespace Kiosco.UI;

public partial class FrmAumentoPrecios : Form
{
    private readonly ProductoServicio _productoServicio = new();
    private readonly CategoriaServicio _categoriaServicio = new();

    public FrmAumentoPrecios()
    {
        InitializeComponent();
    }

    private async void FrmAumentoPrecios_Load(object sender, EventArgs e)
    {
        await CargarCategoriasAsync();
        cboRedondeo.SelectedIndex = 2; // Default: 10
    }

    private async Task CargarCategoriasAsync()
    {
        try
        {
            var categorias = await _categoriaServicio.ObtenerTodasAsync();
            var lista = new List<CategoriaItem>
            {
                new CategoriaItem(0, "[Todas las categorías]")
            };

            foreach (var cat in categorias)
            {
                lista.Add(new CategoriaItem(cat.Id, cat.Nombre));
            }

            cboCategoria.DataSource = lista;
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "Id";
        }
        catch
        {
            cboCategoria.DataSource = new List<CategoriaItem> { new CategoriaItem(0, "[Todas las categorías]") };
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "Id";
        }
    }

    private async void btnAplicar_Click(object sender, EventArgs e)
    {
        var porcentaje = numPorcentaje.Value;
        if (porcentaje == 0)
        {
            MessageBox.Show("El porcentaje de aumento no puede ser 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            numPorcentaje.Focus();
            return;
        }

        var categoriaIdVal = (int)(cboCategoria.SelectedValue ?? 0);
        int? categoriaId = categoriaIdVal > 0 ? categoriaIdVal : null;

        var redondearA = cboRedondeo.SelectedIndex switch
        {
            0 => 1,
            1 => 5,
            2 => 10,
            3 => 50,
            _ => 10
        };

        var catNombre = categoriaId.HasValue ? cboCategoria.Text : "todos los productos activos";
        var confirmar = MessageBox.Show(
            $"¿Está seguro de aplicar un aumento del {porcentaje:N2}% a {catNombre}?\n\n(Redondeando precios de venta a múltiplos de {redondearA})",
            "Confirmar Aumento Masivo",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmar != DialogResult.Yes) return;

        try
        {
            btnAplicar.Enabled = false;
            var filas = await _productoServicio.AumentarPreciosAsync(porcentaje, categoriaId, redondearA);

            MessageBox.Show(
                $"Se han actualizado exitosamente los precios de {filas} producto(s).",
                "Aumento Realizado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnAplicar.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error al aplicar el aumento masivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnAplicar.Enabled = true;
        }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private record CategoriaItem(int Id, string Nombre);
}

