namespace Kiosco.UI;

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

    private void InitializeComponent()
    {
        panelHeader = new Panel();
        lblTitulo = new Label();
        lblEstadoCaja = new Label();
        panelAlerta = new Panel();
        lblAlertaStock = new Label();
        panelBotones = new FlowLayoutPanel();
        btnVender = new Button();
        btnCaja = new Button();
        btnProductos = new Button();
        btnCompras = new Button();
        btnReportes = new Button();
        panelHeader.SuspendLayout();
        panelAlerta.SuspendLayout();
        panelBotones.SuspendLayout();
        SuspendLayout();
        // 
        // panelHeader
        // 
        panelHeader.BackColor = Color.FromArgb(33, 37, 41);
        panelHeader.Controls.Add(lblTitulo);
        panelHeader.Controls.Add(lblEstadoCaja);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Location = new Point(0, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Padding = new Padding(20, 15, 20, 15);
        panelHeader.Size = new Size(884, 75);
        panelHeader.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(20, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(245, 37);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Kiosco Drugstore";
        // 
        // lblEstadoCaja
        // 
        lblEstadoCaja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblEstadoCaja.AutoSize = true;
        lblEstadoCaja.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblEstadoCaja.ForeColor = Color.Gold;
        lblEstadoCaja.Location = new Point(600, 24);
        lblEstadoCaja.Name = "lblEstadoCaja";
        lblEstadoCaja.Size = new Size(240, 28);
        lblEstadoCaja.TabIndex = 1;
        lblEstadoCaja.Text = "Verificando estado...";
        lblEstadoCaja.TextAlign = ContentAlignment.MiddleRight;
        // 
        // panelAlerta
        // 
        panelAlerta.BackColor = Color.FromArgb(255, 243, 205);
        panelAlerta.Controls.Add(lblAlertaStock);
        panelAlerta.Dock = DockStyle.Top;
        panelAlerta.Location = new Point(0, 75);
        panelAlerta.Name = "panelAlerta";
        panelAlerta.Padding = new Padding(20, 10, 20, 10);
        panelAlerta.Size = new Size(884, 45);
        panelAlerta.TabIndex = 1;
        panelAlerta.Visible = false;
        // 
        // lblAlertaStock
        // 
        lblAlertaStock.AutoSize = true;
        lblAlertaStock.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblAlertaStock.ForeColor = Color.FromArgb(102, 77, 3);
        lblAlertaStock.Location = new Point(20, 12);
        lblAlertaStock.Name = "lblAlertaStock";
        lblAlertaStock.Size = new Size(286, 21);
        lblAlertaStock.TabIndex = 0;
        lblAlertaStock.Text = "Alerta: Hay productos bajo stock mínimo";
        // 
        // panelBotones
        // 
        panelBotones.Controls.Add(btnVender);
        panelBotones.Controls.Add(btnCaja);
        panelBotones.Controls.Add(btnProductos);
        panelBotones.Controls.Add(btnCompras);
        panelBotones.Controls.Add(btnReportes);
        panelBotones.Dock = DockStyle.Fill;
        panelBotones.Location = new Point(0, 120);
        panelBotones.Name = "panelBotones";
        panelBotones.Padding = new Padding(30);
        panelBotones.Size = new Size(884, 441);
        panelBotones.TabIndex = 2;
        // 
        // btnVender
        // 
        btnVender.BackColor = Color.FromArgb(40, 167, 69);
        btnVender.Cursor = Cursors.Hand;
        btnVender.FlatAppearance.BorderSize = 0;
        btnVender.FlatStyle = FlatStyle.Flat;
        btnVender.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        btnVender.ForeColor = Color.White;
        btnVender.Location = new Point(35, 35);
        btnVender.Margin = new Padding(5);
        btnVender.Name = "btnVender";
        btnVender.Size = new Size(250, 120);
        btnVender.TabIndex = 0;
        btnVender.Text = "Punto de Venta\r\n(F5)";
        btnVender.UseVisualStyleBackColor = false;
        btnVender.Click += btnVender_Click;
        // 
        // btnCaja
        // 
        btnCaja.BackColor = Color.FromArgb(23, 162, 184);
        btnCaja.Cursor = Cursors.Hand;
        btnCaja.FlatAppearance.BorderSize = 0;
        btnCaja.FlatStyle = FlatStyle.Flat;
        btnCaja.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnCaja.ForeColor = Color.White;
        btnCaja.Location = new Point(295, 35);
        btnCaja.Margin = new Padding(5);
        btnCaja.Name = "btnCaja";
        btnCaja.Size = new Size(250, 120);
        btnCaja.TabIndex = 1;
        btnCaja.Text = "Gestión de Caja";
        btnCaja.UseVisualStyleBackColor = false;
        btnCaja.Click += btnCaja_Click;
        // 
        // btnProductos
        // 
        btnProductos.BackColor = Color.FromArgb(108, 117, 125);
        btnProductos.Cursor = Cursors.Hand;
        btnProductos.FlatAppearance.BorderSize = 0;
        btnProductos.FlatStyle = FlatStyle.Flat;
        btnProductos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnProductos.ForeColor = Color.White;
        btnProductos.Location = new Point(555, 35);
        btnProductos.Margin = new Padding(5);
        btnProductos.Name = "btnProductos";
        btnProductos.Size = new Size(250, 120);
        btnProductos.TabIndex = 2;
        btnProductos.Text = "Productos y Precios";
        btnProductos.UseVisualStyleBackColor = false;
        btnProductos.Click += btnProductos_Click;
        // 
        // btnCompras
        // 
        btnCompras.BackColor = Color.FromArgb(111, 66, 193);
        btnCompras.Cursor = Cursors.Hand;
        btnCompras.FlatAppearance.BorderSize = 0;
        btnCompras.FlatStyle = FlatStyle.Flat;
        btnCompras.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnCompras.ForeColor = Color.White;
        btnCompras.Location = new Point(35, 165);
        btnCompras.Margin = new Padding(5);
        btnCompras.Name = "btnCompras";
        btnCompras.Size = new Size(250, 120);
        btnCompras.TabIndex = 3;
        btnCompras.Text = "Ingreso de Mercadería\r\n(Compras)";
        btnCompras.UseVisualStyleBackColor = false;
        btnCompras.Click += btnCompras_Click;
        // 
        // btnReportes
        // 
        btnReportes.BackColor = Color.FromArgb(253, 126, 20);
        btnReportes.Cursor = Cursors.Hand;
        btnReportes.FlatAppearance.BorderSize = 0;
        btnReportes.FlatStyle = FlatStyle.Flat;
        btnReportes.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnReportes.ForeColor = Color.White;
        btnReportes.Location = new Point(295, 165);
        btnReportes.Margin = new Padding(5);
        btnReportes.Name = "btnReportes";
        btnReportes.Size = new Size(250, 120);
        btnReportes.TabIndex = 4;
        btnReportes.Text = "Reportes (PDF)";
        btnReportes.UseVisualStyleBackColor = false;
        btnReportes.Click += btnReportes_Click;
        // 
        // FrmPrincipal
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 561);
        Controls.Add(panelBotones);
        Controls.Add(panelAlerta);
        Controls.Add(panelHeader);
        KeyPreview = true;
        MinimumSize = new Size(800, 500);
        Name = "FrmPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sistema de Gestión - Kiosco";
        Load += FrmPrincipal_Load;
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        panelAlerta.ResumeLayout(false);
        panelAlerta.PerformLayout();
        panelBotones.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel panelHeader;
    private Label lblTitulo;
    private Label lblEstadoCaja;
    private Panel panelAlerta;
    private Label lblAlertaStock;
    private FlowLayoutPanel panelBotones;
    private Button btnVender;
    private Button btnCaja;
    private Button btnProductos;
    private Button btnCompras;
    private Button btnReportes;
}

