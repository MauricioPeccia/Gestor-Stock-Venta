namespace UI
{
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblEstado = new Label();
            lblDetalleApertura = new Label();
            grpApertura = new GroupBox();
            btnAbrirCaja = new Button();
            txtMontoApertura = new TextBox();
            lblMontoApertura = new Label();
            grpMovimientos = new GroupBox();
            btnRegistrarMov = new Button();
            txtMontoMov = new TextBox();
            lblMontoMov = new Label();
            txtConcepto = new TextBox();
            lblConcepto = new Label();
            cboTipoMov = new ComboBox();
            lblTipoMov = new Label();
            grpCierre = new GroupBox();
            btnCerrarCaja = new Button();
            txtObsCierre = new TextBox();
            lblObsCierre = new Label();
            txtEfectivoContado = new TextBox();
            lblEfectivoContado = new Label();
            grpResumen = new GroupBox();
            btnReabrirCaja = new Button();
            lblResumenDiferencia = new Label();
            lblResumenEfectivoContado = new Label();
            lblResumenEfectivoEsperado = new Label();
            lblResumenGanancia = new Label();
            lblResumenEgresos = new Label();
            lblResumenIngresos = new Label();
            lblResumenVentas = new Label();
            lblResumenApertura = new Label();
            btnCerrarVentana = new Button();
            grpApertura.SuspendLayout();
            grpMovimientos.SuspendLayout();
            grpCierre.SuspendLayout();
            grpResumen.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(15, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(357, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Control de Jornada de Caja";
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblEstado.ForeColor = Color.DarkGreen;
            lblEstado.Location = new Point(18, 52);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(160, 28);
            lblEstado.TabIndex = 1;
            lblEstado.Text = "Estado: Cargando...";
            // 
            // lblDetalleApertura
            // 
            lblDetalleApertura.AutoSize = true;
            lblDetalleApertura.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblDetalleApertura.ForeColor = Color.DimGray;
            lblDetalleApertura.Location = new Point(18, 83);
            lblDetalleApertura.Name = "lblDetalleApertura";
            lblDetalleApertura.Size = new Size(74, 21);
            lblDetalleApertura.TabIndex = 2;
            lblDetalleApertura.Text = "Detalles...";
            // 
            // grpApertura
            // 
            grpApertura.Controls.Add(btnAbrirCaja);
            grpApertura.Controls.Add(txtMontoApertura);
            grpApertura.Controls.Add(lblMontoApertura);
            grpApertura.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpApertura.Location = new Point(18, 115);
            grpApertura.Name = "grpApertura";
            grpApertura.Size = new Size(440, 140);
            grpApertura.TabIndex = 3;
            grpApertura.TabStop = false;
            grpApertura.Text = "1. Apertura de Caja";
            // 
            // btnAbrirCaja
            // 
            btnAbrirCaja.BackColor = Color.ForestGreen;
            btnAbrirCaja.Cursor = Cursors.Hand;
            btnAbrirCaja.FlatStyle = FlatStyle.Flat;
            btnAbrirCaja.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnAbrirCaja.ForeColor = Color.White;
            btnAbrirCaja.Location = new Point(245, 55);
            btnAbrirCaja.Name = "btnAbrirCaja";
            btnAbrirCaja.Size = new Size(175, 45);
            btnAbrirCaja.TabIndex = 2;
            btnAbrirCaja.Text = "ABRIR CAJA";
            btnAbrirCaja.UseVisualStyleBackColor = false;
            // 
            // txtMontoApertura
            // 
            txtMontoApertura.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtMontoApertura.Location = new Point(15, 60);
            txtMontoApertura.Name = "txtMontoApertura";
            txtMontoApertura.Size = new Size(210, 34);
            txtMontoApertura.TabIndex = 1;
            txtMontoApertura.Text = "0";
            // 
            // lblMontoApertura
            // 
            lblMontoApertura.AutoSize = true;
            lblMontoApertura.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblMontoApertura.Location = new Point(15, 33);
            lblMontoApertura.Name = "lblMontoApertura";
            lblMontoApertura.Size = new Size(185, 21);
            lblMontoApertura.TabIndex = 0;
            lblMontoApertura.Text = "Monto inicial en efectivo:";
            // 
            // grpMovimientos
            // 
            grpMovimientos.Controls.Add(btnRegistrarMov);
            grpMovimientos.Controls.Add(txtMontoMov);
            grpMovimientos.Controls.Add(lblMontoMov);
            grpMovimientos.Controls.Add(txtConcepto);
            grpMovimientos.Controls.Add(lblConcepto);
            grpMovimientos.Controls.Add(cboTipoMov);
            grpMovimientos.Controls.Add(lblTipoMov);
            grpMovimientos.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpMovimientos.Location = new Point(18, 265);
            grpMovimientos.Name = "grpMovimientos";
            grpMovimientos.Size = new Size(440, 240);
            grpMovimientos.TabIndex = 4;
            grpMovimientos.TabStop = false;
            grpMovimientos.Text = "2. Movimientos Manuales (Ingreso / Egreso)";
            // 
            // btnRegistrarMov
            // 
            btnRegistrarMov.BackColor = Color.SteelBlue;
            btnRegistrarMov.Cursor = Cursors.Hand;
            btnRegistrarMov.FlatStyle = FlatStyle.Flat;
            btnRegistrarMov.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnRegistrarMov.ForeColor = Color.White;
            btnRegistrarMov.Location = new Point(245, 175);
            btnRegistrarMov.Name = "btnRegistrarMov";
            btnRegistrarMov.Size = new Size(175, 45);
            btnRegistrarMov.TabIndex = 6;
            btnRegistrarMov.Text = "REGISTRAR";
            btnRegistrarMov.UseVisualStyleBackColor = false;
            // 
            // txtMontoMov
            // 
            txtMontoMov.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtMontoMov.Location = new Point(15, 180);
            txtMontoMov.Name = "txtMontoMov";
            txtMontoMov.Size = new Size(210, 32);
            txtMontoMov.TabIndex = 5;
            // 
            // lblMontoMov
            // 
            lblMontoMov.AutoSize = true;
            lblMontoMov.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblMontoMov.Location = new Point(15, 155);
            lblMontoMov.Name = "lblMontoMov";
            lblMontoMov.Size = new Size(81, 21);
            lblMontoMov.TabIndex = 4;
            lblMontoMov.Text = "Monto ($):";
            // 
            // txtConcepto
            // 
            txtConcepto.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtConcepto.Location = new Point(15, 120);
            txtConcepto.Name = "txtConcepto";
            txtConcepto.PlaceholderText = "Ej: Retiro de caja, Cambio de monedas, Flete...";
            txtConcepto.Size = new Size(405, 30);
            txtConcepto.TabIndex = 3;
            // 
            // lblConcepto
            // 
            lblConcepto.AutoSize = true;
            lblConcepto.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblConcepto.Location = new Point(15, 95);
            lblConcepto.Name = "lblConcepto";
            lblConcepto.Size = new Size(139, 21);
            lblConcepto.TabIndex = 2;
            lblConcepto.Text = "Concepto / Motivo:";
            // 
            // cboTipoMov
            // 
            cboTipoMov.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoMov.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cboTipoMov.FormattingEnabled = true;
            cboTipoMov.Items.AddRange(new object[] { "EGRESO", "INGRESO" });
            cboTipoMov.Location = new Point(15, 60);
            cboTipoMov.Name = "cboTipoMov";
            cboTipoMov.Size = new Size(210, 31);
            cboTipoMov.TabIndex = 1;
            // 
            // lblTipoMov
            // 
            lblTipoMov.AutoSize = true;
            lblTipoMov.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblTipoMov.Location = new Point(15, 33);
            lblTipoMov.Name = "lblTipoMov";
            lblTipoMov.Size = new Size(43, 21);
            lblTipoMov.TabIndex = 0;
            lblTipoMov.Text = "Tipo:";
            // 
            // grpCierre
            // 
            grpCierre.Controls.Add(btnCerrarCaja);
            grpCierre.Controls.Add(txtObsCierre);
            grpCierre.Controls.Add(lblObsCierre);
            grpCierre.Controls.Add(txtEfectivoContado);
            grpCierre.Controls.Add(lblEfectivoContado);
            grpCierre.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpCierre.Location = new Point(475, 115);
            grpCierre.Name = "grpCierre";
            grpCierre.Size = new Size(460, 200);
            grpCierre.TabIndex = 5;
            grpCierre.TabStop = false;
            grpCierre.Text = "3. Cierre Ciego de Caja (Arqueo)";
            // 
            // btnCerrarCaja
            // 
            btnCerrarCaja.BackColor = Color.Crimson;
            btnCerrarCaja.Cursor = Cursors.Hand;
            btnCerrarCaja.FlatStyle = FlatStyle.Flat;
            btnCerrarCaja.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnCerrarCaja.ForeColor = Color.White;
            btnCerrarCaja.Location = new Point(265, 135);
            btnCerrarCaja.Name = "btnCerrarCaja";
            btnCerrarCaja.Size = new Size(175, 45);
            btnCerrarCaja.TabIndex = 4;
            btnCerrarCaja.Text = "CERRAR CAJA";
            btnCerrarCaja.UseVisualStyleBackColor = false;
            // 
            // txtObsCierre
            // 
            txtObsCierre.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtObsCierre.Location = new Point(15, 140);
            txtObsCierre.Name = "txtObsCierre";
            txtObsCierre.PlaceholderText = "Notas u observaciones finales...";
            txtObsCierre.Size = new Size(230, 30);
            txtObsCierre.TabIndex = 3;
            // 
            // lblObsCierre
            // 
            lblObsCierre.AutoSize = true;
            lblObsCierre.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblObsCierre.Location = new Point(15, 115);
            lblObsCierre.Name = "lblObsCierre";
            lblObsCierre.Size = new Size(183, 21);
            lblObsCierre.TabIndex = 2;
            lblObsCierre.Text = "Observaciones (opcional):";
            // 
            // txtEfectivoContado
            // 
            txtEfectivoContado.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            txtEfectivoContado.Location = new Point(15, 65);
            txtEfectivoContado.Name = "txtEfectivoContado";
            txtEfectivoContado.Size = new Size(425, 39);
            txtEfectivoContado.TabIndex = 1;
            // 
            // lblEfectivoContado
            // 
            lblEfectivoContado.AutoSize = true;
            lblEfectivoContado.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblEfectivoContado.Location = new Point(15, 35);
            lblEfectivoContado.Name = "lblEfectivoContado";
            lblEfectivoContado.Size = new Size(328, 21);
            lblEfectivoContado.TabIndex = 0;
            lblEfectivoContado.Text = "Efectivo contado físicamente en el cajón ($):";
            // 
            // grpResumen
            // 
            grpResumen.Controls.Add(btnReabrirCaja);
            grpResumen.Controls.Add(lblResumenDiferencia);
            grpResumen.Controls.Add(lblResumenEfectivoContado);
            grpResumen.Controls.Add(lblResumenEfectivoEsperado);
            grpResumen.Controls.Add(lblResumenGanancia);
            grpResumen.Controls.Add(lblResumenEgresos);
            grpResumen.Controls.Add(lblResumenIngresos);
            grpResumen.Controls.Add(lblResumenVentas);
            grpResumen.Controls.Add(lblResumenApertura);
            grpResumen.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpResumen.Location = new Point(475, 325);
            grpResumen.Name = "grpResumen";
            grpResumen.Size = new Size(460, 260);
            grpResumen.TabIndex = 6;
            grpResumen.TabStop = false;
            grpResumen.Text = "Resumen del Turno / Arqueo";
            // 
            // btnReabrirCaja
            // 
            btnReabrirCaja.BackColor = Color.DarkOrange;
            btnReabrirCaja.Cursor = Cursors.Hand;
            btnReabrirCaja.FlatStyle = FlatStyle.Flat;
            btnReabrirCaja.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnReabrirCaja.ForeColor = Color.White;
            btnReabrirCaja.Location = new Point(275, 205);
            btnReabrirCaja.Name = "btnReabrirCaja";
            btnReabrirCaja.Size = new Size(165, 40);
            btnReabrirCaja.TabIndex = 8;
            btnReabrirCaja.Text = "Reabrir Caja Cerrada";
            btnReabrirCaja.UseVisualStyleBackColor = false;
            // 
            // lblResumenDiferencia
            // 
            lblResumenDiferencia.AutoSize = true;
            lblResumenDiferencia.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblResumenDiferencia.ForeColor = Color.Black;
            lblResumenDiferencia.Location = new Point(15, 210);
            lblResumenDiferencia.Name = "lblResumenDiferencia";
            lblResumenDiferencia.Size = new Size(183, 28);
            lblResumenDiferencia.TabIndex = 7;
            lblResumenDiferencia.Text = "Diferencia: $ 0,00";
            // 
            // lblResumenEfectivoContado
            // 
            lblResumenEfectivoContado.AutoSize = true;
            lblResumenEfectivoContado.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblResumenEfectivoContado.Location = new Point(15, 175);
            lblResumenEfectivoContado.Name = "lblResumenEfectivoContado";
            lblResumenEfectivoContado.Size = new Size(196, 21);
            lblResumenEfectivoContado.TabIndex = 6;
            lblResumenEfectivoContado.Text = "Efectivo Contado: $ 0,00";
            // 
            // lblResumenEfectivoEsperado
            // 
            lblResumenEfectivoEsperado.AutoSize = true;
            lblResumenEfectivoEsperado.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblResumenEfectivoEsperado.Location = new Point(15, 145);
            lblResumenEfectivoEsperado.Name = "lblResumenEfectivoEsperado";
            lblResumenEfectivoEsperado.Size = new Size(207, 21);
            lblResumenEfectivoEsperado.TabIndex = 5;
            lblResumenEfectivoEsperado.Text = "Efectivo Esperado: $ 0,00";
            // 
            // lblResumenGanancia
            // 
            lblResumenGanancia.AutoSize = true;
            lblResumenGanancia.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblResumenGanancia.ForeColor = Color.ForestGreen;
            lblResumenGanancia.Location = new Point(15, 120);
            lblResumenGanancia.Name = "lblResumenGanancia";
            lblResumenGanancia.Size = new Size(183, 21);
            lblResumenGanancia.TabIndex = 4;
            lblResumenGanancia.Text = "Ganancia Bruta: $ 0,00";
            // 
            // lblResumenEgresos
            // 
            lblResumenEgresos.AutoSize = true;
            lblResumenEgresos.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblResumenEgresos.ForeColor = Color.Crimson;
            lblResumenEgresos.Location = new Point(230, 90);
            lblResumenEgresos.Name = "lblResumenEgresos";
            lblResumenEgresos.Size = new Size(130, 21);
            lblResumenEgresos.TabIndex = 3;
            lblResumenEgresos.Text = "Egresos: $ 0,00";
            // 
            // lblResumenIngresos
            // 
            lblResumenIngresos.AutoSize = true;
            lblResumenIngresos.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblResumenIngresos.ForeColor = Color.SteelBlue;
            lblResumenIngresos.Location = new Point(15, 90);
            lblResumenIngresos.Name = "lblResumenIngresos";
            lblResumenIngresos.Size = new Size(135, 21);
            lblResumenIngresos.TabIndex = 2;
            lblResumenIngresos.Text = "Ingresos: $ 0,00";
            // 
            // lblResumenVentas
            // 
            lblResumenVentas.AutoSize = true;
            lblResumenVentas.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblResumenVentas.Location = new Point(15, 60);
            lblResumenVentas.Name = "lblResumenVentas";
            lblResumenVentas.Size = new Size(390, 21);
            lblResumenVentas.TabIndex = 1;
            lblResumenVentas.Text = "Ventas Ef: $0,00 | Transf: $0,00 | Total: $0,00 (0 ventas)";
            // 
            // lblResumenApertura
            // 
            lblResumenApertura.AutoSize = true;
            lblResumenApertura.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblResumenApertura.Location = new Point(15, 30);
            lblResumenApertura.Name = "lblResumenApertura";
            lblResumenApertura.Size = new Size(187, 21);
            lblResumenApertura.TabIndex = 0;
            lblResumenApertura.Text = "Monto Apertura: $ 0,00";
            // 
            // btnCerrarVentana
            // 
            btnCerrarVentana.BackColor = Color.Gray;
            btnCerrarVentana.Cursor = Cursors.Hand;
            btnCerrarVentana.FlatStyle = FlatStyle.Flat;
            btnCerrarVentana.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnCerrarVentana.ForeColor = Color.White;
            btnCerrarVentana.Location = new Point(18, 535);
            btnCerrarVentana.Name = "btnCerrarVentana";
            btnCerrarVentana.Size = new Size(440, 50);
            btnCerrarVentana.TabIndex = 7;
            btnCerrarVentana.Text = "VOLVER AL MENÚ";
            btnCerrarVentana.UseVisualStyleBackColor = false;
            // 
            // FrmCaja
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(956, 610);
            Controls.Add(btnCerrarVentana);
            Controls.Add(grpResumen);
            Controls.Add(grpCierre);
            Controls.Add(grpMovimientos);
            Controls.Add(grpApertura);
            Controls.Add(lblDetalleApertura);
            Controls.Add(lblEstado);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FrmCaja";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestión de Caja";
            grpApertura.ResumeLayout(false);
            grpApertura.PerformLayout();
            grpMovimientos.ResumeLayout(false);
            grpMovimientos.PerformLayout();
            grpCierre.ResumeLayout(false);
            grpCierre.PerformLayout();
            grpResumen.ResumeLayout(false);
            grpResumen.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblEstado;
        private Label lblDetalleApertura;
        private GroupBox grpApertura;
        private Button btnAbrirCaja;
        private TextBox txtMontoApertura;
        private Label lblMontoApertura;
        private GroupBox grpMovimientos;
        private Button btnRegistrarMov;
        private TextBox txtMontoMov;
        private Label lblMontoMov;
        private TextBox txtConcepto;
        private Label lblConcepto;
        private ComboBox cboTipoMov;
        private Label lblTipoMov;
        private GroupBox grpCierre;
        private Button btnCerrarCaja;
        private TextBox txtObsCierre;
        private Label lblObsCierre;
        private TextBox txtEfectivoContado;
        private Label lblEfectivoContado;
        private GroupBox grpResumen;
        private Label lblResumenVentas;
        private Label lblResumenApertura;
        private Label lblResumenIngresos;
        private Label lblResumenEgresos;
        private Label lblResumenGanancia;
        private Label lblResumenEfectivoEsperado;
        private Label lblResumenEfectivoContado;
        private Label lblResumenDiferencia;
        private Button btnReabrirCaja;
        private Button btnCerrarVentana;
    }
}