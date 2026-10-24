using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Módulo 4: Reportes y Estadísticas Analíticas.
    /// Consume vistas SQL preexistentes (F1: Cierre Diario + Backup Automático, F2: Top 10 Productos, F3: Ganancias Mensuales).
    /// Construido 100% de forma programática en InicializarComponentes() y consumiendo RecursosUI.
    /// </summary>
    public sealed class FormReportes : Form
    {
        private const string TextoAtajosFooterDefault =
            "[F1] Cierre Diario   |   [F2] Top 10 Productos   |   [F3] Ganancias   |   [Ctrl+B] Backup   |   [ESC] Volver";

        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Panel _pnlResumenHeader = null!;
        private Label _lblResumenTitulo = null!;
        private Label _lblResumenValor = null!;
        private Label _lblSeparadorHeader = null!;

        private Panel _pnlToolbar = null!;
        private Button _btnF1CierreDiario = null!;
        private Button _btnF2TopProductos = null!;
        private Button _btnF3Ganancias = null!;

        private DataGridView _dgvReportes = null!;

        private Panel _pnlFooter = null!;
        private Label _lblAtajosFooter = null!;
        private Label _lblTotalRegistros = null!;

        private readonly ReportesNegocio _reportesNegocio;
        private readonly GestorBackupNegocio _gestorBackup;

        public FormReportes()
        {
            _reportesNegocio = new ReportesNegocio();
            _gestorBackup = new GestorBackupNegocio();

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "[ 4 ] Reportes y Métricas Analíticas - Kiosco Lite";
                this.ClientSize = new Size(1080, 680);
                this.MinimumSize = new Size(920, 580);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.KeyPreview = true;
                this.DoubleBuffered = true;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();
                _pnlResumenHeader = new Panel();
                _lblResumenTitulo = new Label();
                _lblResumenValor = new Label();
                _lblSeparadorHeader = new Label();

                _pnlToolbar = new Panel();
                _btnF1CierreDiario = new Button();
                _btnF2TopProductos = new Button();
                _btnF3Ganancias = new Button();

                _dgvReportes = new DataGridView();

                _pnlFooter = new Panel();
                _lblAtajosFooter = new Label();
                _lblTotalRegistros = new Label();

                // 3. Configuración de Propiedades y Estilos
                // --- Header ---
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Size = new Size(this.ClientSize.Width, 92);
                _pnlHeader.BackColor = RecursosUI.ColorSuperficie;
                _pnlHeader.Padding = new Padding(20);

                _lblTitulo.Text = "Reportes y Estadísticas";
                _lblTitulo.Font = RecursosUI.FuenteTituloModulo;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(20, 14);

                _lblSubtitulo.Text = "Visualización analítica de vistas SQL  [F1: Cierre Diario | F2: Top 10 Productos | F3: Ganancias | ESC: Volver]";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(22, 52);

                _pnlResumenHeader.Size = new Size(240, 60);
                _pnlResumenHeader.Location = new Point(_pnlHeader.Width - 260, 15);
                _pnlResumenHeader.BackColor = RecursosUI.ColorFondoVentana;
                _pnlResumenHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                _lblResumenTitulo.Text = "REPORTE ACTIVO";
                _lblResumenTitulo.Font = RecursosUI.FuentePequenaNegrita;
                _lblResumenTitulo.ForeColor = RecursosUI.ColorTextoMuted;
                _lblResumenTitulo.AutoSize = true;
                _lblResumenTitulo.Location = new Point(12, 8);

                _lblResumenValor.Text = "Cierre Diario";
                _lblResumenValor.Font = RecursosUI.FuenteEncabezado;
                _lblResumenValor.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblResumenValor.AutoSize = true;
                _lblResumenValor.Location = new Point(12, 26);

                _lblSeparadorHeader.Height = 1;
                _lblSeparadorHeader.Dock = DockStyle.Bottom;
                _lblSeparadorHeader.BackColor = RecursosUI.ColorSeparador;

                // --- Toolbar ---
                _pnlToolbar.Dock = DockStyle.Top;
                _pnlToolbar.Size = new Size(this.ClientSize.Width, 60);
                _pnlToolbar.BackColor = RecursosUI.ColorFondoVentana;
                _pnlToolbar.Padding = new Padding(20, 10, 20, 10);

                ConfigurarBotonReporte(_btnF1CierreDiario, "[ F1 ]   Cierre Diario", new Point(20, 10), true, 0);
                ConfigurarBotonReporte(_btnF2TopProductos, "[ F2 ]   Top 10 Productos", new Point(230, 10), false, 1);
                ConfigurarBotonReporte(_btnF3Ganancias, "[ F3 ]   Ganancias Mensuales", new Point(440, 10), false, 2);

                // --- Grilla Analítica ---
                _dgvReportes.Dock = DockStyle.Fill;
                _dgvReportes.BackgroundColor = RecursosUI.ColorSuperficie;
                _dgvReportes.BorderStyle = BorderStyle.None;
                _dgvReportes.EnableHeadersVisualStyles = false;
                _dgvReportes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                _dgvReportes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                _dgvReportes.GridColor = RecursosUI.ColorSeparador;

                // Cabecera con fuente más grande, en negrita y centrada
                _dgvReportes.ColumnHeadersDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvReportes.ColumnHeadersDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvReportes.ColumnHeadersDefaultCellStyle.Font = RecursosUI.FuenteBotonModulo;
                _dgvReportes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _dgvReportes.ColumnHeadersHeight = 48;
                _dgvReportes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                _dgvReportes.DefaultCellStyle.BackColor = RecursosUI.ColorSuperficie;
                _dgvReportes.DefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvReportes.DefaultCellStyle.Font = RecursosUI.FuenteEstandar;
                _dgvReportes.DefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvReportes.DefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvReportes.DefaultCellStyle.Padding = new Padding(6);

                _dgvReportes.AlternatingRowsDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvReportes.AlternatingRowsDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvReportes.AlternatingRowsDefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvReportes.AlternatingRowsDefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;

                _dgvReportes.RowTemplate.Height = 38;
                _dgvReportes.AllowUserToAddRows = false;
                _dgvReportes.AllowUserToDeleteRows = false;
                _dgvReportes.ReadOnly = true;
                _dgvReportes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _dgvReportes.MultiSelect = false;
                _dgvReportes.AllowUserToResizeColumns = false;
                _dgvReportes.AllowUserToResizeRows = false;
                _dgvReportes.RowHeadersVisible = false;
                _dgvReportes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                _dgvReportes.TabIndex = 3;

                // --- Footer ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Size = new Size(this.ClientSize.Width, 42);
                _pnlFooter.BackColor = RecursosUI.ColorBarraEstado;

                _lblAtajosFooter.Text = TextoAtajosFooterDefault;
                _lblAtajosFooter.Font = RecursosUI.FuentePequenaNegrita;
                _lblAtajosFooter.ForeColor = RecursosUI.ColorTextoClaro;
                _lblAtajosFooter.AutoSize = true;
                _lblAtajosFooter.Location = new Point(20, 12);

                _lblTotalRegistros.Text = "Filas: 0";
                _lblTotalRegistros.Font = RecursosUI.FuenteNegrita;
                _lblTotalRegistros.ForeColor = RecursosUI.ColorTextoClaro;
                _lblTotalRegistros.AutoSize = true;
                _lblTotalRegistros.Location = new Point(_pnlFooter.Width - 130, 11);
                _lblTotalRegistros.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                // 5. Suscripción a Eventos
                _btnF1CierreDiario.Click += (s, e) => CargarReporteCierreDiario();
                _btnF2TopProductos.Click += (s, e) => CargarReporteTopProductos();
                _btnF3Ganancias.Click += (s, e) => CargarReporteGanancias();

                this.Load += (s, e) => CargarReporteCierreDiario();

                // 7. Agregado de Controles
                _pnlResumenHeader.Controls.Add(_lblResumenTitulo);
                _pnlResumenHeader.Controls.Add(_lblResumenValor);
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_pnlResumenHeader);
                _pnlHeader.Controls.Add(_lblSeparadorHeader);

                _pnlToolbar.Controls.Add(_btnF1CierreDiario);
                _pnlToolbar.Controls.Add(_btnF2TopProductos);
                _pnlToolbar.Controls.Add(_btnF3Ganancias);

                _pnlFooter.Controls.Add(_lblAtajosFooter);
                _pnlFooter.Controls.Add(_lblTotalRegistros);

                this.Controls.Add(_dgvReportes);
                this.Controls.Add(_pnlToolbar);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private void ConfigurarBotonReporte(Button btn, string texto, Point ubicacion, bool activo, int tabIndex)
        {
            btn.Text = texto;
            btn.Size = new Size(200, 40);
            btn.Location = ubicacion;
            btn.Font = RecursosUI.FuenteNegrita;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TabIndex = tabIndex;
            AplicarEstiloBoton(btn, activo);
        }

        private static void AplicarEstiloBoton(Button btn, bool activo)
        {
            if (activo)
            {
                btn.BackColor = RecursosUI.ColorPrimario;
                btn.ForeColor = RecursosUI.ColorTextoClaro;
            }
            else
            {
                btn.BackColor = RecursosUI.ColorSuperficie;
                btn.ForeColor = RecursosUI.ColorTextoSecundario;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.B))
            {
                _ = DispararBackupSilenciosoAsync(soloSiNoExisteHoy: false);
                return true;
            }

            switch (keyData)
            {
                case Keys.F1:
                    CargarReporteCierreDiario();
                    return true;

                case Keys.F2:
                    CargarReporteTopProductos();
                    return true;

                case Keys.F3:
                    CargarReporteGanancias();
                    return true;

                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CargarReporteCierreDiario()
        {
            ActualizarBotonesActivos(_btnF1CierreDiario);
            _lblResumenValor.Text = "Cierre Diario";

            try
            {
                DataTable dt = _reportesNegocio.ObtenerCierreDiario();
                _dgvReportes.DataSource = null;
                _dgvReportes.DataSource = dt;
                _lblTotalRegistros.Text = $"Filas: {dt.Rows.Count}";

                FormatearGrillaCierreDiario();

                // Disparo automático y silencioso del backup diario al consultar el Cierre de Caja Diario
                _ = DispararBackupSilenciosoAsync(soloSiNoExisteHoy: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al cargar el reporte de Cierre Diario: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarReporteTopProductos()
        {
            ActualizarBotonesActivos(_btnF2TopProductos);
            _lblResumenValor.Text = "Top 10 Productos";

            try
            {
                DataTable dt = _reportesNegocio.ObtenerTop10Productos();
                _dgvReportes.DataSource = null;
                _dgvReportes.DataSource = dt;
                _lblTotalRegistros.Text = $"Filas: {dt.Rows.Count}";

                FormatearGrillaTop10();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al cargar el reporte Top 10 Productos: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarReporteGanancias()
        {
            ActualizarBotonesActivos(_btnF3Ganancias);
            _lblResumenValor.Text = "Ganancias Mensuales";

            try
            {
                DataTable dt = _reportesNegocio.ObtenerGananciasMensuales();
                _dgvReportes.DataSource = null;
                _dgvReportes.DataSource = dt;
                _lblTotalRegistros.Text = $"Filas: {dt.Rows.Count}";

                FormatearGrillaMensual();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al cargar el reporte de Ganancias: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// PASO 1: Estética general de las tablas.
        /// Embellece los HeaderText (reemplazando '_' por espacio y aplicando TitleCase),
        /// centra los encabezados y aplica formato monetario base.
        /// </summary>
        private void FormatearEsteticaGeneralGrilla()
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            _dgvReportes.ColumnHeadersDefaultCellStyle.Font = RecursosUI.FuenteBotonModulo;
            _dgvReportes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn col in _dgvReportes.Columns)
            {
                string nombreOriginal = col.Name;
                string tituloLimpio = nombreOriginal.Equals("anio", StringComparison.OrdinalIgnoreCase)
                    ? "Año"
                    : textInfo.ToTitleCase(nombreOriginal.Replace("_", " ").ToLowerInvariant());

                col.HeaderText = tituloLimpio;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                string nombreLower = nombreOriginal.ToLowerInvariant();
                if (nombreLower.Contains("total") || nombreLower.Contains("monto") || nombreLower.Contains("ganancia") ||
                    nombreLower.Contains("precio") || nombreLower.Contains("ingreso") || nombreLower.Contains("costo") ||
                    nombreLower.Contains("recaudacion") || nombreLower.Contains("efectivo") || nombreLower.Contains("transferencia"))
                {
                    if (!nombreLower.Contains("ventas") && !nombreLower.Contains("operaciones") && !nombreLower.Contains("unidades"))
                    {
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        col.DefaultCellStyle.Format = "C2";
                    }
                }
            }
        }

        /// <summary>
        /// PASO 2: Formato específico para Cierre Diario (F1).
        /// Oculta "tickets_emitidos" y resalta "total_recaudado".
        /// </summary>
        private void FormatearGrillaCierreDiario()
        {
            FormatearEsteticaGeneralGrilla();

            OcultarColumna("tickets_emitidos");

            if (_dgvReportes.Columns.Contains("fecha"))
            {
                _dgvReportes.Columns["fecha"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (_dgvReportes.Columns.Contains("cantidad_operaciones"))
            {
                _dgvReportes.Columns["cantidad_operaciones"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            ResaltarColumnaDestacada("total_recaudado", RecursosUI.FuenteNegrita);
        }

        /// <summary>
        /// PASO 3: Formato específico para Top 10 Productos (F2).
        /// Oculta columnas técnicas, aplica formato condicional/personalizado "0.###" a las unidades
        /// y resalta "ganancia_generada".
        /// </summary>
        private void FormatearGrillaTop10()
        {
            FormatearEsteticaGeneralGrilla();

            OcultarColumna("id_producto");
            OcultarColumna("codigo_interno");
            OcultarColumna("es_fraccionado");

            if (_dgvReportes.Columns.Contains("total_unidades_o_kg"))
            {
                DataGridViewColumn colUnidades = _dgvReportes.Columns["total_unidades_o_kg"]!;
                colUnidades.HeaderText = "Unidades / Kg";
                colUnidades.DefaultCellStyle.Format = "0.###";
                colUnidades.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            ResaltarColumnaDestacada("ganancia_generada", RecursosUI.FuenteNegrita);
        }

        /// <summary>
        /// PASO 4: Formato específico para Ganancias Mensuales (F3).
        /// Centra "total_ventas" y resalta fuertemente "ganancia_neta".
        /// </summary>
        private void FormatearGrillaMensual()
        {
            FormatearEsteticaGeneralGrilla();

            if (_dgvReportes.Columns.Contains("anio"))
            {
                _dgvReportes.Columns["anio"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (_dgvReportes.Columns.Contains("mes"))
            {
                _dgvReportes.Columns["mes"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (_dgvReportes.Columns.Contains("total_ventas"))
            {
                _dgvReportes.Columns["total_ventas"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            ResaltarColumnaDestacada("ganancia_neta", RecursosUI.FuenteGrillaHeader);
        }

        private void OcultarColumna(string nombreColumna)
        {
            if (_dgvReportes.Columns.Contains(nombreColumna))
            {
                _dgvReportes.Columns[nombreColumna]!.Visible = false;
            }
        }

        private void ResaltarColumnaDestacada(string nombreColumna, Font fuente)
        {
            if (!_dgvReportes.Columns.Contains(nombreColumna)) return;

            DataGridViewColumn col = _dgvReportes.Columns[nombreColumna]!;
            col.DefaultCellStyle.BackColor = RecursosUI.ColorResaltadoExitoFondo;
            col.DefaultCellStyle.ForeColor = RecursosUI.ColorTextoTotalCaja;
            col.DefaultCellStyle.SelectionBackColor = RecursosUI.ColorResaltadoExitoSel;
            col.DefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoTotalCaja;
            col.DefaultCellStyle.Font = fuente;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.DefaultCellStyle.Format = "C2";
        }

        /// <summary>
        /// Ejecuta el backup en un hilo secundario (Task.Run) y presenta un aviso visual discreto
        /// y no bloqueante en el pie del formulario utilizando RecursosUI.cs.
        /// </summary>
        private async Task DispararBackupSilenciosoAsync(bool soloSiNoExisteHoy)
        {
            if (this.IsDisposed) return;

            if (!soloSiNoExisteHoy)
            {
                _pnlFooter.BackColor = RecursosUI.ColorMovimientos;
                _lblAtajosFooter.Text = "⏳ Generando respaldo silencioso de kiosco_db (mysqldump)...";
            }

            ResultadoBackup resultado = await _gestorBackup.EjecutarRespaldoSilencioso(soloSiNoExisteHoy);

            if (this.IsDisposed) return;

            if (soloSiNoExisteHoy && resultado.OmitidoPorExistirHoy)
                return;

            _pnlFooter.BackColor = resultado.Exito ? RecursosUI.ColorInventario : RecursosUI.ColorPeligro;
            _lblAtajosFooter.Text = resultado.Exito
                ? $"✔ Respaldo automático en Cierre Diario -> {resultado.Mensaje}"
                : $"✖ Alerta de Backup -> {resultado.Mensaje}";

            await Task.Delay(4500);

            if (!this.IsDisposed)
            {
                _pnlFooter.BackColor = RecursosUI.ColorBarraEstado;
                _lblAtajosFooter.Text = TextoAtajosFooterDefault;
            }
        }

        private void ActualizarBotonesActivos(Button botonActivo)
        {
            AplicarEstiloBoton(_btnF1CierreDiario, _btnF1CierreDiario == botonActivo);
            AplicarEstiloBoton(_btnF2TopProductos, _btnF2TopProductos == botonActivo);
            AplicarEstiloBoton(_btnF3Ganancias, _btnF3Ganancias == botonActivo);
        }
    }
}
