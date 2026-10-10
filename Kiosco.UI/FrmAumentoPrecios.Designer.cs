namespace Kiosco.UI;

partial class FrmAumentoPrecios
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
        lblInfo = new Label();
        lblPorcentaje = new Label();
        numPorcentaje = new NumericUpDown();
        lblCategoria = new Label();
        cboCategoria = new ComboBox();
        lblRedondeo = new Label();
        cboRedondeo = new ComboBox();
        btnAplicar = new Button();
        btnCancelar = new Button();
        ((System.ComponentModel.ISupportInitialize)numPorcentaje).BeginInit();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblTitulo.Location = new Point(25, 20);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(295, 30);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Aumento Masivo de Precios";
        // 
        // lblInfo
        // 
        lblInfo.Font = new Font("Segoe UI", 9F);
        lblInfo.ForeColor = Color.DimGray;
        lblInfo.Location = new Point(25, 55);
        lblInfo.Name = "lblInfo";
        lblInfo.Size = new Size(390, 45);
        lblInfo.TabIndex = 1;
        lblInfo.Text = "Actualiza en lote los precios de venta de los productos y deja registro en el historial para auditoría.";
        // 
        // lblPorcentaje
        // 
        lblPorcentaje.AutoSize = true;
        lblPorcentaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPorcentaje.Location = new Point(25, 110);
        lblPorcentaje.Name = "lblPorcentaje";
        lblPorcentaje.Size = new Size(211, 21);
        lblPorcentaje.TabIndex = 2;
        lblPorcentaje.Text = "Porcentaje de aumento (%):";
        // 
        // numPorcentaje
        // 
        numPorcentaje.DecimalPlaces = 2;
        numPorcentaje.Font = new Font("Segoe UI", 12F);
        numPorcentaje.Location = new Point(25, 135);
        numPorcentaje.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        numPorcentaje.Minimum = new decimal(new int[] { 100, 0, 0, -2147483648 });
        numPorcentaje.Name = "numPorcentaje";
        numPorcentaje.Size = new Size(390, 34);
        numPorcentaje.TabIndex = 3;
        numPorcentaje.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // lblCategoria
        // 
        lblCategoria.AutoSize = true;
        lblCategoria.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblCategoria.Location = new Point(25, 185);
        lblCategoria.Name = "lblCategoria";
        lblCategoria.Size = new Size(167, 21);
        lblCategoria.TabIndex = 4;
        lblCategoria.Text = "Filtrar por categoría:";
        // 
        // cboCategoria
        // 
        cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategoria.Font = new Font("Segoe UI", 10.5F);
        cboCategoria.FormattingEnabled = true;
        cboCategoria.Location = new Point(25, 210);
        cboCategoria.Name = "cboCategoria";
        cboCategoria.Size = new Size(390, 31);
        cboCategoria.TabIndex = 5;
        // 
        // lblRedondeo
        // 
        lblRedondeo.AutoSize = true;
        lblRedondeo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblRedondeo.Location = new Point(25, 255);
        lblRedondeo.Name = "lblRedondeo";
        lblRedondeo.Size = new Size(231, 21);
        lblRedondeo.TabIndex = 6;
        lblRedondeo.Text = "Redondear precios a múltiplo:";
        // 
        // cboRedondeo
        // 
        cboRedondeo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRedondeo.Font = new Font("Segoe UI", 10.5F);
        cboRedondeo.FormattingEnabled = true;
        cboRedondeo.Items.AddRange(new object[] { "1 (Sin redondeo)", "5", "10", "50" });
        cboRedondeo.Location = new Point(25, 280);
        cboRedondeo.Name = "cboRedondeo";
        cboRedondeo.Size = new Size(390, 31);
        cboRedondeo.TabIndex = 7;
        // 
        // btnAplicar
        // 
        btnAplicar.BackColor = Color.FromArgb(40, 167, 69);
        btnAplicar.FlatAppearance.BorderSize = 0;
        btnAplicar.FlatStyle = FlatStyle.Flat;
        btnAplicar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        btnAplicar.ForeColor = Color.White;
        btnAplicar.Location = new Point(165, 345);
        btnAplicar.Name = "btnAplicar";
        btnAplicar.Size = new Size(120, 42);
        btnAplicar.TabIndex = 8;
        btnAplicar.Text = "Aplicar";
        btnAplicar.UseVisualStyleBackColor = false;
        btnAplicar.Click += btnAplicar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Font = new Font("Segoe UI", 10.5F);
        btnCancelar.Location = new Point(295, 345);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(120, 42);
        btnCancelar.TabIndex = 9;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FrmAumentoPrecios
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(440, 410);
        Controls.Add(btnCancelar);
        Controls.Add(btnAplicar);
        Controls.Add(cboRedondeo);
        Controls.Add(lblRedondeo);
        Controls.Add(cboCategoria);
        Controls.Add(lblCategoria);
        Controls.Add(numPorcentaje);
        Controls.Add(lblPorcentaje);
        Controls.Add(lblInfo);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmAumentoPrecios";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Aumento Masivo de Precios";
        Load += FrmAumentoPrecios_Load;
        ((System.ComponentModel.ISupportInitialize)numPorcentaje).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitulo;
    private Label lblInfo;
    private Label lblPorcentaje;
    private NumericUpDown numPorcentaje;
    private Label lblCategoria;
    private ComboBox cboCategoria;
    private Label lblRedondeo;
    private ComboBox cboRedondeo;
    private Button btnAplicar;
    private Button btnCancelar;
}

