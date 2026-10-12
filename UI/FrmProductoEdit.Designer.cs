namespace UI
{
    partial class FrmProductoEdit
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
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblUnidad = new Label();
            cboUnidad = new ComboBox();
            lblStockMinimo = new Label();
            txtStockMinimo = new TextBox();
            lblPrecioCosto = new Label();
            txtPrecioCosto = new TextBox();
            lblPrecioVenta = new Label();
            txtPrecioVenta = new TextBox();
            lblMargen = new Label();
            lblStockInicial = new Label();
            txtStockInicial = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(198, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nuevo Producto";
            // 
            // lblCodigo
            // 
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(20, 60);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(183, 20);
            lblCodigo.TabIndex = 1;
            lblCodigo.Text = "Código de barras (opcional):";
            // 
            // txtCodigo
            // 
            txtCodigo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtCodigo.Location = new Point(20, 85);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new Size(420, 32);
            txtCodigo.TabIndex = 2;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(20, 125);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(174, 20);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre / Descripción (*):";
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtNombre.Location = new Point(20, 150);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(420, 32);
            txtNombre.TabIndex = 4;
            // 
            // lblUnidad
            // 
            lblUnidad.AutoSize = true;
            lblUnidad.Location = new Point(20, 195);
            lblUnidad.Name = "lblUnidad";
            lblUnidad.Size = new Size(60, 20);
            lblUnidad.TabIndex = 5;
            lblUnidad.Text = "Unidad:";
            // 
            // cboUnidad
            // 
            cboUnidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUnidad.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cboUnidad.FormattingEnabled = true;
            cboUnidad.Items.AddRange(new object[] { "UN", "KG", "LT" });
            cboUnidad.Location = new Point(20, 220);
            cboUnidad.Name = "cboUnidad";
            cboUnidad.Size = new Size(200, 31);
            cboUnidad.TabIndex = 6;
            // 
            // lblStockMinimo
            // 
            lblStockMinimo.AutoSize = true;
            lblStockMinimo.Location = new Point(240, 195);
            lblStockMinimo.Name = "lblStockMinimo";
            lblStockMinimo.Size = new Size(153, 20);
            lblStockMinimo.TabIndex = 7;
            lblStockMinimo.Text = "Stock Mínimo (Alerta):";
            // 
            // txtStockMinimo
            // 
            txtStockMinimo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtStockMinimo.Location = new Point(240, 220);
            txtStockMinimo.Name = "txtStockMinimo";
            txtStockMinimo.Size = new Size(200, 30);
            txtStockMinimo.TabIndex = 8;
            txtStockMinimo.Text = "0";
            // 
            // lblPrecioCosto
            // 
            lblPrecioCosto.AutoSize = true;
            lblPrecioCosto.Location = new Point(20, 265);
            lblPrecioCosto.Name = "lblPrecioCosto";
            lblPrecioCosto.Size = new Size(117, 20);
            lblPrecioCosto.TabIndex = 9;
            lblPrecioCosto.Text = "Precio Costo ($):";
            // 
            // txtPrecioCosto
            // 
            txtPrecioCosto.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtPrecioCosto.Location = new Point(20, 290);
            txtPrecioCosto.Name = "txtPrecioCosto";
            txtPrecioCosto.Size = new Size(200, 32);
            txtPrecioCosto.TabIndex = 10;
            txtPrecioCosto.Text = "0";
            // 
            // lblPrecioVenta
            // 
            lblPrecioVenta.AutoSize = true;
            lblPrecioVenta.Location = new Point(240, 265);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.Size = new Size(137, 20);
            lblPrecioVenta.TabIndex = 11;
            lblPrecioVenta.Text = "Precio de Venta ($):";
            // 
            // txtPrecioVenta
            // 
            txtPrecioVenta.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtPrecioVenta.Location = new Point(240, 290);
            txtPrecioVenta.Name = "txtPrecioVenta";
            txtPrecioVenta.Size = new Size(200, 32);
            txtPrecioVenta.TabIndex = 12;
            txtPrecioVenta.Text = "0";
            // 
            // lblMargen
            // 
            lblMargen.AutoSize = true;
            lblMargen.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            lblMargen.ForeColor = Color.ForestGreen;
            lblMargen.Location = new Point(240, 325);
            lblMargen.Name = "lblMargen";
            lblMargen.Size = new Size(131, 20);
            lblMargen.TabIndex = 13;
            lblMargen.Text = "Margen: 0% ($ 0,00)";
            // 
            // lblStockInicial
            // 
            lblStockInicial.AutoSize = true;
            lblStockInicial.Location = new Point(20, 345);
            lblStockInicial.Name = "lblStockInicial";
            lblStockInicial.Size = new Size(91, 20);
            lblStockInicial.TabIndex = 14;
            lblStockInicial.Text = "Stock Inicial:";
            // 
            // txtStockInicial
            // 
            txtStockInicial.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtStockInicial.Location = new Point(20, 370);
            txtStockInicial.Name = "txtStockInicial";
            txtStockInicial.Size = new Size(200, 32);
            txtStockInicial.TabIndex = 15;
            txtStockInicial.Text = "0";
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.ForestGreen;
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(20, 430);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(220, 45);
            btnGuardar.TabIndex = 16;
            btnGuardar.Text = "GUARDAR (Enter)";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(260, 430);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(180, 45);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar (Esc)";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // FrmProductoEdit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(465, 495);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtStockInicial);
            Controls.Add(lblStockInicial);
            Controls.Add(lblMargen);
            Controls.Add(txtPrecioVenta);
            Controls.Add(lblPrecioVenta);
            Controls.Add(txtPrecioCosto);
            Controls.Add(lblPrecioCosto);
            Controls.Add(txtStockMinimo);
            Controls.Add(lblStockMinimo);
            Controls.Add(cboUnidad);
            Controls.Add(lblUnidad);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtCodigo);
            Controls.Add(lblCodigo);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmProductoEdit";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Producto";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblUnidad;
        private ComboBox cboUnidad;
        private Label lblStockMinimo;
        private TextBox txtStockMinimo;
        private Label lblPrecioCosto;
        private TextBox txtPrecioCosto;
        private Label lblPrecioVenta;
        private TextBox txtPrecioVenta;
        private Label lblMargen;
        private Label lblStockInicial;
        private TextBox txtStockInicial;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}

