namespace UI
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            grpFiltros = new GroupBox();
            btnExportarPdf = new Button();
            btnConsultar = new Button();
            dtpHasta = new DateTimePicker();
            lblHasta = new Label();
            dtpDesde = new DateTimePicker();
            lblDesde = new Label();
            grpResumen = new GroupBox();
            lblCompras = new Label();
            lblGanancia = new Label();
            lblCantidadVentas = new Label();
            lblTransf = new Label();
            lblEfectivo = new Label();
            lblTotalVentas = new Label();
            grpTopProductos = new GroupBox();
            dgvTopProductos = new DataGridView();
            btnCerrar = new Button();
            grpFiltros.SuspendLayout();
            grpResumen.SuspendLayout();
            grpTopProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopProductos).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(15, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(428, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reportes y Estadísticas de Ventas";
            // 
            // grpFiltros
            // 
            grpFiltros.Controls.Add(btnExportarPdf);
            grpFiltros.Controls.Add(btnConsultar);
            grpFiltros.Controls.Add(dtpHasta);
            grpFiltros.Controls.Add(lblHasta);
            grpFiltros.Controls.Add(dtpDesde);
            grpFiltros.Controls.Add(lblDesde);
            grpFiltros.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            grpFiltros.Location = new Point(15, 55);
            grpFiltros.Name = "grpFiltros";
            grpFiltros.Size = new Size(920, 80);
            grpFiltros.TabIndex = 1;
            grpFiltros.TabStop = false;
            grpFiltros.Text = "Período de Análisis";
            // 
            // btnExportarPdf
            // 
            btnExportarPdf.BackColor = Color.DarkMagenta;
            btnExportarPdf.Cursor = Cursors.Hand;
            btnExportarPdf.FlatStyle = FlatStyle.Flat;
            btnExportarPdf.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnExportarPdf.ForeColor = Color.White;
            btnExportarPdf.Location = new Point(720, 27);
            btnExportarPdf.Name = "btnExportarPdf";
            btnExportarPdf.Size = new Size(185, 38);
            btnExportarPdf.TabIndex = 5;
            btnExportarPdf.Text = "📄 Exportar PDF";
            btnExportarPdf.UseVisualStyleBackColor = false;
            // 
            // btnConsultar
            // 
            btnConsultar.BackColor = Color.SteelBlue;
            btnConsultar.Cursor = Cursors.Hand;
            btnConsultar.FlatStyle = FlatStyle.Flat;
            btnConsultar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnConsultar.ForeColor = Color.White;
            btnConsultar.Location = new Point(545, 27);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(160, 38);
            btnConsultar.TabIndex = 4;
            btnConsultar.Text = "🔍 Consultar";
            btnConsultar.UseVisualStyleBackColor = false;
            // 
            // dtpHasta
            // 
            dtpHasta.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(345, 31);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(170, 29);
            dtpHasta.TabIndex = 3;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblHasta.Location = new Point(290, 36);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(49, 20);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "Hasta:";
            // 
            // dtpDesde
            // 
            dtpDesde.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(80, 31);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(170, 29);
            dtpDesde.TabIndex = 1;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblDesde.Location = new Point(20, 36);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(54, 20);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde:";
            // 
            // grpResumen
            // 
            grpResumen.Controls.Add(lblCompras);
            grpResumen.Controls.Add(lblGanancia);
            grpResumen.Controls.Add(lblCantidadVentas);
            grpResumen.Controls.Add(lblTransf);
            grpResumen.Controls.Add(lblEfectivo);
            grpResumen.Controls.Add(lblTotalVentas);
            grpResumen.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpResumen.Location = new Point(15, 145);
            grpResumen.Name = "grpResumen";
            grpResumen.Size = new Size(920, 145);
            grpResumen.TabIndex = 2;
            grpResumen.TabStop = false;
            grpResumen.Text = "Métricas Financieras del Período";
            // 
            // lblCompras
            // 
            lblCompras.AutoSize = true;
            lblCompras.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblCompras.ForeColor = Color.RoyalBlue;
            lblCompras.Location = new Point(480, 105);
            lblCompras.Name = "lblCompras";
            lblCompras.Size = new Size(207, 25);
            lblCompras.TabIndex = 5;
            lblCompras.Text = "Total en Compras: $ 0,00";
            // 
            // lblGanancia
            // 
            lblGanancia.AutoSize = true;
            lblGanancia.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblGanancia.ForeColor = Color.ForestGreen;
            lblGanancia.Location = new Point(480, 68);
            lblGanancia.Name = "lblGanancia";
            lblGanancia.Size = new Size(278, 28);
            lblGanancia.TabIndex = 4;
            lblGanancia.Text = "Ganancia Bruta Estimada: -";
            // 
            // lblCantidadVentas
            // 
            lblCantidadVentas.AutoSize = true;
            lblCantidadVentas.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblCantidadVentas.ForeColor = Color.DimGray;
            lblCantidadVentas.Location = new Point(480, 35);
            lblCantidadVentas.Name = "lblCantidadVentas";
            lblCantidadVentas.Size = new Size(160, 23);
            lblCantidadVentas.TabIndex = 3;
            lblCantidadVentas.Text = "Cantidad Ventas: 0";
            // 
            // lblTransf
            // 
            lblTransf.AutoSize = true;
            lblTransf.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTransf.ForeColor = Color.DarkSlateGray;
            lblTransf.Location = new Point(20, 105);
            lblTransf.Name = "lblTransf";
            lblTransf.Size = new Size(203, 23);
            lblTransf.TabIndex = 2;
            lblTransf.Text = "En Transferencia: $ 0,00";
            // 
            // lblEfectivo
            // 
            lblEfectivo.AutoSize = true;
            lblEfectivo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblEfectivo.ForeColor = Color.DarkSlateGray;
            lblEfectivo.Location = new Point(20, 72);
            lblEfectivo.Name = "lblEfectivo";
            lblEfectivo.Size = new Size(162, 23);
            lblEfectivo.TabIndex = 1;
            lblEfectivo.Text = "En Efectivo: $ 0,00";
            // 
            // lblTotalVentas
            // 
            lblTotalVentas.AutoSize = true;
            lblTotalVentas.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotalVentas.ForeColor = Color.DarkBlue;
            lblTotalVentas.Location = new Point(20, 30);
            lblTotalVentas.Name = "lblTotalVentas";
            lblTotalVentas.Size = new Size(260, 32);
            lblTotalVentas.TabIndex = 0;
            lblTotalVentas.Text = "Total Facturado: $ 0,00";
            // 
            // grpTopProductos
            // 
            grpTopProductos.Controls.Add(dgvTopProductos);
            grpTopProductos.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpTopProductos.Location = new Point(15, 300);
            grpTopProductos.Name = "grpTopProductos";
            grpTopProductos.Size = new Size(920, 235);
            grpTopProductos.TabIndex = 3;
            grpTopProductos.TabStop = false;
            grpTopProductos.Text = "Top 10 Productos Más Vendidos";
            // 
            // dgvTopProductos
            // 
            dgvTopProductos.AllowUserToAddRows = false;
            dgvTopProductos.AllowUserToDeleteRows = false;
            dgvTopProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTopProductos.Location = new Point(15, 30);
            dgvTopProductos.MultiSelect = false;
            dgvTopProductos.Name = "dgvTopProductos";
            dgvTopProductos.ReadOnly = true;
            dgvTopProductos.RowHeadersWidth = 35;
            dgvTopProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTopProductos.Size = new Size(890, 190);
            dgvTopProductos.TabIndex = 0;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.Gray;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(805, 545);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(130, 38);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 595);
            Controls.Add(btnCerrar);
            Controls.Add(grpTopProductos);
            Controls.Add(grpResumen);
            Controls.Add(grpFiltros);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmReportes";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reportes y Estadísticas";
            grpFiltros.ResumeLayout(false);
            grpFiltros.PerformLayout();
            grpResumen.ResumeLayout(false);
            grpResumen.PerformLayout();
            grpTopProductos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private GroupBox grpFiltros;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnConsultar;
        private Button btnExportarPdf;
        private GroupBox grpResumen;
        private Label lblTotalVentas;
        private Label lblEfectivo;
        private Label lblTransf;
        private Label lblCantidadVentas;
        private Label lblGanancia;
        private Label lblCompras;
        private GroupBox grpTopProductos;
        private DataGridView dgvTopProductos;
        private Button btnCerrar;
    }
}
