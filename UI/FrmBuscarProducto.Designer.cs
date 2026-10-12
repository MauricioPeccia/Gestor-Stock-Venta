namespace UI
{
    partial class FrmBuscarProducto
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
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvProductos = new DataGridView();
            btnSeleccionar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 12);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(117, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar producto:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(12, 35);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(720, 27);
            txtBuscar.TabIndex = 1;
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(12, 75);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersWidth = 30;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(720, 320);
            dgvProductos.TabIndex = 2;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.BackColor = Color.ForestGreen;
            btnSeleccionar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnSeleccionar.ForeColor = Color.White;
            btnSeleccionar.Location = new Point(440, 410);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(160, 40);
            btnSeleccionar.TabIndex = 3;
            btnSeleccionar.Text = "Seleccionar (Enter)";
            btnSeleccionar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(610, 410);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(122, 40);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar (Esc)";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // FrmBuscarProducto
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 465);
            Controls.Add(btnCancelar);
            Controls.Add(btnSeleccionar);
            Controls.Add(dgvProductos);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBuscarProducto";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Buscar Producto";
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvProductos;
        private Button btnSeleccionar;
        private Button btnCancelar;
    }
}

