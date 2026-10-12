namespace UI
{
    partial class FrmProductos
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
            chkSoloBajoStock = new CheckBox();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnAumentoPrecios = new Button();
            btnAjusteStock = new Button();
            dgvProductos = new DataGridView();
            lblInfo = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblBuscar.Location = new Point(12, 15);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(135, 23);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar producto:";
            // 
            // txtBuscar
            // 
            txtBuscar.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtBuscar.Location = new Point(150, 10);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Escribir código o nombre...";
            txtBuscar.Size = new Size(350, 32);
            txtBuscar.TabIndex = 1;
            // 
            // chkSoloBajoStock
            // 
            chkSoloBajoStock.AutoSize = true;
            chkSoloBajoStock.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            chkSoloBajoStock.ForeColor = Color.DarkRed;
            chkSoloBajoStock.Location = new Point(515, 14);
            chkSoloBajoStock.Name = "chkSoloBajoStock";
            chkSoloBajoStock.Size = new Size(244, 27);
            chkSoloBajoStock.TabIndex = 2;
            chkSoloBajoStock.Text = "Solo bajo stock mínimo ⚠️";
            chkSoloBajoStock.UseVisualStyleBackColor = true;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.ForestGreen;
            btnNuevo.Cursor = Cursors.Hand;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(12, 55);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(160, 42);
            btnNuevo.TabIndex = 3;
            btnNuevo.Text = "+ Nuevo Producto";
            btnNuevo.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.SteelBlue;
            btnEditar.Cursor = Cursors.Hand;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(180, 55);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(140, 42);
            btnEditar.TabIndex = 4;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.Crimson;
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(330, 55);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(150, 42);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Dar de Baja";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // btnAumentoPrecios
            // 
            btnAumentoPrecios.BackColor = Color.DarkOrange;
            btnAumentoPrecios.Cursor = Cursors.Hand;
            btnAumentoPrecios.FlatStyle = FlatStyle.Flat;
            btnAumentoPrecios.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnAumentoPrecios.ForeColor = Color.White;
            btnAumentoPrecios.Location = new Point(490, 55);
            btnAumentoPrecios.Name = "btnAumentoPrecios";
            btnAumentoPrecios.Size = new Size(200, 42);
            btnAumentoPrecios.TabIndex = 6;
            btnAumentoPrecios.Text = "Aumento Masivo %";
            btnAumentoPrecios.UseVisualStyleBackColor = false;
            // 
            // btnAjusteStock
            // 
            btnAjusteStock.BackColor = Color.SlateGray;
            btnAjusteStock.Cursor = Cursors.Hand;
            btnAjusteStock.FlatStyle = FlatStyle.Flat;
            btnAjusteStock.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnAjusteStock.ForeColor = Color.White;
            btnAjusteStock.Location = new Point(700, 55);
            btnAjusteStock.Name = "btnAjusteStock";
            btnAjusteStock.Size = new Size(150, 42);
            btnAjusteStock.TabIndex = 7;
            btnAjusteStock.Text = "Ajustar Stock";
            btnAjusteStock.UseVisualStyleBackColor = false;
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(12, 110);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersWidth = 35;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(980, 430);
            dgvProductos.TabIndex = 8;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblInfo.ForeColor = Color.DimGray;
            lblInfo.Location = new Point(12, 555);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(130, 21);
            lblInfo.TabIndex = 9;
            lblInfo.Text = "Cargando datos...";
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.Gray;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(852, 545);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(140, 40);
            btnCerrar.TabIndex = 10;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // FrmProductos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1004, 595);
            Controls.Add(btnCerrar);
            Controls.Add(lblInfo);
            Controls.Add(dgvProductos);
            Controls.Add(btnAjusteStock);
            Controls.Add(btnAumentoPrecios);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnNuevo);
            Controls.Add(chkSoloBajoStock);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FrmProductos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Administración de Productos";
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private CheckBox chkSoloBajoStock;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnAumentoPrecios;
        private Button btnAjusteStock;
        private DataGridView dgvProductos;
        private Label lblInfo;
        private Button btnCerrar;
    }
}