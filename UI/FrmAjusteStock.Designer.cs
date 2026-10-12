namespace UI
{
    partial class FrmAjusteStock
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
            lblProducto = new Label();
            txtProducto = new TextBox();
            btnBuscarProducto = new Button();
            lblStockActual = new Label();
            lblNuevoStock = new Label();
            txtNuevoStock = new TextBox();
            lblDiferencia = new Label();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
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
            lblTitulo.Size = new Size(276, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ajuste Manual de Stock";
            // 
            // lblProducto
            // 
            lblProducto.AutoSize = true;
            lblProducto.Location = new Point(20, 60);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(157, 20);
            lblProducto.TabIndex = 1;
            lblProducto.Text = "Producto seleccionado:";
            // 
            // txtProducto
            // 
            txtProducto.BackColor = Color.WhiteSmoke;
            txtProducto.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            txtProducto.ForeColor = Color.DarkSlateGray;
            txtProducto.Location = new Point(20, 85);
            txtProducto.Name = "txtProducto";
            txtProducto.PlaceholderText = "Haga clic en 'Buscar Producto' para seleccionar...";
            txtProducto.ReadOnly = true;
            txtProducto.Size = new Size(310, 31);
            txtProducto.TabIndex = 2;
            // 
            // btnBuscarProducto
            // 
            btnBuscarProducto.BackColor = Color.SteelBlue;
            btnBuscarProducto.Cursor = Cursors.Hand;
            btnBuscarProducto.FlatStyle = FlatStyle.Flat;
            btnBuscarProducto.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuscarProducto.ForeColor = Color.White;
            btnBuscarProducto.Location = new Point(340, 84);
            btnBuscarProducto.Name = "btnBuscarProducto";
            btnBuscarProducto.Size = new Size(120, 33);
            btnBuscarProducto.TabIndex = 3;
            btnBuscarProducto.Text = "🔍 Buscar...";
            btnBuscarProducto.UseVisualStyleBackColor = false;
            // 
            // lblStockActual
            // 
            lblStockActual.AutoSize = true;
            lblStockActual.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblStockActual.ForeColor = Color.SteelBlue;
            lblStockActual.Location = new Point(20, 130);
            lblStockActual.Name = "lblStockActual";
            lblStockActual.Size = new Size(244, 25);
            lblStockActual.TabIndex = 4;
            lblStockActual.Text = "Stock Actual en Sistema: -";
            // 
            // lblNuevoStock
            // 
            lblNuevoStock.AutoSize = true;
            lblNuevoStock.Location = new Point(20, 175);
            lblNuevoStock.Name = "lblNuevoStock";
            lblNuevoStock.Size = new Size(176, 20);
            lblNuevoStock.TabIndex = 5;
            lblNuevoStock.Text = "Nuevo Stock Real Físico (*):";
            // 
            // txtNuevoStock
            // 
            txtNuevoStock.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            txtNuevoStock.Location = new Point(20, 200);
            txtNuevoStock.Name = "txtNuevoStock";
            txtNuevoStock.Size = new Size(200, 34);
            txtNuevoStock.TabIndex = 6;
            // 
            // lblDiferencia
            // 
            lblDiferencia.AutoSize = true;
            lblDiferencia.Font = new Font("Segoe UI", 10F, FontStyle.Italic, GraphicsUnit.Point);
            lblDiferencia.Location = new Point(240, 208);
            lblDiferencia.Name = "lblDiferencia";
            lblDiferencia.Size = new Size(111, 23);
            lblDiferencia.TabIndex = 7;
            lblDiferencia.Text = "Diferencia: -";
            // 
            // lblMotivo
            // 
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(20, 255);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(357, 20);
            lblMotivo.TabIndex = 8;
            lblMotivo.Text = "Motivo del Ajuste (*) (conteo, rotura, vencimiento...):";
            // 
            // txtMotivo
            // 
            txtMotivo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtMotivo.Location = new Point(20, 280);
            txtMotivo.Name = "txtMotivo";
            txtMotivo.PlaceholderText = "Ej: Rotura en depósito, Mercadería vencida, Conteo de inventario...";
            txtMotivo.Size = new Size(440, 27);
            txtMotivo.TabIndex = 9;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.ForestGreen;
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(20, 335);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(230, 45);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "APLICAR AJUSTE";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(270, 335);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(190, 45);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar (Esc)";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // FrmAjusteStock
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(485, 405);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtMotivo);
            Controls.Add(lblMotivo);
            Controls.Add(lblDiferencia);
            Controls.Add(txtNuevoStock);
            Controls.Add(lblNuevoStock);
            Controls.Add(lblStockActual);
            Controls.Add(btnBuscarProducto);
            Controls.Add(txtProducto);
            Controls.Add(lblProducto);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAjusteStock";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ajuste de Stock";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblProducto;
        private TextBox txtProducto;
        private Button btnBuscarProducto;
        private Label lblStockActual;
        private Label lblNuevoStock;
        private TextBox txtNuevoStock;
        private Label lblDiferencia;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
