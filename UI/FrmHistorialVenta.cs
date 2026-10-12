using Entidades;
using Servicios;
using System.Globalization;

namespace UI
{
    public partial class FrmHistorialVenta : Form
    {
        private readonly VentaServicio _ventaServicio = new();
        private readonly CajaServicio _cajaServicio = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");
        private JornadaCaja? _jornadaActual;

        public FrmHistorialVenta()
        {
            InitializeComponent();

            ConfigurarGrillas();

            btnRefrescar.Click += async (s, e) => await CargarVentasAsync();
            btnAnular.Click += async (s, e) => await AnularVentaSeleccionadaAsync();
            btnCerrar.Click += (s, e) => Close();

            dgvVentas.SelectionChanged += async (s, e) => await CargarDetalleVentaSeleccionadaAsync();
            dgvVentas.CellFormatting += DgvVentas_CellFormatting;

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F10 && btnAnular.Enabled)
                {
                    e.SuppressKeyPress = true;
                    _ = AnularVentaSeleccionadaAsync();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            };

            Load += async (s, e) => await InicializarAsync();
        }

        private void ConfigurarGrillas()
        {
            dgvVentas.AutoGenerateColumns = false;
            dgvVentas.Columns.Clear();

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "N° Venta",
                Width = 90
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "fecha",
                HeaderText = "Fecha / Hora",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "total",
                HeaderText = "Total",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "estado",
                HeaderText = "Estado",
                Width = 110
            });

            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Producto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cant.",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                HeaderText = "Subtotal",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async Task InicializarAsync()
        {
            try
            {
                _jornadaActual = await _cajaServicio.ObtenerAbiertaAsync()
                                 ?? await _cajaServicio.ObtenerPorFechaAsync(DateTime.Today);

                if (_jornadaActual == null)
                {
                    lblJornadaInfo.Text = "No hay jornada de caja registrada hoy.";
                    btnAnular.Enabled = false;
                    return;
                }

                lblJornadaInfo.Text = $"Jornada #{_jornadaActual.Id} ({_jornadaActual.Fecha:dd/MM/yyyy}) - Estado: {_jornadaActual.Estado}";
                await CargarVentasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar historial: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarVentasAsync()
        {
            if (_jornadaActual == null) return;

            try
            {
                var ventas = (await _ventaServicio.VentasDeJornadaAsync(_jornadaActual.Id)).ToList();
                dgvVentas.DataSource = ventas;

                if (ventas.Count == 0)
                {
                    lblDetalleInfo.Text = "No hay ventas registradas en esta jornada.";
                    dgvDetalle.DataSource = null;
                    lblPagos.Text = "Pagos: -";
                    btnAnular.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvVentas_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvVentas.Rows.Count) return;

            var item = dgvVentas.Rows[e.RowIndex].DataBoundItem;
            if (item != null)
            {
                dynamic row = item;
                if (row.estado == "ANULADA")
                {
                    dgvVentas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DimGray;
                    dgvVentas.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
                    dgvVentas.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.IndianRed;
                    return;
                }
            }
            dgvVentas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }

        private async Task CargarDetalleVentaSeleccionadaAsync()
        {
            var item = dgvVentas.CurrentRow?.DataBoundItem;
            if (item == null)
            {
                btnAnular.Enabled = false;
                return;
            }
            dynamic venta = item;

            long ventaId = Convert.ToInt64(venta.id);
            string estado = Convert.ToString(venta.estado);
            decimal total = Convert.ToDecimal(venta.total);

            lblDetalleInfo.Text = $"Venta #{ventaId} ({total.ToString("C2", _ci)}) - Estado: {estado}";
            btnAnular.Enabled = estado == "COMPLETADA";

            try
            {
                var items = (await _ventaServicio.DetalleDeVentaAsync(ventaId)).ToList();
                dgvDetalle.DataSource = items;

                var pagos = (await _ventaServicio.PagosDeVentaAsync(ventaId)).ToList();
                if (pagos.Count > 0)
                {
                    var resumenPagos = string.Join(" | ", pagos.Select(p => $"{p.Metodo}: {((decimal)p.Monto).ToString("C2", _ci)}"));
                    lblPagos.Text = $"Formas de Pago: {resumenPagos}";
                }
                else
                {
                    lblPagos.Text = "Formas de Pago: No especificadas";
                }
            }
            catch (Exception ex)
            {
                lblDetalleInfo.Text = $"Error al cargar detalle: {ex.Message}";
            }
        }

        private async Task AnularVentaSeleccionadaAsync()
        {
            var item = dgvVentas.CurrentRow?.DataBoundItem;
            if (item == null) return;
            dynamic venta = item;

            long ventaId = Convert.ToInt64(venta.id);
            string estado = Convert.ToString(venta.estado);

            if (estado == "ANULADA")
            {
                MessageBox.Show("Esta venta ya se encuentra anulada.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var motivo = Microsoft.VisualBasic.Interaction.InputBox(
                $"Ingrese el motivo de la anulación para la Venta #{ventaId} (obligatorio):\n(El stock de los productos vendidos será reintegrado automáticamente)",
                "Anular Venta", "Error de cobro / Devolución");

            if (string.IsNullOrWhiteSpace(motivo))
            {
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Está seguro de que desea anular la Venta #{ventaId}?\n\nMotivo: {motivo}\n\nEsta acción reintegrará el stock y modificará el total vendido de la jornada.",
                "Confirmar Anulación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                await _ventaServicio.AnularAsync(ventaId, motivo);
                MessageBox.Show($"La Venta #{ventaId} ha sido anulada con éxito y su stock fue reintegrado.",
                    "Venta Anulada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await CargarVentasAsync();
            }
            catch (NegocioException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al anular venta: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
