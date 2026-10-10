namespace Kiosco.UI;

partial class FrmCaja
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
        lblInfoCaja = new Label();
        lblEfectivo = new Label();
        numEfectivoContado = new NumericUpDown();
        lblObservaciones = new Label();
        txtObservaciones = new TextBox();
        btnCerrarCaja = new Button();
        btnCancelar = new Button();
        ((System.ComponentModel.ISupportInitialize)numEfectivoContado).BeginInit();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.Location = new Point(20, 20);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(188, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Cierre de Caja";
        // 
        // lblInfoCaja
        // 
        lblInfoCaja.AutoSize = true;
        lblInfoCaja.Font = new Font("Segoe UI", 10F);
        lblInfoCaja.Location = new Point(20, 65);
        lblInfoCaja.Name = "lblInfoCaja";
        lblInfoCaja.Size = new Size(153, 23);
        lblInfoCaja.TabIndex = 1;
        lblInfoCaja.Text = "Cargando estado...";
        // 
        // lblEfectivo
        // 
        lblEfectivo.AutoSize = true;
        lblEfectivo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblEfectivo.Location = new Point(20, 110);
        lblEfectivo.Name = "lblEfectivo";
        lblEfectivo.Size = new Size(207, 23);
        lblEfectivo.TabIndex = 2;
        lblEfectivo.Text = "Efectivo contado (físico):";
        // 
        // numEfectivoContado
        // 
        numEfectivoContado.DecimalPlaces = 2;
        numEfectivoContado.Font = new Font("Segoe UI", 12F);
        numEfectivoContado.Location = new Point(20, 140);
        numEfectivoContado.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numEfectivoContado.Name = "numEfectivoContado";
        numEfectivoContado.Size = new Size(380, 34);
        numEfectivoContado.TabIndex = 3;
        numEfectivoContado.ThousandsSeparator = true;
        // 
        // lblObservaciones
        // 
        lblObservaciones.AutoSize = true;
        lblObservaciones.Font = new Font("Segoe UI", 10F);
        lblObservaciones.Location = new Point(20, 190);
        lblObservaciones.Name = "lblObservaciones";
        lblObservaciones.Size = new Size(125, 23);
        lblObservaciones.TabIndex = 4;
        lblObservaciones.Text = "Observaciones:";
        // 
        // txtObservaciones
        // 
        txtObservaciones.Font = new Font("Segoe UI", 10F);
        txtObservaciones.Location = new Point(20, 220);
        txtObservaciones.Multiline = true;
        txtObservaciones.Name = "txtObservaciones";
        txtObservaciones.Size = new Size(380, 80);
        txtObservaciones.TabIndex = 5;
        // 
        // btnCerrarCaja
        // 
        btnCerrarCaja.BackColor = Color.IndianRed;
        btnCerrarCaja.FlatStyle = FlatStyle.Flat;
        btnCerrarCaja.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCerrarCaja.ForeColor = Color.White;
        btnCerrarCaja.Location = new Point(20, 320);
        btnCerrarCaja.Name = "btnCerrarCaja";
        btnCerrarCaja.Size = new Size(180, 45);
        btnCerrarCaja.TabIndex = 6;
        btnCerrarCaja.Text = "Cerrar Caja";
        btnCerrarCaja.UseVisualStyleBackColor = false;
        btnCerrarCaja.Click += btnCerrarCaja_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Font = new Font("Segoe UI", 10F);
        btnCancelar.Location = new Point(220, 320);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(180, 45);
        btnCancelar.TabIndex = 7;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FrmCaja
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(430, 395);
        Controls.Add(btnCancelar);
        Controls.Add(btnCerrarCaja);
        Controls.Add(txtObservaciones);
        Controls.Add(lblObservaciones);
        Controls.Add(numEfectivoContado);
        Controls.Add(lblEfectivo);
        Controls.Add(lblInfoCaja);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmCaja";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Gestión de Caja";
        Load += FrmCaja_Load;
        ((System.ComponentModel.ISupportInitialize)numEfectivoContado).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitulo;
    private Label lblInfoCaja;
    private Label lblEfectivo;
    private NumericUpDown numEfectivoContado;
    private Label lblObservaciones;
    private TextBox txtObservaciones;
    private Button btnCerrarCaja;
    private Button btnCancelar;
}

