namespace UI
{
    partial class FrmProovedores
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
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            btnAgregar = new Button();
            btnModificar = new Button();
            btnEliminar = new Button();
            dgvProveedores = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(15, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(276, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Proveedores";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(15, 60);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(183, 20);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre / Razón Social (*):";
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtNombre.Location = new Point(15, 85);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(260, 27);
            txtNombre.TabIndex = 2;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(290, 60);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(70, 20);
            lblTelefono.TabIndex = 3;
            lblTelefono.Text = "Teléfono:";
            // 
            // txtTelefono
            // 
            txtTelefono.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtTelefono.Location = new Point(290, 85);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(180, 27);
            txtTelefono.TabIndex = 4;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(485, 60);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(49, 20);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtEmail.Location = new Point(485, 85);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(210, 27);
            txtEmail.TabIndex = 6;
            // 
            // lblDireccion
            // 
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new Point(15, 120);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(75, 20);
            lblDireccion.TabIndex = 7;
            lblDireccion.Text = "Dirección:";
            // 
            // txtDireccion
            // 
            txtDireccion.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtDireccion.Location = new Point(15, 145);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(360, 27);
            txtDireccion.TabIndex = 8;
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.ForestGreen;
            btnAgregar.Cursor = Cursors.Hand;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(390, 142);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(100, 32);
            btnAgregar.TabIndex = 9;
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
            btnModificar.Location = new Point(500, 142);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(100, 32);
            btnModificar.TabIndex = 10;
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
            btnEliminar.Location = new Point(610, 142);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(95, 32);
            btnEliminar.TabIndex = 11;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // dgvProveedores
            // 
            dgvProveedores.AllowUserToAddRows = false;
            dgvProveedores.AllowUserToDeleteRows = false;
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProveedores.Location = new Point(15, 190);
            dgvProveedores.MultiSelect = false;
            dgvProveedores.Name = "dgvProveedores";
            dgvProveedores.ReadOnly = true;
            dgvProveedores.RowHeadersWidth = 35;
            dgvProveedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProveedores.Size = new Size(790, 310);
            dgvProveedores.TabIndex = 12;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.Gray;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(675, 515);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(130, 38);
            btnCerrar.TabIndex = 13;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // FrmProovedores
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 565);
            Controls.Add(btnCerrar);
            Controls.Add(dgvProveedores);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(btnAgregar);
            Controls.Add(txtDireccion);
            Controls.Add(lblDireccion);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtTelefono);
            Controls.Add(lblTelefono);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmProovedores";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Proveedores";
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private DataGridView dgvProveedores;
        private Button btnCerrar;
    }
}
