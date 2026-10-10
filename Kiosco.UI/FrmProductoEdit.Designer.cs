namespace Kiosco.UI;

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

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        lblCodigo = new Label();
        txtCodigo = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblCategoria = new Label();
        cboCategoria = new ComboBox();
        lblUnidad = new Label();
        cboUnidad = new ComboBox();
        lblPrecioCosto = new Label();
        numPrecioCosto = new NumericUpDown();
        lblPrecioVenta = new Label();
        numPrecioVenta = new NumericUpDown();
        lblStockMinimo = new Label();
        numStockMinimo = new NumericUpDown();
        lblStockInicial = new Label();
        numStockInicial = new NumericUpDown();
        lblAyudaStock = new Label();
        btnGuardar = new Button();
        btnCancelar = new Button();
        ((System.ComponentModel.ISupportInitialize)numPrecioCosto).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numPrecioVenta).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numStockMinimo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numStockInicial).BeginInit();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.Location = new Point(25, 20);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(195, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Nuevo Producto";
        // 
        // lblCodigo
        // 
        lblCodigo.AutoSize = true;
        lblCodigo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblCodigo.Location = new Point(25, 65);
        lblCodigo.Name = "lblCodigo";
        lblCodigo.Size = new Size(193, 21);
        lblCodigo.TabIndex = 1;
        lblCodigo.Text = "Código de barras (opc.):";
        // 
        // txtCodigo
        // 
        txtCodigo.Font = new Font("Segoe UI", 10.5F);
        txtCodigo.Location = new Point(25, 90);
        txtCodigo.Name = "txtCodigo";
        txtCodigo.PlaceholderText = "Dejar vacío si no posee código";
        txtCodigo.Size = new Size(420, 31);
        txtCodigo.TabIndex = 2;
        // 
        // lblNombre
        // 
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblNombre.Location = new Point(25, 130);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(95, 21);
        lblNombre.TabIndex = 3;
        lblNombre.Text = "Nombre (*):";
        // 
        // txtNombre
        // 
        txtNombre.Font = new Font("Segoe UI", 10.5F);
        txtNombre.Location = new Point(25, 155);
        txtNombre.Name = "txtNombre";
        txtNombre.PlaceholderText = "Descripción / Nombre del artículo";
        txtNombre.Size = new Size(420, 31);
        txtNombre.TabIndex = 4;
        // 
        // lblCategoria
        // 
        lblCategoria.AutoSize = true;
        lblCategoria.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblCategoria.Location = new Point(25, 195);
        lblCategoria.Name = "lblCategoria";
        lblCategoria.Size = new Size(88, 21);
        lblCategoria.TabIndex = 5;
        lblCategoria.Text = "Categoría:";
        // 
        // cboCategoria
        // 
        cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategoria.Font = new Font("Segoe UI", 10F);
        cboCategoria.FormattingEnabled = true;
        cboCategoria.Location = new Point(25, 220);
        cboCategoria.Name = "cboCategoria";
        cboCategoria.Size = new Size(240, 29);
        cboCategoria.TabIndex = 6;
        // 
        // lblUnidad
        // 
        lblUnidad.AutoSize = true;
        lblUnidad.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblUnidad.Location = new Point(285, 195);
        lblUnidad.Name = "lblUnidad";
        lblUnidad.Size = new Size(69, 21);
        lblUnidad.TabIndex = 7;
        lblUnidad.Text = "Unidad:";
        // 
        // cboUnidad
        // 
        cboUnidad.DropDownStyle = ComboBoxStyle.DropDownList;
        cboUnidad.Font = new Font("Segoe UI", 10F);
        cboUnidad.FormattingEnabled = true;
        cboUnidad.Items.AddRange(new object[] { "UN", "KG", "LT" });
        cboUnidad.Location = new Point(285, 220);
        cboUnidad.Name = "cboUnidad";
        cboUnidad.Size = new Size(160, 29);
        cboUnidad.TabIndex = 8;
        // 
        // lblPrecioCosto
        // 
        lblPrecioCosto.AutoSize = true;
        lblPrecioCosto.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPrecioCosto.Location = new Point(25, 260);
        lblPrecioCosto.Name = "lblPrecioCosto";
        lblPrecioCosto.Size = new Size(130, 21);
        lblPrecioCosto.TabIndex = 9;
        lblPrecioCosto.Text = "Precio de Costo:";
        // 
        // numPrecioCosto
        // 
        numPrecioCosto.DecimalPlaces = 2;
        numPrecioCosto.Font = new Font("Segoe UI", 10.5F);
        numPrecioCosto.Location = new Point(25, 285);
        numPrecioCosto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numPrecioCosto.Name = "numPrecioCosto";
        numPrecioCosto.Size = new Size(200, 31);
        numPrecioCosto.TabIndex = 10;
        numPrecioCosto.ThousandsSeparator = true;
        // 
        // lblPrecioVenta
        // 
        lblPrecioVenta.AutoSize = true;
        lblPrecioVenta.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPrecioVenta.Location = new Point(245, 260);
        lblPrecioVenta.Name = "lblPrecioVenta";
        lblPrecioVenta.Size = new Size(131, 21);
        lblPrecioVenta.TabIndex = 11;
        lblPrecioVenta.Text = "Precio de Venta:";
        // 
        // numPrecioVenta
        // 
        numPrecioVenta.DecimalPlaces = 2;
        numPrecioVenta.Font = new Font("Segoe UI", 10.5F);
        numPrecioVenta.Location = new Point(245, 285);
        numPrecioVenta.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numPrecioVenta.Name = "numPrecioVenta";
        numPrecioVenta.Size = new Size(200, 31);
        numPrecioVenta.TabIndex = 12;
        numPrecioVenta.ThousandsSeparator = true;
        // 
        // lblStockMinimo
        // 
        lblStockMinimo.AutoSize = true;
        lblStockMinimo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblStockMinimo.Location = new Point(25, 325);
        lblStockMinimo.Name = "lblStockMinimo";
        lblStockMinimo.Size = new Size(119, 21);
        lblStockMinimo.TabIndex = 13;
        lblStockMinimo.Text = "Stock Mínimo:";
        // 
        // numStockMinimo
        // 
        numStockMinimo.DecimalPlaces = 3;
        numStockMinimo.Font = new Font("Segoe UI", 10.5F);
        numStockMinimo.Location = new Point(25, 350);
        numStockMinimo.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numStockMinimo.Name = "numStockMinimo";
        numStockMinimo.Size = new Size(200, 31);
        numStockMinimo.TabIndex = 14;
        numStockMinimo.ThousandsSeparator = true;
        // 
        // lblStockInicial
        // 
        lblStockInicial.AutoSize = true;
        lblStockInicial.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblStockInicial.Location = new Point(245, 325);
        lblStockInicial.Name = "lblStockInicial";
        lblStockInicial.Size = new Size(107, 21);
        lblStockInicial.TabIndex = 15;
        lblStockInicial.Text = "Stock Inicial:";
        // 
        // numStockInicial
        // 
        numStockInicial.DecimalPlaces = 3;
        numStockInicial.Font = new Font("Segoe UI", 10.5F);
        numStockInicial.Location = new Point(245, 350);
        numStockInicial.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numStockInicial.Name = "numStockInicial";
        numStockInicial.Size = new Size(200, 31);
        numStockInicial.TabIndex = 16;
        numStockInicial.ThousandsSeparator = true;
        // 
        // lblAyudaStock
        // 
        lblAyudaStock.AutoSize = true;
        lblAyudaStock.Font = new Font("Segoe UI", 8F);
        lblAyudaStock.ForeColor = Color.DimGray;
        lblAyudaStock.Location = new Point(245, 384);
        lblAyudaStock.Name = "lblAyudaStock";
        lblAyudaStock.Size = new Size(200, 19);
        lblAyudaStock.TabIndex = 17;
        lblAyudaStock.Text = "Solo configurable en alta inicial";
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        btnGuardar.ForeColor = Color.White;
        btnGuardar.Location = new Point(195, 425);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(120, 42);
        btnGuardar.TabIndex = 18;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Font = new Font("Segoe UI", 10.5F);
        btnCancelar.Location = new Point(325, 425);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(120, 42);
        btnCancelar.TabIndex = 19;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FrmProductoEdit
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(470, 490);
        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);
        Controls.Add(lblAyudaStock);
        Controls.Add(numStockInicial);
        Controls.Add(lblStockInicial);
        Controls.Add(numStockMinimo);
        Controls.Add(lblStockMinimo);
        Controls.Add(numPrecioVenta);
        Controls.Add(lblPrecioVenta);
        Controls.Add(numPrecioCosto);
        Controls.Add(lblPrecioCosto);
        Controls.Add(cboUnidad);
        Controls.Add(lblUnidad);
        Controls.Add(cboCategoria);
        Controls.Add(lblCategoria);
        Controls.Add(txtNombre);
        Controls.Add(lblNombre);
        Controls.Add(txtCodigo);
        Controls.Add(lblCodigo);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmProductoEdit";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Datos de Producto";
        Load += FrmProductoEdit_Load;
        ((System.ComponentModel.ISupportInitialize)numPrecioCosto).EndInit();
        ((System.ComponentModel.ISupportInitialize)numPrecioVenta).EndInit();
        ((System.ComponentModel.ISupportInitialize)numStockMinimo).EndInit();
        ((System.ComponentModel.ISupportInitialize)numStockInicial).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitulo;
    private Label lblCodigo;
    private TextBox txtCodigo;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblCategoria;
    private ComboBox cboCategoria;
    private Label lblUnidad;
    private ComboBox cboUnidad;
    private Label lblPrecioCosto;
    private NumericUpDown numPrecioCosto;
    private Label lblPrecioVenta;
    private NumericUpDown numPrecioVenta;
    private Label lblStockMinimo;
    private NumericUpDown numStockMinimo;
    private Label lblStockInicial;
    private NumericUpDown numStockInicial;
    private Label lblAyudaStock;
    private Button btnGuardar;
    private Button btnCancelar;
}

