namespace Kiosco.UI;

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

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        panelSuperior = new Panel();
        btnAumento = new Button();
        btnDesactivar = new Button();
        btnEditar = new Button();
        btnNuevo = new Button();
        txtBuscar = new TextBox();
        lblBuscar = new Label();
        panelInferior = new Panel();
        lblTotalProductos = new Label();
        dgvProductos = new DataGridView();
        colCodigo = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colCategoria = new DataGridViewTextBoxColumn();
        colUnidad = new DataGridViewTextBoxColumn();
        colPrecioCosto = new DataGridViewTextBoxColumn();
        colPrecioVenta = new DataGridViewTextBoxColumn();
        colStockActual = new DataGridViewTextBoxColumn();
        colStockMinimo = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        panelSuperior.SuspendLayout();
        panelInferior.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
        SuspendLayout();
        // 
        // panelSuperior
        // 
        panelSuperior.BackColor = Color.FromArgb(248, 249, 250);
        panelSuperior.Controls.Add(btnAumento);
        panelSuperior.Controls.Add(btnDesactivar);
        panelSuperior.Controls.Add(btnEditar);
        panelSuperior.Controls.Add(btnNuevo);
        panelSuperior.Controls.Add(txtBuscar);
        panelSuperior.Controls.Add(lblBuscar);
        panelSuperior.Dock = DockStyle.Top;
        panelSuperior.Location = new Point(0, 0);
        panelSuperior.Name = "panelSuperior";
        panelSuperior.Padding = new Padding(20, 15, 20, 15);
        panelSuperior.Size = new Size(1100, 95);
        panelSuperior.TabIndex = 0;
        // 
        // btnAumento
        // 
        btnAumento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAumento.BackColor = Color.FromArgb(253, 126, 20);
        btnAumento.FlatAppearance.BorderSize = 0;
        btnAumento.FlatStyle = FlatStyle.Flat;
        btnAumento.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAumento.ForeColor = Color.White;
        btnAumento.Location = new Point(900, 42);
        btnAumento.Name = "btnAumento";
        btnAumento.Size = new Size(180, 38);
        btnAumento.TabIndex = 5;
        btnAumento.Text = "Aumento Masivo";
        btnAumento.UseVisualStyleBackColor = false;
        btnAumento.Click += btnAumento_Click;
        // 
        // btnDesactivar
        // 
        btnDesactivar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDesactivar.BackColor = Color.FromArgb(220, 53, 69);
        btnDesactivar.FlatAppearance.BorderSize = 0;
        btnDesactivar.FlatStyle = FlatStyle.Flat;
        btnDesactivar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnDesactivar.ForeColor = Color.White;
        btnDesactivar.Location = new Point(765, 42);
        btnDesactivar.Name = "btnDesactivar";
        btnDesactivar.Size = new Size(125, 38);
        btnDesactivar.TabIndex = 4;
        btnDesactivar.Text = "Desactivar";
        btnDesactivar.UseVisualStyleBackColor = false;
        btnDesactivar.Click += btnDesactivar_Click;
        // 
        // btnEditar
        // 
        btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEditar.BackColor = Color.FromArgb(23, 162, 184);
        btnEditar.FlatAppearance.BorderSize = 0;
        btnEditar.FlatStyle = FlatStyle.Flat;
        btnEditar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnEditar.ForeColor = Color.White;
        btnEditar.Location = new Point(630, 42);
        btnEditar.Name = "btnEditar";
        btnEditar.Size = new Size(125, 38);
        btnEditar.TabIndex = 3;
        btnEditar.Text = "Editar";
        btnEditar.UseVisualStyleBackColor = false;
        btnEditar.Click += btnEditar_Click;
        // 
        // btnNuevo
        // 
        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.BackColor = Color.FromArgb(40, 167, 69);
        btnNuevo.FlatAppearance.BorderSize = 0;
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnNuevo.ForeColor = Color.White;
        btnNuevo.Location = new Point(495, 42);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(125, 38);
        btnNuevo.TabIndex = 2;
        btnNuevo.Text = "+ Nuevo";
        btnNuevo.UseVisualStyleBackColor = false;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // txtBuscar
        // 
        txtBuscar.Font = new Font("Segoe UI", 11F);
        txtBuscar.Location = new Point(20, 44);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.PlaceholderText = "Escriba para buscar por nombre o código en tiempo real...";
        txtBuscar.Size = new Size(450, 32);
        txtBuscar.TabIndex = 1;
        txtBuscar.TextChanged += txtBuscar_TextChanged;
        txtBuscar.KeyDown += txtBuscar_KeyDown;
        // 
        // lblBuscar
        // 
        lblBuscar.AutoSize = true;
        lblBuscar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblBuscar.Location = new Point(20, 18);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(137, 21);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Búsqueda rápida:";
        // 
        // panelInferior
        // 
        panelInferior.BackColor = Color.FromArgb(248, 249, 250);
        panelInferior.Controls.Add(lblTotalProductos);
        panelInferior.Dock = DockStyle.Bottom;
        panelInferior.Location = new Point(0, 580);
        panelInferior.Name = "panelInferior";
        panelInferior.Padding = new Padding(20, 10, 20, 10);
        panelInferior.Size = new Size(1100, 40);
        panelInferior.TabIndex = 1;
        // 
        // lblTotalProductos
        // 
        lblTotalProductos.AutoSize = true;
        lblTotalProductos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotalProductos.ForeColor = Color.DimGray;
        lblTotalProductos.Location = new Point(20, 10);
        lblTotalProductos.Name = "lblTotalProductos";
        lblTotalProductos.Size = new Size(157, 20);
        lblTotalProductos.TabIndex = 0;
        lblTotalProductos.Text = "Cargando catálogo...";
        // 
        // dgvProductos
        // 
        dgvProductos.AllowUserToAddRows = false;
        dgvProductos.AllowUserToDeleteRows = false;
        dgvProductos.AllowUserToResizeRows = false;
        dgvProductos.BackgroundColor = Color.White;
        dgvProductos.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 240, 240);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvProductos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvProductos.ColumnHeadersHeight = 35;
        dgvProductos.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colCategoria, colUnidad, colPrecioCosto, colPrecioVenta, colStockActual, colStockMinimo, colEstado });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(13, 110, 253);
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvProductos.DefaultCellStyle = dataGridViewCellStyle2;
        dgvProductos.Dock = DockStyle.Fill;
        dgvProductos.Location = new Point(0, 95);
        dgvProductos.MultiSelect = false;
        dgvProductos.Name = "dgvProductos";
        dgvProductos.ReadOnly = true;
        dgvProductos.RowHeadersVisible = false;
        dgvProductos.RowHeadersWidth = 51;
        dgvProductos.RowTemplate.Height = 30;
        dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProductos.Size = new Size(1100, 485);
        dgvProductos.TabIndex = 2;
        dgvProductos.CellDoubleClick += dgvProductos_CellDoubleClick;
        dgvProductos.CellFormatting += dgvProductos_CellFormatting;
        // 
        // colCodigo
        // 
        colCodigo.DataPropertyName = "Codigo";
        colCodigo.HeaderText = "Código";
        colCodigo.MinimumWidth = 110;
        colCodigo.Name = "colCodigo";
        colCodigo.ReadOnly = true;
        colCodigo.Width = 130;
        // 
        // colNombre
        // 
        colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colNombre.DataPropertyName = "Nombre";
        colNombre.HeaderText = "Nombre";
        colNombre.MinimumWidth = 200;
        colNombre.Name = "colNombre";
        colNombre.ReadOnly = true;
        // 
        // colCategoria
        // 
        colCategoria.HeaderText = "Categoría";
        colCategoria.MinimumWidth = 120;
        colCategoria.Name = "colCategoria";
        colCategoria.ReadOnly = true;
        colCategoria.Width = 140;
        // 
        // colUnidad
        // 
        colUnidad.DataPropertyName = "Unidad";
        colUnidad.HeaderText = "Unidad";
        colUnidad.MinimumWidth = 60;
        colUnidad.Name = "colUnidad";
        colUnidad.ReadOnly = true;
        colUnidad.Width = 70;
        // 
        // colPrecioCosto
        // 
        colPrecioCosto.DataPropertyName = "PrecioCosto";
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle3.Format = "C2";
        colPrecioCosto.DefaultCellStyle = dataGridViewCellStyle3;
        colPrecioCosto.HeaderText = "Costo";
        colPrecioCosto.MinimumWidth = 90;
        colPrecioCosto.Name = "colPrecioCosto";
        colPrecioCosto.ReadOnly = true;
        colPrecioCosto.Width = 100;
        // 
        // colPrecioVenta
        // 
        colPrecioVenta.DataPropertyName = "PrecioVenta";
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle4.Format = "C2";
        colPrecioVenta.DefaultCellStyle = dataGridViewCellStyle4;
        colPrecioVenta.HeaderText = "Precio Venta";
        colPrecioVenta.MinimumWidth = 100;
        colPrecioVenta.Name = "colPrecioVenta";
        colPrecioVenta.ReadOnly = true;
        colPrecioVenta.Width = 110;
        // 
        // colStockActual
        // 
        colStockActual.DataPropertyName = "StockActual";
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle5.Format = "N3";
        colStockActual.DefaultCellStyle = dataGridViewCellStyle5;
        colStockActual.HeaderText = "Stock Actual";
        colStockActual.MinimumWidth = 90;
        colStockActual.Name = "colStockActual";
        colStockActual.ReadOnly = true;
        colStockActual.Width = 100;
        // 
        // colStockMinimo
        // 
        colStockMinimo.DataPropertyName = "StockMinimo";
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle6.Format = "N3";
        colStockMinimo.DefaultCellStyle = dataGridViewCellStyle6;
        colStockMinimo.HeaderText = "Stock Mín.";
        colStockMinimo.MinimumWidth = 90;
        colStockMinimo.Name = "colStockMinimo";
        colStockMinimo.ReadOnly = true;
        colStockMinimo.Width = 100;
        // 
        // colEstado
        // 
        colEstado.HeaderText = "Estado";
        colEstado.MinimumWidth = 70;
        colEstado.Name = "colEstado";
        colEstado.ReadOnly = true;
        colEstado.Width = 80;
        // 
        // FrmProductos
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 620);
        Controls.Add(dgvProductos);
        Controls.Add(panelInferior);
        Controls.Add(panelSuperior);
        MinimumSize = new Size(900, 500);
        Name = "FrmProductos";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Catálogo de Productos y Precios";
        Load += FrmProductos_Load;
        panelSuperior.ResumeLayout(false);
        panelSuperior.PerformLayout();
        panelInferior.ResumeLayout(false);
        panelInferior.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
        ResumeLayout(false);
    }

    private Panel panelSuperior;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Button btnNuevo;
    private Button btnEditar;
    private Button btnDesactivar;
    private Button btnAumento;
    private Panel panelInferior;
    private Label lblTotalProductos;
    private DataGridView dgvProductos;
    private DataGridViewTextBoxColumn colCodigo;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colCategoria;
    private DataGridViewTextBoxColumn colUnidad;
    private DataGridViewTextBoxColumn colPrecioCosto;
    private DataGridViewTextBoxColumn colPrecioVenta;
    private DataGridViewTextBoxColumn colStockActual;
    private DataGridViewTextBoxColumn colStockMinimo;
    private DataGridViewTextBoxColumn colEstado;
}

