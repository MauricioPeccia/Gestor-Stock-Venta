using Entidades;
using Servicios;

namespace UI
{
    public partial class FrmCategorias : Form
    {
        private readonly CategoriaServicio _categoriaServicio = new();
        private List<Categoria> _categorias = new();
        private Categoria? _seleccionada;

        public FrmCategorias()
        {
            InitializeComponent();

            ConfigurarGrilla();

            btnAgregar.Click += async (s, e) => await AgregarCategoriaAsync();
            btnModificar.Click += async (s, e) => await ModificarCategoriaAsync();
            btnEliminar.Click += async (s, e) => await EliminarCategoriaAsync();
            btnCerrar.Click += (s, e) => Close();

            dgvCategorias.SelectionChanged += DgvCategorias_SelectionChanged;

            Load += async (s, e) => await CargarCategoriasAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvCategorias.AutoGenerateColumns = false;
            dgvCategorias.Columns.Clear();

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Categoria.Id),
                HeaderText = "ID",
                Width = 80
            });

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Categoria.Nombre),
                HeaderText = "Nombre de la Categoría",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private async Task CargarCategoriasAsync()
        {
            try
            {
                _categorias = await _categoriaServicio.ListarAsync();
                dgvCategorias.DataSource = _categorias;
                txtNombre.Clear();
                _seleccionada = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCategorias_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow?.DataBoundItem is Categoria c)
            {
                _seleccionada = c;
                txtNombre.Text = c.Nombre;
            }
        }

        private async Task AgregarCategoriaAsync()
        {
            var nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            try
            {
                await _categoriaServicio.CrearAsync(new Categoria { Nombre = nombre });
                MessageBox.Show("Categoría agregada con éxito.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarCategoriasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear categoría: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ModificarCategoriaAsync()
        {
            if (_seleccionada == null)
            {
                MessageBox.Show("Seleccione una categoría de la lista para modificar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            try
            {
                _seleccionada.Nombre = nombre;
                await _categoriaServicio.ActualizarAsync(_seleccionada);
                MessageBox.Show("Categoría modificada con éxito.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarCategoriasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar categoría: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EliminarCategoriaAsync()
        {
            if (_seleccionada == null)
            {
                MessageBox.Show("Seleccione una categoría para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var conf = MessageBox.Show(
                $"¿Está seguro de que desea eliminar la categoría '{_seleccionada.Nombre}'?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (conf != DialogResult.Yes) return;

            try
            {
                await _categoriaServicio.DesactivarAsync(_seleccionada.Id);
                MessageBox.Show("Categoría eliminada con éxito.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarCategoriasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar categoría: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

