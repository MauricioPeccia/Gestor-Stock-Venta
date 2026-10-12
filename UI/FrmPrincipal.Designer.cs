namespace UI
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnVender = new Button();
            lblEstadoCaja = new Label();
            SuspendLayout();
            // 
            // btnVender
            // 
            btnVender.BackColor = SystemColors.ControlText;
            btnVender.FlatStyle = FlatStyle.Popup;
            btnVender.ForeColor = Color.Gray;
            btnVender.Location = new Point(419, 508);
            btnVender.Name = "btnVender";
            btnVender.Size = new Size(134, 46);
            btnVender.TabIndex = 0;
            btnVender.Text = "VENDER";
            btnVender.UseVisualStyleBackColor = false;
            // 
            // lblEstadoCaja
            // 
            lblEstadoCaja.AutoSize = true;
            lblEstadoCaja.Location = new Point(22, 14);
            lblEstadoCaja.Name = "lblEstadoCaja";
            lblEstadoCaja.Size = new Size(37, 20);
            lblEstadoCaja.TabIndex = 1;
            lblEstadoCaja.Text = "       ";
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1010, 566);
            Controls.Add(lblEstadoCaja);
            Controls.Add(btnVender);
            Name = "FrmPrincipal";
            Text = "FrmPrincipal";
            Load += FrmPrincipal_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVender;
        private Label lblEstadoCaja;
    }
}