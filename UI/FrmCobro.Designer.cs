namespace UI
{
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            lblMetodo = new Label();
            cboMetodo = new ComboBox();
            lblPagaCon = new Label();
            txtPagaCon = new TextBox();
            lblVueltoTitulo = new Label();
            lblVuelto = new Label();
            lblTransferencia = new Label();
            txtTransferencia = new TextBox();
            lblDescuento = new Label();
            txtDescuento = new TextBox();
            btnConfirmar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotalTitulo.Location = new Point(25, 20);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.Size = new Size(130, 28);
            lblTotalTitulo.TabIndex = 0;
            lblTotalTitulo.Text = "Total a Cobrar:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotal.ForeColor = Color.ForestGreen;
            lblTotal.Location = new Point(25, 48);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(137, 54);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "$ 0,00";
            // 
            // lblDescuento
            // 
            lblDescuento.AutoSize = true;
            lblDescuento.Location = new Point(25, 115);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.Size = new Size(107, 20);
            lblDescuento.TabIndex = 2;
            lblDescuento.Text = "Descuento ($):";
            // 
            // txtDescuento
            // 
            txtDescuento.Location = new Point(25, 140);
            txtDescuento.Name = "txtDescuento";
            txtDescuento.Size = new Size(420, 27);
            txtDescuento.TabIndex = 3;
            txtDescuento.Text = "0";
            // 
            // lblMetodo
            // 
            lblMetodo.AutoSize = true;
            lblMetodo.Location = new Point(25, 180);
            lblMetodo.Name = "lblMetodo";
            lblMetodo.Size = new Size(125, 20);
            lblMetodo.TabIndex = 4;
            lblMetodo.Text = "Método de Pago:";
            // 
            // cboMetodo
            // 
            cboMetodo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMetodo.FormattingEnabled = true;
            cboMetodo.Items.AddRange(new object[] { "EFECTIVO", "TRANSFERENCIA", "MIXTO" });
            cboMetodo.Location = new Point(25, 205);
            cboMetodo.Name = "cboMetodo";
            cboMetodo.Size = new Size(420, 28);
            cboMetodo.TabIndex = 5;
            // 
            // lblPagaCon
            // 
            lblPagaCon.AutoSize = true;
            lblPagaCon.Location = new Point(25, 245);
            lblPagaCon.Name = "lblPagaCon";
            lblPagaCon.Size = new Size(166, 20);
            lblPagaCon.TabIndex = 6;
            lblPagaCon.Text = "Paga con (Efectivo) ($):";
            // 
            // txtPagaCon
            // 
            txtPagaCon.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtPagaCon.Location = new Point(25, 270);
            txtPagaCon.Name = "txtPagaCon";
            txtPagaCon.Size = new Size(420, 34);
            txtPagaCon.TabIndex = 7;
            // 
            // lblTransferencia
            // 
            lblTransferencia.AutoSize = true;
            lblTransferencia.Location = new Point(25, 315);
            lblTransferencia.Name = "lblTransferencia";
            lblTransferencia.Size = new Size(174, 20);
            lblTransferencia.TabIndex = 8;
            lblTransferencia.Text = "Monto Transferencia ($):";
            lblTransferencia.Visible = false;
            // 
            // txtTransferencia
            // 
            txtTransferencia.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtTransferencia.Location = new Point(25, 340);
            txtTransferencia.Name = "txtTransferencia";
            txtTransferencia.Size = new Size(420, 34);
            txtTransferencia.TabIndex = 9;
            txtTransferencia.Visible = false;
            // 
            // lblVueltoTitulo
            // 
            lblVueltoTitulo.AutoSize = true;
            lblVueltoTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblVueltoTitulo.Location = new Point(25, 390);
            lblVueltoTitulo.Name = "lblVueltoTitulo";
            lblVueltoTitulo.Size = new Size(71, 25);
            lblVueltoTitulo.TabIndex = 10;
            lblVueltoTitulo.Text = "Vuelto:";
            // 
            // lblVuelto
            // 
            lblVuelto.AutoSize = true;
            lblVuelto.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblVuelto.ForeColor = Color.DarkBlue;
            lblVuelto.Location = new Point(25, 415);
            lblVuelto.Name = "lblVuelto";
            lblVuelto.Size = new Size(103, 41);
            lblVuelto.TabIndex = 11;
            lblVuelto.Text = "$ 0,00";
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.ForestGreen;
            btnConfirmar.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(25, 485);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(220, 50);
            btnConfirmar.TabIndex = 12;
            btnConfirmar.Text = "CONFIRMAR (Enter)";
            btnConfirmar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(265, 485);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(180, 50);
            btnCancelar.TabIndex = 13;
            btnCancelar.Text = "Cancelar (Esc)";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // FrmCobro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(475, 560);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmar);
            Controls.Add(lblVuelto);
            Controls.Add(lblVueltoTitulo);
            Controls.Add(txtTransferencia);
            Controls.Add(lblTransferencia);
            Controls.Add(txtPagaCon);
            Controls.Add(lblPagaCon);
            Controls.Add(cboMetodo);
            Controls.Add(lblMetodo);
            Controls.Add(txtDescuento);
            Controls.Add(lblDescuento);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCobro";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cobrar Venta";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTotalTitulo;
        private Label lblTotal;
        private Label lblDescuento;
        private TextBox txtDescuento;
        private Label lblMetodo;
        private ComboBox cboMetodo;
        private Label lblPagaCon;
        private TextBox txtPagaCon;
        private Label lblTransferencia;
        private TextBox txtTransferencia;
        private Label lblVueltoTitulo;
        private Label lblVuelto;
        private Button btnConfirmar;
        private Button btnCancelar;
    }
}

