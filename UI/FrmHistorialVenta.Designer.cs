namespace UI
{
    partial class FrmHistorialVenta
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
            lblJornadaInfo = new Label();
            btnRefrescar = new Button();
            dgvVentas = new DataGridView();
            grpDetalle = new GroupBox();
            lblPagos = new Label();
            dgvDetalle = new DataGridView();
            lblDetalleInfo = new Label();
            btnAnular = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            grpDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(227, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Historial de Ventas";
            // 
            // lblJornadaInfo
            // 
            lblJornadaInfo.AutoSize = true;
            lblJornadaInfo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblJornadaInfo.ForeColor = Color.DimGray;
            lblJornadaInfo.Location = new Point(14, 48);
            lblJornadaInfo.Name = "lblJornadaInfo";
            lblJornadaInfo.Size = new Size(169, 23);
            lblJornadaInfo.TabIndex = 1;
            lblJornadaInfo.Text = "Jornada actual: ...";
            // 
            // btnRefrescar
            // 
            btnRefrescar.BackColor = Color.SteelBlue;
            btnRefrescar.Cursor = Cursors.Hand;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.Location = new Point(360, 35);
            btnRefrescar.Name = "btnRefrescar";
            btnRefrescar.Size = new Size(130, 36);
            btnRefrescar.TabIndex = 2;
            btnRefrescar.Text = "Actualizar";
            btnRefrescar.UseVisualStyleBackColor = false;
            // 
            // dgvVentas
            // 
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.Location = new Point(12, 80);
            dgvVentas.MultiSelect = false;
            dgvVentas.Name = "dgvVentas";
            dgvVentas.ReadOnly = true;
            dgvVentas.RowHeadersWidth = 35;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.Size = new Size(478, 440);
            dgvVentas.TabIndex = 3;
            // 
            // grpDetalle
            // 
            grpDetalle.Controls.Add(lblPagos);
            grpDetalle.Controls.Add(dgvDetalle);
            grpDetalle.Controls.Add(lblDetalleInfo);
            grpDetalle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpDetalle.Location = new Point(505, 80);
            grpDetalle.Name = "grpDetalle";
            grpDetalle.Size = new Size(465, 440);
            grpDetalle.TabIndex = 4;
            grpDetalle.TabStop = false;
            grpDetalle.Text = "Detalle de Venta";
            // 
            // lblPagos
            // 
            lblPagos.AutoSize = true;
            lblPagos.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblPagos.ForeColor = Color.DimGray;
            lblPagos.Location = new Point(12, 405);
            lblPagos.Name = "lblPagos";
            lblPagos.Size = new Size(62, 20);
            lblPagos.TabIndex = 2;
            lblPagos.Text = "Pagos: -";
            // 
            // dgvDetalle
            // 
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.Location = new Point(12, 60);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.RowHeadersWidth = 30;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(440, 335);
            dgvDetalle.TabIndex = 1;
            // 
            // lblDetalleInfo
            // 
            lblDetalleInfo.AutoSize = true;
            lblDetalleInfo.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblDetalleInfo.ForeColor = Color.DarkSlateGray;
            lblDetalleInfo.Location = new Point(12, 30);
            lblDetalleInfo.Name = "lblDetalleInfo";
            lblDetalleInfo.Size = new Size(213, 21);
            lblDetalleInfo.TabIndex = 0;
            lblDetalleInfo.Text = "Seleccione una venta para ver";
            // 
            // btnAnular
            // 
            btnAnular.BackColor = Color.Crimson;
            btnAnular.Cursor = Cursors.Hand;
            btnAnular.FlatStyle = FlatStyle.Flat;
            btnAnular.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnAnular.ForeColor = Color.White;
            btnAnular.Location = new Point(12, 535);
            btnAnular.Name = "btnAnular";
            btnAnular.Size = new Size(230, 45);
            btnAnular.TabIndex = 5;
            btnAnular.Text = "ANULAR VENTA (F10)";
            btnAnular.UseVisualStyleBackColor = false;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.Gray;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(830, 535);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(140, 45);
            btnCerrar.TabIndex = 6;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // FrmHistorialVenta
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(982, 595);
            Controls.Add(btnCerrar);
            Controls.Add(btnAnular);
            Controls.Add(grpDetalle);
            Controls.Add(dgvVentas);
            Controls.Add(btnRefrescar);
            Controls.Add(lblJornadaInfo);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmHistorialVenta";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historial y Anulación de Ventas";
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            grpDetalle.ResumeLayout(false);
            grpDetalle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblJornadaInfo;
        private Button btnRefrescar;
        private DataGridView dgvVentas;
        private GroupBox grpDetalle;
        private Label lblDetalleInfo;
        private DataGridView dgvDetalle;
        private Label lblPagos;
        private Button btnAnular;
        private Button btnCerrar;
    }
}