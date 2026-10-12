using Entidades;
using Servicios;

namespace UI
{
    public partial class FrmProovedores : Form
    {
        private readonly ProveedorServicio _proveedorServicio = new();
        private List<Proveedor> _proveedores = new();
        private Proveedor? _seleccionado;

        public FrmProovedores()
        {
            InitializeComponent();

            ConfigurarGrilla();

            btnAgregar.Click += async (s, e) => await AgregarProveedorAsync();
            btnModificar.Click += async (s, e) => await ModificarProveedorAsync();
            btnEliminar.Click += async (s, e) => await EliminarProveedorAsync();
            btnCerrar.Click += (s, e) => Close();

            dgvProveedores.SelectionChanged += DgvProveedores_SelectionChanged;

            Load += async (s, e) => await CargarProveedoresAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvProveedores.AutoGenerateColumns = false;
            dgvProveedores.Columns.Clear();

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Proveedor.Id),
                HeaderText = "ID",
                Width = 70
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Proveedor.Nombre),
                HeaderText = "Nombre / Razón Social",
                Width = 220
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Proveedor.Telefono),
                HeaderText = "Teléfono",
                Width = 140
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Proveedor.Email),
                HeaderText = "Email",
                Width = 170
            });

            dgvProveedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Proveedor.Direccion),
                HeaderText = "Dirección",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private async Task CargarProveedoresAsync()
        {
            try
            {
                _proveedores = await _proveedorServicio.ListarAsync();
                dgvProveedores.DataSource = _proveedores;
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            _seleccionado = null;
        }

        private void DgvProveedores_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow?.DataBoundItem is Proveedor p)
            {
                _seleccionado = p;
                txtNombre.Text = p.Nombre;
                txtTelefono.Text = p.Telefono ?? "";
                txtEmail.Text = p.Email ?? "";
                txtDireccion.Text = p.Direccion ?? "";
            }
        }

        private async Task AgregarProveedorAsync()
        {
            var nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre o razón social es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            try
            {
                var nuevo = new Proveedor
                {
                    Nombre = nombre,
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim()
                };

                await _proveedorServicio.CrearAsync(nuevo);
                MessageBox.Show("Proveedor agregado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarProveedoresAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ModificarProveedorAsync()
        {
            if (_seleccionado == null)
            {
                MessageBox.Show("Seleccione un proveedor de la grilla para modificar.", "Aviso",
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
                _seleccionado.Nombre = nombre;
                _seleccionado.Telefono = txtTelefono.Text.Trim();
                _seleccionado.Email = txtEmail.Text.Trim();
                _seleccionado.Direccion = txtDireccion.Text.Trim();

                await _proveedorServicio.ActualizarAsync(_seleccionado);
                MessageBox.Show("Proveedor actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarProveedoresAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EliminarProveedorAsync()
        {
            if (_seleccionado == null)
            {
                MessageBox.Show("Seleccione un proveedor para dar de baja.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var conf = MessageBox.Show(
                $"¿Está seguro de que desea eliminar al proveedor '{_seleccionado.Nombre}'?",
                "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (conf != DialogResult.Yes) return;

            try
            {
                await _proveedorServicio.DesactivarAsync(_seleccionado.Id);
                MessageBox.Show("Proveedor eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarProveedoresAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

