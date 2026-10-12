namespace UI
{
    partial class FrmCompras
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
            grpCabecera = new GroupBox();
            txtObs = new TextBox();
            lblObs = new Label();
            chkPagadaConCaja = new CheckBox();
            txtComprobante = new TextBox();
            lblComprobante = new Label();
            cboProveedor = new ComboBox();
            lblProveedor = new Label();
            grpAgregarItem = new GroupBox();
            btnAgregarItem = new Button();
            txtCostoUnitario = new TextBox();
            lblCosto = new Label();
            txtCantidad = new TextBox();
            lblCantidad = new Label();
            btnBuscarProd = new Button();
            txtProducto = new TextBox();
            lblProducto = new Label();
            dgvItems = new DataGridView();
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            btnQuitarItem = new Button();
            btnRegistrarCompra = new Button();
            btnCancelar = new Button();
            grpCabecera.SuspendLayout();
            grpAgregarItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(15, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(397, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registro de Compra de Mercadería";
            // 
            // grpCabecera
            // 
            grpCabecera.Controls.Add(txtObs);
            grpCabecera.Controls.Add(lblObs);
            grpCabecera.Controls.Add(chkPagadaConCaja);
            grpCabecera.Controls.Add(txtComprobante);
            grpCabecera.Controls.Add(lblComprobante);
            grpCabecera.Controls.Add(cboProveedor);
            grpCabecera.Controls.Add(lblProveedor);
            grpCabecera.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            grpCabecera.Location = new Point(15, 50);
            grpCabecera.Name = "grpCabecera";
            grpCabecera.Size = new Size(920, 115);
            grpCabecera.TabIndex = 1;
            grpCabecera.TabStop = false;
            grpCabecera.Text = "Datos del Comprobante / Proveedor";
            // 
            // txtObs
            // 
            txtObs.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtObs.Location = new Point(480, 75);
            txtObs.Name = "txtObs";
            txtObs.PlaceholderText = "Notas adicionales...";
            txtObs.Size = new Size(420, 29);
            txtObs.TabIndex = 6;
            // 
            // lblObs
            // 
            lblObs.AutoSize = true;
            lblObs.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblObs.Location = new Point(480, 52);
            lblObs.Name = "lblObs";
            lblObs.Size = new Size(108, 20);
            lblObs.TabIndex = 5;
            lblObs.Text = "Observaciones:";
            // 
            // chkPagadaConCaja
            // 
            chkPagadaConCaja.AutoSize = true;
            chkPagadaConCaja.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            chkPagadaConCaja.ForeColor = Color.DarkSlateBlue;
            chkPagadaConCaja.Location = new Point(480, 25);
            chkPagadaConCaja.Name = "chkPagadaConCaja";
            chkPagadaConCaja.Size = new Size(393, 25);
            chkPagadaConCaja.TabIndex = 4;
            chkPagadaConCaja.Text = "Pagar con la Caja actual (Registrar Egreso) 💵";
            chkPagadaConCaja.UseVisualStyleBackColor = true;
            // 
            // txtComprobante
            // 
            txtComprobante.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtComprobante.Location = new Point(270, 75);
            txtComprobante.Name = "txtComprobante";
            txtComprobante.PlaceholderText = "Ej: 0001-000421";
            txtComprobante.Size = new Size(190, 29);
            txtComprobante.TabIndex = 3;
            // 
            // lblComprobante
            // 
            lblComprobante.AutoSize = true;
            lblComprobante.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblComprobante.Location = new Point(270, 52);
            lblComprobante.Name = "lblComprobante";
            lblComprobante.Size = new Size(130, 20);
            lblComprobante.TabIndex = 2;
            lblComprobante.Text = "N° Comprobante:";
            // 
            // cboProveedor
            // 
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProveedor.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            cboProveedor.FormattingEnabled = true;
            cboProveedor.Location = new Point(15, 75);
            cboProveedor.Name = "cboProveedor";
            cboProveedor.Size = new Size(240, 29);
            cboProveedor.TabIndex = 1;
            // 
            // lblProveedor
            // 
            lblProveedor.AutoSize = true;
            lblProveedor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblProveedor.Location = new Point(15, 52);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.Size = new Size(80, 20);
            lblProveedor.TabIndex = 0;
            lblProveedor.Text = "Proveedor:";
            // 
            // grpAgregarItem
            // 
            grpAgregarItem.Controls.Add(btnAgregarItem);
            grpAgregarItem.Controls.Add(txtCostoUnitario);
            grpAgregarItem.Controls.Add(lblCosto);
            grpAgregarItem.Controls.Add(txtCantidad);
            grpAgregarItem.Controls.Add(lblCantidad);
            grpAgregarItem.Controls.Add(btnBuscarProd);
            grpAgregarItem.Controls.Add(txtProducto);
            grpAgregarItem.Controls.Add(lblProducto);
            grpAgregarItem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            grpAgregarItem.Location = new Point(15, 175);
            grpAgregarItem.Name = "grpAgregarItem";
            grpAgregarItem.Size = new Size(920, 95);
            grpAgregarItem.TabIndex = 2;
            grpAgregarItem.TabStop = false;
            grpAgregarItem.Text = "Cargar Productos al Remito / Factura";
            // 
            // btnAgregarItem
            // 
            btnAgregarItem.BackColor = Color.ForestGreen;
            btnAgregarItem.Cursor = Cursors.Hand;
            btnAgregarItem.FlatStyle = FlatStyle.Flat;
            btnAgregarItem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnAgregarItem.ForeColor = Color.White;
            btnAgregarItem.Location = new Point(765, 43);
            btnAgregarItem.Name = "btnAgregarItem";
            btnAgregarItem.Size = new Size(135, 34);
            btnAgregarItem.TabIndex = 7;
            btnAgregarItem.Text = "+ Agregar";
            btnAgregarItem.UseVisualStyleBackColor = false;
            // 
            // txtCostoUnitario
            // 
            txtCostoUnitario.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtCostoUnitario.Location = new Point(625, 46);
            txtCostoUnitario.Name = "txtCostoUnitario";
            txtCostoUnitario.Size = new Size(125, 29);
            txtCostoUnitario.TabIndex = 6;
            // 
            // lblCosto
            // 
            lblCosto.AutoSize = true;
            lblCosto.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblCosto.Location = new Point(625, 23);
            lblCosto.Name = "lblCosto";
            lblCosto.Size = new Size(124, 20);
            lblCosto.TabIndex = 5;
            lblCosto.Text = "Costo Unitario ($):";
            // 
            // txtCantidad
            // 
            txtCantidad.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtCantidad.Location = new Point(505, 46);
            txtCantidad.Name = "txtCantidad";
            txtCantidad.Size = new Size(105, 29);
            txtCantidad.TabIndex = 4;
            txtCantidad.Text = "1";
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblCantidad.Location = new Point(505, 23);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(72, 20);
            lblCantidad.TabIndex = 3;
            lblCantidad.Text = "Cantidad:";
            // 
            // btnBuscarProd
            // 
            btnBuscarProd.BackColor = Color.SteelBlue;
            btnBuscarProd.Cursor = Cursors.Hand;
            btnBuscarProd.FlatStyle = FlatStyle.Flat;
            btnBuscarProd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuscarProd.ForeColor = Color.White;
            btnBuscarProd.Location = new Point(390, 44);
            btnBuscarProd.Name = "btnBuscarProd";
            btnBuscarProd.Size = new Size(100, 32);
            btnBuscarProd.TabIndex = 2;
            btnBuscarProd.Text = "Buscar...";
            btnBuscarProd.UseVisualStyleBackColor = false;
            // 
            // txtProducto
            // 
            txtProducto.BackColor = Color.WhiteSmoke;
            txtProducto.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtProducto.Location = new Point(15, 46);
            txtProducto.Name = "txtProducto";
            txtProducto.PlaceholderText = "Seleccione el producto a comprar...";
            txtProducto.ReadOnly = true;
            txtProducto.Size = new Size(365, 29);
            txtProducto.TabIndex = 1;
            // 
            // lblProducto
            // 
            lblProducto.AutoSize = true;
            lblProducto.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblProducto.Location = new Point(15, 23);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(72, 20);
            lblProducto.TabIndex = 0;
            lblProducto.Text = "Producto:";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Location = new Point(15, 280);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersWidth = 35;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(920, 240);
            dgvItems.TabIndex = 3;
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotalTitulo.ForeColor = Color.DimGray;
            lblTotalTitulo.Location = new Point(620, 535);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.Size = new Size(157, 25);
            lblTotalTitulo.TabIndex = 4;
            lblTotalTitulo.Text = "TOTAL COMPRA:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotal.ForeColor = Color.ForestGreen;
            lblTotal.Location = new Point(780, 525);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(119, 46);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "$ 0,00";
            // 
            // btnQuitarItem
            // 
            btnQuitarItem.BackColor = Color.IndianRed;
            btnQuitarItem.Cursor = Cursors.Hand;
            btnQuitarItem.FlatStyle = FlatStyle.Flat;
            btnQuitarItem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnQuitarItem.ForeColor = Color.White;
            btnQuitarItem.Location = new Point(15, 530);
            btnQuitarItem.Name = "btnQuitarItem";
            btnQuitarItem.Size = new Size(150, 42);
            btnQuitarItem.TabIndex = 6;
            btnQuitarItem.Text = "Quitar Ítem";
            btnQuitarItem.UseVisualStyleBackColor = false;
            // 
            // btnRegistrarCompra
            // 
            btnRegistrarCompra.BackColor = Color.ForestGreen;
            btnRegistrarCompra.Cursor = Cursors.Hand;
            btnRegistrarCompra.FlatStyle = FlatStyle.Flat;
            btnRegistrarCompra.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnRegistrarCompra.ForeColor = Color.White;
            btnRegistrarCompra.Location = new Point(180, 530);
            btnRegistrarCompra.Name = "btnRegistrarCompra";
            btnRegistrarCompra.Size = new Size(240, 42);
            btnRegistrarCompra.TabIndex = 7;
            btnRegistrarCompra.Text = "REGISTRAR COMPRA";
            btnRegistrarCompra.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(435, 530);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(130, 42);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Volver";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // FrmCompras
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 585);
            Controls.Add(btnCancelar);
            Controls.Add(btnRegistrarCompra);
            Controls.Add(btnQuitarItem);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalTitulo);
            Controls.Add(dgvItems);
            Controls.Add(grpAgregarItem);
            Controls.Add(grpCabecera);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmCompras";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ingreso de Mercadería y Compras";
            grpCabecera.ResumeLayout(false);
            grpCabecera.PerformLayout();
            grpAgregarItem.ResumeLayout(false);
            grpAgregarItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private GroupBox grpCabecera;
        private ComboBox cboProveedor;
        private Label lblProveedor;
        private TextBox txtComprobante;
        private Label lblComprobante;
        private CheckBox chkPagadaConCaja;
        private TextBox txtObs;
        private Label lblObs;
        private GroupBox grpAgregarItem;
        private Label lblProducto;
        private TextBox txtProducto;
        private Button btnBuscarProd;
        private Label lblCantidad;
        private TextBox txtCantidad;
        private Label lblCosto;
        private TextBox txtCostoUnitario;
        private Button btnAgregarItem;
        private DataGridView dgvItems;
        private Label lblTotalTitulo;
        private Label lblTotal;
        private Button btnQuitarItem;
        private Button btnRegistrarCompra;
        private Button btnCancelar;
    }
}
