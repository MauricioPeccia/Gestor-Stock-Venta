namespace Kiosco.UI;

partial class FrmCobro
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
        lblTotalTitulo = new Label();
        lblTotalValor = new Label();
        lblEfectivo = new Label();
        numEfectivo = new NumericUpDown();
        lblTransferencia = new Label();
        numTransferencia = new NumericUpDown();
        panelVuelto = new Panel();
        lblVueltoTitulo = new Label();
        lblVueltoValor = new Label();
        btnConfirmar = new Button();
        btnCancelar = new Button();
        ((System.ComponentModel.ISupportInitialize)numEfectivo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numTransferencia).BeginInit();
        panelVuelto.SuspendLayout();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.Location = new Point(25, 20);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(160, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Cobro - Pago";
        // 
        // lblTotalTitulo
        // 
        lblTotalTitulo.AutoSize = true;
        lblTotalTitulo.Font = new Font("Segoe UI", 10F);
        lblTotalTitulo.Location = new Point(25, 65);
        lblTotalTitulo.Name = "lblTotalTitulo";
        lblTotalTitulo.Size = new Size(111, 23);
        lblTotalTitulo.TabIndex = 1;
        lblTotalTitulo.Text = "Total a cobrar:";
        // 
        // lblTotalValor
        // 
        lblTotalValor.AutoSize = true;
        lblTotalValor.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTotalValor.ForeColor = Color.DarkGreen;
        lblTotalValor.Location = new Point(25, 90);
        lblTotalValor.Name = "lblTotalValor";
        lblTotalValor.Size = new Size(107, 46);
        lblTotalValor.TabIndex = 2;
        lblTotalValor.Text = "$ 0,00";
        // 
        // lblEfectivo
        // 
        lblEfectivo.AutoSize = true;
        lblEfectivo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblEfectivo.Location = new Point(25, 150);
        lblEfectivo.Name = "lblEfectivo";
        lblEfectivo.Size = new Size(150, 23);
        lblEfectivo.TabIndex = 3;
        lblEfectivo.Text = "Efectivo recibido:";
        // 
        // numEfectivo
        // 
        numEfectivo.DecimalPlaces = 2;
        numEfectivo.Font = new Font("Segoe UI", 14F);
        numEfectivo.Location = new Point(25, 180);
        numEfectivo.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numEfectivo.Name = "numEfectivo";
        numEfectivo.Size = new Size(350, 39);
        numEfectivo.TabIndex = 4;
        numEfectivo.ThousandsSeparator = true;
        numEfectivo.ValueChanged += Calculos_ValueChanged;
        numEfectivo.Enter += num_Enter;
        // 
        // lblTransferencia
        // 
        lblTransferencia.AutoSize = true;
        lblTransferencia.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTransferencia.Location = new Point(25, 235);
        lblTransferencia.Name = "lblTransferencia";
        lblTransferencia.Size = new Size(122, 23);
        lblTransferencia.TabIndex = 5;
        lblTransferencia.Text = "Transferencia:";
        // 
        // numTransferencia
        // 
        numTransferencia.DecimalPlaces = 2;
        numTransferencia.Font = new Font("Segoe UI", 14F);
        numTransferencia.Location = new Point(25, 265);
        numTransferencia.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numTransferencia.Name = "numTransferencia";
        numTransferencia.Size = new Size(350, 39);
        numTransferencia.TabIndex = 6;
        numTransferencia.ThousandsSeparator = true;
        numTransferencia.ValueChanged += Calculos_ValueChanged;
        numTransferencia.Enter += num_Enter;
        // 
        // panelVuelto
        // 
        panelVuelto.BackColor = Color.FromArgb(233, 247, 239);
        panelVuelto.BorderStyle = BorderStyle.FixedSingle;
        panelVuelto.Controls.Add(lblVueltoTitulo);
        panelVuelto.Controls.Add(lblVueltoValor);
        panelVuelto.Location = new Point(25, 320);
        panelVuelto.Name = "panelVuelto";
        panelVuelto.Padding = new Padding(15, 10, 15, 10);
        panelVuelto.Size = new Size(350, 75);
        panelVuelto.TabIndex = 7;
        // 
        // lblVueltoTitulo
        // 
        lblVueltoTitulo.AutoSize = true;
        lblVueltoTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblVueltoTitulo.ForeColor = Color.FromArgb(21, 87, 36);
        lblVueltoTitulo.Location = new Point(10, 8);
        lblVueltoTitulo.Name = "lblVueltoTitulo";
        lblVueltoTitulo.Size = new Size(60, 20);
        lblVueltoTitulo.TabIndex = 0;
        lblVueltoTitulo.Text = "VUELTO";
        // 
        // lblVueltoValor
        // 
        lblVueltoValor.AutoSize = true;
        lblVueltoValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblVueltoValor.ForeColor = Color.FromArgb(21, 87, 36);
        lblVueltoValor.Location = new Point(10, 30);
        lblVueltoValor.Name = "lblVueltoValor";
        lblVueltoValor.Size = new Size(88, 37);
        lblVueltoValor.TabIndex = 1;
        lblVueltoValor.Text = "$ 0,00";
        // 
        // btnConfirmar
        // 
        btnConfirmar.BackColor = Color.FromArgb(40, 167, 69);
        btnConfirmar.FlatAppearance.BorderSize = 0;
        btnConfirmar.FlatStyle = FlatStyle.Flat;
        btnConfirmar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnConfirmar.ForeColor = Color.White;
        btnConfirmar.Location = new Point(25, 415);
        btnConfirmar.Name = "btnConfirmar";
        btnConfirmar.Size = new Size(170, 50);
        btnConfirmar.TabIndex = 8;
        btnConfirmar.Text = "Confirmar (F5)";
        btnConfirmar.UseVisualStyleBackColor = false;
        btnConfirmar.Click += btnConfirmar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Font = new Font("Segoe UI", 11F);
        btnCancelar.Location = new Point(205, 415);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(170, 50);
        btnCancelar.TabIndex = 9;
        btnCancelar.Text = "Cancelar (Esc)";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FrmCobro
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(405, 485);
        Controls.Add(btnCancelar);
        Controls.Add(btnConfirmar);
        Controls.Add(panelVuelto);
        Controls.Add(numTransferencia);
        Controls.Add(lblTransferencia);
        Controls.Add(numEfectivo);
        Controls.Add(lblEfectivo);
        Controls.Add(lblTotalValor);
        Controls.Add(lblTotalTitulo);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmCobro";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Cobro de Venta";
        KeyDown += FrmCobro_KeyDown;
        ((System.ComponentModel.ISupportInitialize)numEfectivo).EndInit();
        ((System.ComponentModel.ISupportInitialize)numTransferencia).EndInit();
        panelVuelto.ResumeLayout(false);
        panelVuelto.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitulo;
    private Label lblTotalTitulo;
    private Label lblTotalValor;
    private Label lblEfectivo;
    private NumericUpDown numEfectivo;
    private Label lblTransferencia;
    private NumericUpDown numTransferencia;
    private Panel panelVuelto;
    private Label lblVueltoTitulo;
    private Label lblVueltoValor;
    private Button btnConfirmar;
    private Button btnCancelar;
}

