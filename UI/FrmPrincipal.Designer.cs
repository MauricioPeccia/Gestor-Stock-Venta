namespace UI
{
    partial class FrmPrincipal
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
            pnlHeader = new Panel();
            lblSubtitulo = new Label();
            lblTituloPrincipal = new Label();
            pnlEstadoCaja = new Panel();
            lblEstadoCaja = new Label();
            btnVender = new Button();
            btnCaja = new Button();
            btnProductos = new Button();
            btnAjusteStock = new Button();
            btnHistorial = new Button();
            btnCompras = new Button();
            btnCategorias = new Button();
            btnProveedores = new Button();
            btnReportes = new Button();
            lblFooter = new Label();
            pnlHeader.SuspendLayout();
            pnlEstadoCaja.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(15, 23, 42);
            pnlHeader.Controls.Add(lblSubtitulo);
            pnlHeader.Controls.Add(lblTituloPrincipal);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1084, 75);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitulo.Location = new Point(32, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(387, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Sistema de Gestión Comercial y Control Integral de Stock";
            // 
            // lblTituloPrincipal
            // 
            lblTituloPrincipal.AutoSize = true;
            lblTituloPrincipal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTituloPrincipal.ForeColor = Color.White;
            lblTituloPrincipal.Location = new Point(30, 10);
            lblTituloPrincipal.Name = "lblTituloPrincipal";
            lblTituloPrincipal.Size = new Size(466, 41);
            lblTituloPrincipal.TabIndex = 0;
            lblTituloPrincipal.Text = "GESTOR KIOSCO — POS & STOCK";
            // 
            // pnlEstadoCaja
            // 
            pnlEstadoCaja.BackColor = Color.White;
            pnlEstadoCaja.BorderStyle = BorderStyle.FixedSingle;
            pnlEstadoCaja.Controls.Add(lblEstadoCaja);
            pnlEstadoCaja.Location = new Point(32, 90);
            pnlEstadoCaja.Name = "pnlEstadoCaja";
            pnlEstadoCaja.Size = new Size(1020, 48);
            pnlEstadoCaja.TabIndex = 1;
            // 
            // lblEstadoCaja
            // 
            lblEstadoCaja.AutoSize = true;
            lblEstadoCaja.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEstadoCaja.ForeColor = Color.FromArgb(16, 149, 104);
            lblEstadoCaja.Location = new Point(15, 10);
            lblEstadoCaja.Name = "lblEstadoCaja";
            lblEstadoCaja.Size = new Size(223, 25);
            lblEstadoCaja.TabIndex = 0;
            lblEstadoCaja.Text = "● Estado de la jornada...";
            // 
            // btnVender
            // 
            btnVender.BackColor = Color.FromArgb(16, 149, 104);
            btnVender.Cursor = Cursors.Hand;
            btnVender.FlatAppearance.BorderSize = 0;
            btnVender.FlatStyle = FlatStyle.Flat;
            btnVender.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            btnVender.ForeColor = Color.White;
            btnVender.Location = new Point(32, 155);
            btnVender.Name = "btnVender";
            btnVender.Size = new Size(320, 115);
            btnVender.TabIndex = 2;
            btnVender.Text = "\U0001f6d2 NUEVA VENTA (F1)\r\nCobro rápido POS";
            btnVender.UseVisualStyleBackColor = false;
            // 
            // btnCaja
            // 
            btnCaja.BackColor = Color.FromArgb(30, 41, 59);
            btnCaja.Cursor = Cursors.Hand;
            btnCaja.FlatAppearance.BorderSize = 0;
            btnCaja.FlatStyle = FlatStyle.Flat;
            btnCaja.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnCaja.ForeColor = Color.White;
            btnCaja.Location = new Point(382, 155);
            btnCaja.Name = "btnCaja";
            btnCaja.Size = new Size(320, 115);
            btnCaja.TabIndex = 3;
            btnCaja.Text = "💵 CONTROL CAJA (F2)\r\nApertura, Arqueo y Mov.";
            btnCaja.UseVisualStyleBackColor = false;
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(30, 41, 59);
            btnProductos.Cursor = Cursors.Hand;
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnProductos.ForeColor = Color.White;
            btnProductos.Location = new Point(732, 155);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(320, 115);
            btnProductos.TabIndex = 4;
            btnProductos.Text = "📦 PRODUCTOS (F3)\r\nCatálogo, Precios y Stock";
            btnProductos.UseVisualStyleBackColor = false;
            // 
            // btnAjusteStock
            // 
            btnAjusteStock.BackColor = Color.FromArgb(217, 119, 6);
            btnAjusteStock.Cursor = Cursors.Hand;
            btnAjusteStock.FlatAppearance.BorderSize = 0;
            btnAjusteStock.FlatStyle = FlatStyle.Flat;
            btnAjusteStock.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnAjusteStock.ForeColor = Color.White;
            btnAjusteStock.Location = new Point(32, 285);
            btnAjusteStock.Name = "btnAjusteStock";
            btnAjusteStock.Size = new Size(320, 115);
            btnAjusteStock.TabIndex = 5;
            btnAjusteStock.Text = "⚖️ AJUSTE STOCK (F4)\r\nMermas e Inventario";
            btnAjusteStock.UseVisualStyleBackColor = false;
            // 
            // btnHistorial
            // 
            btnHistorial.BackColor = Color.FromArgb(51, 65, 85);
            btnHistorial.Cursor = Cursors.Hand;
            btnHistorial.FlatAppearance.BorderSize = 0;
            btnHistorial.FlatStyle = FlatStyle.Flat;
            btnHistorial.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnHistorial.ForeColor = Color.White;
            btnHistorial.Location = new Point(382, 285);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(320, 115);
            btnHistorial.TabIndex = 6;
            btnHistorial.Text = "📋 HISTORIAL (F5)\r\nVentas y Anulaciones";
            btnHistorial.UseVisualStyleBackColor = false;
            // 
            // btnCompras
            // 
            btnCompras.BackColor = Color.FromArgb(79, 70, 229);
            btnCompras.Cursor = Cursors.Hand;
            btnCompras.FlatAppearance.BorderSize = 0;
            btnCompras.FlatStyle = FlatStyle.Flat;
            btnCompras.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnCompras.ForeColor = Color.White;
            btnCompras.Location = new Point(732, 285);
            btnCompras.Name = "btnCompras";
            btnCompras.Size = new Size(320, 115);
            btnCompras.TabIndex = 7;
            btnCompras.Text = "📥 COMPRAS (F6)\r\nIngreso de Mercadería";
            btnCompras.UseVisualStyleBackColor = false;
            // 
            // btnCategorias
            // 
            btnCategorias.BackColor = Color.FromArgb(71, 85, 105);
            btnCategorias.Cursor = Cursors.Hand;
            btnCategorias.FlatAppearance.BorderSize = 0;
            btnCategorias.FlatStyle = FlatStyle.Flat;
            btnCategorias.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            btnCategorias.ForeColor = Color.White;
            btnCategorias.Location = new Point(32, 415);
            btnCategorias.Name = "btnCategorias";
            btnCategorias.Size = new Size(320, 85);
            btnCategorias.TabIndex = 8;
            btnCategorias.Text = "🏷️ CATEGORÍAS (F7)";
            btnCategorias.UseVisualStyleBackColor = false;
            // 
            // btnProveedores
            // 
            btnProveedores.BackColor = Color.FromArgb(71, 85, 105);
            btnProveedores.Cursor = Cursors.Hand;
            btnProveedores.FlatAppearance.BorderSize = 0;
            btnProveedores.FlatStyle = FlatStyle.Flat;
            btnProveedores.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            btnProveedores.ForeColor = Color.White;
            btnProveedores.Location = new Point(382, 415);
            btnProveedores.Name = "btnProveedores";
            btnProveedores.Size = new Size(320, 85);
            btnProveedores.TabIndex = 9;
            btnProveedores.Text = "🚚 PROVEEDORES (F8)";
            btnProveedores.UseVisualStyleBackColor = false;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(109, 40, 217);
            btnReportes.Cursor = Cursors.Hand;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(732, 415);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(320, 85);
            btnReportes.TabIndex = 10;
            btnReportes.Text = "📊 REPORTES (F9)\r\nPDF & Estadísticas";
            btnReportes.UseVisualStyleBackColor = false;
            // 
            // lblFooter
            // 
            lblFooter.AutoSize = true;
            lblFooter.Font = new Font("Segoe UI", 9F);
            lblFooter.ForeColor = Color.FromArgb(100, 116, 139);
            lblFooter.Location = new Point(32, 515);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(606, 20);
            lblFooter.TabIndex = 11;
            lblFooter.Text = "Manejo completo por teclado: Presione F1 a F9 para acceder a los módulos | Esc para salir";
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1084, 550);
            Controls.Add(lblFooter);
            Controls.Add(btnReportes);
            Controls.Add(btnProveedores);
            Controls.Add(btnCategorias);
            Controls.Add(btnCompras);
            Controls.Add(btnHistorial);
            Controls.Add(btnAjusteStock);
            Controls.Add(btnProductos);
            Controls.Add(btnCaja);
            Controls.Add(btnVender);
            Controls.Add(pnlEstadoCaja);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestor Kiosco — Sistema Integral";
            Load += FrmPrincipal_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlEstadoCaja.ResumeLayout(false);
            pnlEstadoCaja.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTituloPrincipal;
        private Label lblSubtitulo;
        private Panel pnlEstadoCaja;
        private Label lblEstadoCaja;
        private Button btnVender;
        private Button btnCaja;
        private Button btnProductos;
        private Button btnAjusteStock;
        private Button btnHistorial;
        private Button btnCompras;
        private Button btnCategorias;
        private Button btnProveedores;
        private Button btnReportes;
        private Label lblFooter;
    }
}

