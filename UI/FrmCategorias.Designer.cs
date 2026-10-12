namespace UI
{
    partial class FrmCategorias
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            btnAgregar = new Button();
            btnModificar = new Button();
            btnEliminar = new Button();
            dgvCategorias = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(15, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(273, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Categorías";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(15, 60);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(168, 20);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre de la categoría:";
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtNombre.Location = new Point(15, 85);
            txtNombre.Name = "txtNombre";
            txtNombre.PlaceholderText = "Ej: Golosinas, Bebidas, Cigarrillos...";
            txtNombre.Size = new Size(330, 32);
            txtNombre.TabIndex = 2;
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.ForestGreen;
            btnAgregar.Cursor = Cursors.Hand;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(355, 84);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(110, 34);
            btnAgregar.TabIndex = 3;
            btnAgregar.Text = "+ Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.SteelBlue;
            btnModificar.Cursor = Cursors.Hand;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(475, 84);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(110, 34);
            btnModificar.TabIndex = 4;
            btnModificar.Text = "Guardar";
            btnModificar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.Crimson;
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(595, 84);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(110, 34);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // dgvCategorias
            // 
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.AllowUserToDeleteRows = false;
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategorias.Location = new Point(15, 130);
            dgvCategorias.MultiSelect = false;
            dgvCategorias.Name = "dgvCategorias";
            dgvCategorias.ReadOnly = true;
            dgvCategorias.RowHeadersWidth = 35;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.Size = new Size(690, 280);
            dgvCategorias.TabIndex = 6;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.Gray;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(575, 420);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(130, 38);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // FrmCategorias
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 470);
            Controls.Add(btnCerrar);
            Controls.Add(dgvCategorias);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(btnAgregar);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmCategorias";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Categorías";
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private DataGridView dgvCategorias;
        private Button btnCerrar;
    }
}
