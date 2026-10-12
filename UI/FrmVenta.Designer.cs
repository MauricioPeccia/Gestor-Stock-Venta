namespace UI
{
    partial class FrmVenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtBusqueda = new TextBox();
            lblUltimoProducto = new Label();
            dgvItems = new DataGridView();
            btnCobrar = new Button();
            btnEliminar = new Button();
            btnCancelar = new Button();
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            lblItemsCount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // txtBusqueda
            // 
            txtBusqueda.Font = new Font("Segoe UI", 12F);
            txtBusqueda.Location = new Point(10, 10);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.PlaceholderText = "Escanear código de barras o escribir nombre del producto (Enter para agregar)...";
            txtBusqueda.Size = new Size(934, 34);
            txtBusqueda.TabIndex = 0;
            // 
            // lblUltimoProducto
            // 
            lblUltimoProducto.AutoSize = true;
            lblUltimoProducto.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblUltimoProducto.ForeColor = Color.DimGray;
            lblUltimoProducto.Location = new Point(12, 48);
            lblUltimoProducto.Name = "lblUltimoProducto";
            lblUltimoProducto.Size = new Size(395, 21);
            lblUltimoProducto.TabIndex = 1;
            lblUltimoProducto.Text = "Listo para escanear productos. F3 = Buscar en catálogo.";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Location = new Point(10, 75);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersWidth = 40;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(934, 400);
            dgvItems.TabIndex = 2;
            // 
            // btnCobrar
            // 
            btnCobrar.BackColor = Color.OliveDrab;
            btnCobrar.Cursor = Cursors.Hand;
            btnCobrar.FlatStyle = FlatStyle.Flat;
            btnCobrar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCobrar.ForeColor = Color.White;
            btnCobrar.Location = new Point(12, 502);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(180, 53);
            btnCobrar.TabIndex = 3;
            btnCobrar.Text = "COBRAR (F12)";
            btnCobrar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.Firebrick;
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(202, 502);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(180, 53);
            btnEliminar.TabIndex = 4;
            btnEliminar.Text = "Quitar Ítem (Supr)";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(392, 502);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(180, 53);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar (Esc)";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalTitulo.ForeColor = Color.DimGray;
            lblTotalTitulo.Location = new Point(754, 502);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.Size = new Size(73, 25);
            lblTotalTitulo.TabIndex = 6;
            lblTotalTitulo.Text = "TOTAL:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTotal.ForeColor = Color.ForestGreen;
            lblTotal.Location = new Point(749, 522);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(149, 60);
            lblTotal.TabIndex = 7;
            lblTotal.Text = "$ 0,00";
            // 
            // lblItemsCount
            // 
            lblItemsCount.AutoSize = true;
            lblItemsCount.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblItemsCount.ForeColor = Color.Black;
            lblItemsCount.Location = new Point(14, 567);
            lblItemsCount.Name = "lblItemsCount";
            lblItemsCount.Size = new Size(70, 23);
            lblItemsCount.TabIndex = 8;
            lblItemsCount.Text = "Ítems: 0";
            // 
            // FrmVenta
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(961, 601);
            Controls.Add(lblItemsCount);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalTitulo);
            Controls.Add(btnCancelar);
            Controls.Add(btnEliminar);
            Controls.Add(btnCobrar);
            Controls.Add(dgvItems);
            Controls.Add(lblUltimoProducto);
            Controls.Add(txtBusqueda);
            KeyPreview = true;
            Name = "FrmVenta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Punto de Venta";
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBusqueda;
        private Label lblUltimoProducto;
        private DataGridView dgvItems;
        private Button btnCobrar;
        private Button btnEliminar;
        private Button btnCancelar;
        private Label lblTotalTitulo;
        private Label lblTotal;
        private Label lblItemsCount;
    }
}