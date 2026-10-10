namespace Kiosco.UI;

partial class FrmVenta
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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        panelSuperior = new Panel();
        lblAyudaCodigo = new Label();
        txtCodigo = new TextBox();
        lblCodigo = new Label();
        panelInferior = new Panel();
        btnBuscar = new Button();
        btnCancelarVenta = new Button();
        btnQuitarItem = new Button();
        btnCobrar = new Button();
        panelTotal = new Panel();
        lblTotalTitulo = new Label();
        lblTotal = new Label();
        dgvItems = new DataGridView();
        colNombre = new DataGridViewTextBoxColumn();
        colUnidad = new DataGridViewTextBoxColumn();
        colCantidad = new DataGridViewTextBoxColumn();
        colPrecioUnitario = new DataGridViewTextBoxColumn();
        colSubtotal = new DataGridViewTextBoxColumn();
        panelSuperior.SuspendLayout();
        panelInferior.SuspendLayout();
        panelTotal.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
        SuspendLayout();
        // 
        // panelSuperior
        // 
        panelSuperior.BackColor = Color.FromArgb(248, 249, 250);
        panelSuperior.Controls.Add(lblAyudaCodigo);
        panelSuperior.Controls.Add(txtCodigo);
        panelSuperior.Controls.Add(lblCodigo);
        panelSuperior.Dock = DockStyle.Top;
        panelSuperior.Location = new Point(0, 0);
        panelSuperior.Name = "panelSuperior";
        panelSuperior.Padding = new Padding(20, 15, 20, 15);
        panelSuperior.Size = new Size(1008, 90);
        panelSuperior.TabIndex = 0;
        // 
        // lblAyudaCodigo
        // 
        lblAyudaCodigo.AutoSize = true;
        lblAyudaCodigo.Font = new Font("Segoe UI", 8.5F);
        lblAyudaCodigo.ForeColor = Color.DimGray;
        lblAyudaCodigo.Location = new Point(20, 60);
        lblAyudaCodigo.Name = "lblAyudaCodigo";
        lblAyudaCodigo.Size = new Size(540, 20);
        lblAyudaCodigo.TabIndex = 2;
        lblAyudaCodigo.Text = "Presione Enter para agregar. Para múltiples unidades puede escribir '3*código'. F2: Buscar";
        // 
        // txtCodigo
        // 
        txtCodigo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtCodigo.Font = new Font("Segoe UI", 14F);
        txtCodigo.Location = new Point(20, 20);
        txtCodigo.Name = "txtCodigo";
        txtCodigo.PlaceholderText = "Escanee o ingrese el código de barras y presione Enter...";
        txtCodigo.Size = new Size(968, 39);
        txtCodigo.TabIndex = 0;
        txtCodigo.KeyDown += txtCodigo_KeyDown;
        // 
        // lblCodigo
        // 
        lblCodigo.AutoSize = true;
        lblCodigo.Location = new Point(0, 0);
        lblCodigo.Name = "lblCodigo";
        lblCodigo.Size = new Size(0, 20);
        lblCodigo.TabIndex = 1;
        // 
        // panelInferior
        // 
        panelInferior.BackColor = Color.FromArgb(248, 249, 250);
        panelInferior.Controls.Add(btnBuscar);
        panelInferior.Controls.Add(btnCancelarVenta);
        panelInferior.Controls.Add(btnQuitarItem);
        panelInferior.Controls.Add(btnCobrar);
        panelInferior.Controls.Add(panelTotal);
        panelInferior.Dock = DockStyle.Bottom;
        panelInferior.Location = new Point(0, 580);
        panelInferior.Name = "panelInferior";
        panelInferior.Padding = new Padding(20);
        panelInferior.Size = new Size(1008, 140);
        panelInferior.TabIndex = 1;
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.FromArgb(108, 117, 125);
        btnBuscar.FlatAppearance.BorderSize = 0;
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnBuscar.ForeColor = Color.White;
        btnBuscar.Location = new Point(20, 20);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(150, 48);
        btnBuscar.TabIndex = 4;
        btnBuscar.Text = "Buscar (F2)";
        btnBuscar.UseVisualStyleBackColor = false;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // btnCancelarVenta
        // 
        btnCancelarVenta.BackColor = Color.FromArgb(220, 53, 69);
        btnCancelarVenta.FlatAppearance.BorderSize = 0;
        btnCancelarVenta.FlatStyle = FlatStyle.Flat;
        btnCancelarVenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCancelarVenta.ForeColor = Color.White;
        btnCancelarVenta.Location = new Point(180, 20);
        btnCancelarVenta.Name = "btnCancelarVenta";
        btnCancelarVenta.Size = new Size(160, 48);
        btnCancelarVenta.TabIndex = 3;
        btnCancelarVenta.Text = "Cancelar (Esc)";
        btnCancelarVenta.UseVisualStyleBackColor = false;
        btnCancelarVenta.Click += btnCancelarVenta_Click;
        // 
        // btnQuitarItem
        // 
        btnQuitarItem.BackColor = Color.FromArgb(255, 193, 7);
        btnQuitarItem.FlatAppearance.BorderSize = 0;
        btnQuitarItem.FlatStyle = FlatStyle.Flat;
        btnQuitarItem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnQuitarItem.ForeColor = Color.Black;
        btnQuitarItem.Location = new Point(350, 20);
        btnQuitarItem.Name = "btnQuitarItem";
        btnQuitarItem.Size = new Size(160, 48);
        btnQuitarItem.TabIndex = 2;
        btnQuitarItem.Text = "Quitar Ítem (Supr)";
        btnQuitarItem.UseVisualStyleBackColor = false;
        btnQuitarItem.Click += btnQuitarItem_Click;
        // 
        // btnCobrar
        // 
        btnCobrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCobrar.BackColor = Color.FromArgb(40, 167, 69);
        btnCobrar.FlatAppearance.BorderSize = 0;
        btnCobrar.FlatStyle = FlatStyle.Flat;
        btnCobrar.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        btnCobrar.ForeColor = Color.White;
        btnCobrar.Location = new Point(748, 20);
        btnCobrar.Name = "btnCobrar";
        btnCobrar.Size = new Size(240, 100);
        btnCobrar.TabIndex = 1;
        btnCobrar.Text = "COBRAR\r\n(F5)";
        btnCobrar.UseVisualStyleBackColor = false;
        btnCobrar.Click += btnCobrar_Click;
        // 
        // panelTotal
        // 
        panelTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        panelTotal.BackColor = Color.FromArgb(233, 236, 239);
        panelTotal.BorderStyle = BorderStyle.FixedSingle;
        panelTotal.Controls.Add(lblTotalTitulo);
        panelTotal.Controls.Add(lblTotal);
        panelTotal.Location = new Point(530, 20);
        panelTotal.Name = "panelTotal";
        panelTotal.Size = new Size(205, 100);
        panelTotal.TabIndex = 0;
        // 
        // lblTotalTitulo
        // 
        lblTotalTitulo.AutoSize = true;
        lblTotalTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotalTitulo.ForeColor = Color.DimGray;
        lblTotalTitulo.Location = new Point(12, 10);
        lblTotalTitulo.Name = "lblTotalTitulo";
        lblTotalTitulo.Size = new Size(58, 20);
        lblTotalTitulo.TabIndex = 0;
        lblTotalTitulo.Text = "TOTAL:";
        // 
        // lblTotal
        // 
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTotal.ForeColor = Color.DarkGreen;
        lblTotal.Location = new Point(10, 35);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(107, 46);
        lblTotal.TabIndex = 1;
        lblTotal.Text = "$ 0,00";
        // 
        // dgvItems
        // 
        dgvItems.AllowUserToAddRows = false;
        dgvItems.AllowUserToDeleteRows = false;
        dgvItems.AllowUserToResizeRows = false;
        dgvItems.BackgroundColor = Color.White;
        dgvItems.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 240, 240);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvItems.ColumnHeadersHeight = 40;
        dgvItems.Columns.AddRange(new DataGridViewColumn[] { colNombre, colUnidad, colCantidad, colPrecioUnitario, colSubtotal });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(13, 110, 253);
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvItems.DefaultCellStyle = dataGridViewCellStyle2;
        dgvItems.Dock = DockStyle.Fill;
        dgvItems.Location = new Point(0, 90);
        dgvItems.MultiSelect = false;
        dgvItems.Name = "dgvItems";
        dgvItems.ReadOnly = true;
        dgvItems.RowHeadersVisible = false;
        dgvItems.RowHeadersWidth = 51;
        dgvItems.RowTemplate.Height = 35;
        dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvItems.Size = new Size(1008, 490);
        dgvItems.TabIndex = 2;
        // 
        // colNombre
        // 
        colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colNombre.DataPropertyName = "Nombre";
        colNombre.HeaderText = "Producto";
        colNombre.MinimumWidth = 200;
        colNombre.Name = "colNombre";
        colNombre.ReadOnly = true;
        // 
        // colUnidad
        // 
        colUnidad.DataPropertyName = "Unidad";
        colUnidad.HeaderText = "Unidad";
        colUnidad.MinimumWidth = 70;
        colUnidad.Name = "colUnidad";
        colUnidad.ReadOnly = true;
        colUnidad.Width = 80;
        // 
        // colCantidad
        // 
        colCantidad.DataPropertyName = "Cantidad";
        dataGridViewCellStyle3.Format = "N3";
        colCantidad.DefaultCellStyle = dataGridViewCellStyle3;
        colCantidad.HeaderText = "Cantidad";
        colCantidad.MinimumWidth = 100;
        colCantidad.Name = "colCantidad";
        colCantidad.ReadOnly = true;
        colCantidad.Width = 110;
        // 
        // colPrecioUnitario
        // 
        colPrecioUnitario.DataPropertyName = "PrecioUnitario";
        colPrecioUnitario.HeaderText = "Precio Unit.";
        colPrecioUnitario.MinimumWidth = 120;
        colPrecioUnitario.Name = "colPrecioUnitario";
        colPrecioUnitario.ReadOnly = true;
        colPrecioUnitario.Width = 130;
        // 
        // colSubtotal
        // 
        colSubtotal.DataPropertyName = "Subtotal";
        colSubtotal.HeaderText = "Subtotal";
        colSubtotal.MinimumWidth = 130;
        colSubtotal.Name = "colSubtotal";
        colSubtotal.ReadOnly = true;
        colSubtotal.Width = 140;
        // 
        // FrmVenta
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1008, 720);
        Controls.Add(dgvItems);
        Controls.Add(panelInferior);
        Controls.Add(panelSuperior);
        KeyPreview = true;
        MinimumSize = new Size(850, 600);
        Name = "FrmVenta";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Punto de Venta";
        Load += FrmVenta_Load;
        KeyDown += FrmVenta_KeyDown;
        panelSuperior.ResumeLayout(false);
        panelSuperior.PerformLayout();
        panelInferior.ResumeLayout(false);
        panelTotal.ResumeLayout(false);
        panelTotal.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
        ResumeLayout(false);
    }

    private Panel panelSuperior;
    private TextBox txtCodigo;
    private Label lblCodigo;
    private Label lblAyudaCodigo;
    private Panel panelInferior;
    private Panel panelTotal;
    private Label lblTotalTitulo;
    private Label lblTotal;
    private Button btnCobrar;
    private Button btnQuitarItem;
    private Button btnCancelarVenta;
    private Button btnBuscar;
    private DataGridView dgvItems;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colUnidad;
    private DataGridViewTextBoxColumn colCantidad;
    private DataGridViewTextBoxColumn colPrecioUnitario;
    private DataGridViewTextBoxColumn colSubtotal;
}

