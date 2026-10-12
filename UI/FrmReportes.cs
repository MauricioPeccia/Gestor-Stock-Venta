using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Servicios;
using System.Diagnostics;
using System.Globalization;

namespace UI
{
    public partial class FrmReportes : Form
    {
        private readonly ReporteServicio _reporteServicio = new();
        private readonly CultureInfo _ci = new CultureInfo("es-AR");
        private ReporteResumenPeriodo _resumenActual = new();
        private List<ProductoTopVenta> _topProductos = new();

        public FrmReportes()
        {
            InitializeComponent();

            ConfigurarGrilla();

            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;

            btnConsultar.Click += async (s, e) => await CargarReporteAsync();
            btnExportarPdf.Click += (s, e) => ExportarReportePdf();
            btnCerrar.Click += (s, e) => Close();

            Load += async (s, e) => await CargarReporteAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvTopProductos.AutoGenerateColumns = false;
            dgvTopProductos.Columns.Clear();

            dgvTopProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoTopVenta.Nombre),
                HeaderText = "Producto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvTopProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoTopVenta.CantidadTotal),
                HeaderText = "Cantidad Vendida",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTopProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductoTopVenta.MontoTotal),
                HeaderText = "Total Recaudado",
                Width = 180,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = _ci, Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async Task CargarReporteAsync()
        {
            var desde = dtpDesde.Value.Date;
            var hasta = dtpHasta.Value.Date;

            if (desde > hasta)
            {
                MessageBox.Show("La fecha 'Desde' no puede ser posterior a 'Hasta'.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnConsultar.Enabled = false;

                _resumenActual = await _reporteServicio.ObtenerResumenPeriodoAsync(desde, hasta);
                _topProductos = await _reporteServicio.ObtenerTopProductosAsync(desde, hasta);

                lblTotalVentas.Text = $"Total Facturado: {_resumenActual.TotalVentas.ToString("C2", _ci)}";
                lblEfectivo.Text = $"En Efectivo: {_resumenActual.VentasEfectivo.ToString("C2", _ci)}";
                lblTransf.Text = $"En Transferencia: {_resumenActual.VentasTransferencia.ToString("C2", _ci)}";
                lblCantidadVentas.Text = $"Cantidad de Ventas: {_resumenActual.CantidadVentas}";
                lblGanancia.Text = $"Ganancia Bruta Estimada: {_resumenActual.GananciaBruta.ToString("C2", _ci)}";
                lblCompras.Text = $"Total en Compras: {_resumenActual.TotalCompras.ToString("C2", _ci)}";

                dgvTopProductos.DataSource = _topProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar reportes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConsultar.Enabled = true;
            }
        }

        private void ExportarReportePdf()
        {
            if (_resumenActual.CantidadVentas == 0 && _topProductos.Count == 0)
            {
                MessageBox.Show("No hay datos cargados para exportar en el período seleccionado.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                FileName = $"Reporte_Ventas_{dtpDesde.Value:yyyyMMdd}_{dtpHasta.Value:yyyyMMdd}.pdf",
                Title = "Guardar Reporte en PDF"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var desdeStr = dtpDesde.Value.ToString("dd/MM/yyyy");
                var hastaStr = dtpHasta.Value.ToString("dd/MM/yyyy");

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                        page.Header().Column(col =>
                        {
                            col.Item().Text("GESTOR KIOSCO — REPORTE DE VENTAS").FontSize(18).Bold().FontColor(Colors.Indigo.Medium);
                            col.Item().Text($"Período analizado: {desdeStr} al {hastaStr}").FontSize(11).FontColor(Colors.Grey.Darken1);
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        });

                        page.Content().PaddingVertical(10).Column(col =>
                        {
                            col.Item().Text("Resumen Financiero").FontSize(13).Bold();
                            col.Item().PaddingTop(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text($"Total Facturado: {_resumenActual.TotalVentas.ToString("C2", _ci)}").Bold();
                                table.Cell().Text($"Ganancia Bruta Estimada: {_resumenActual.GananciaBruta.ToString("C2", _ci)}").Bold();
                                table.Cell().Text($"Ventas en Efectivo: {_resumenActual.VentasEfectivo.ToString("C2", _ci)}");
                                table.Cell().Text($"Total en Compras: {_resumenActual.TotalCompras.ToString("C2", _ci)}");
                                table.Cell().Text($"Ventas en Transferencia: {_resumenActual.VentasTransferencia.ToString("C2", _ci)}");
                                table.Cell().Text($"Cantidad de Ventas: {_resumenActual.CantidadVentas}");
                            });

                            col.Item().PaddingTop(20).Text("Top Productos Más Vendidos").FontSize(13).Bold();
                            col.Item().PaddingTop(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("#").Bold();
                                    header.Cell().Text("Producto").Bold();
                                    header.Cell().AlignRight().Text("Cantidad").Bold();
                                    header.Cell().AlignRight().Text("Total").Bold();
                                });

                                for (int i = 0; i < _topProductos.Count; i++)
                                {
                                    var item = _topProductos[i];
                                    table.Cell().Text((i + 1).ToString());
                                    table.Cell().Text(item.Nombre);
                                    table.Cell().AlignRight().Text(item.CantidadTotal.ToString("N2"));
                                    table.Cell().AlignRight().Text(item.MontoTotal.ToString("C2", _ci));
                                }
                            });
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Generado automáticamente el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " | Página ");
                            x.CurrentPageNumber();
                        });
                    });
                }).GeneratePdf(sfd.FileName);

                var abrir = MessageBox.Show("Reporte generado exitosamente en PDF.\n\n¿Desea abrir el archivo ahora?",
                    "Exportación Exitosa", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (abrir == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

