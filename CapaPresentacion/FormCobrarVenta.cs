using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Pantalla modal de confirmación de pago (F12).
    /// Construida 100% por código en InicializarComponentes(), operada por teclado ([1] Efectivo, [2] Transferencia, [ENTER] Confirmar, [ESC] Cancelar)
    /// y consumiendo exclusivamente recursos de RecursosUI.cs.
    /// </summary>
    public sealed class FormCobrarVenta : Form
    {
        public string MedioPagoSeleccionado { get; private set; } = "EFECTIVO";
        public bool GenerarTicketSeleccionado => chkGenerarTicket.Checked;
        public int IdVentaRegistrada { get; private set; }
        public decimal TotalVenta { get; }

        private readonly List<DetalleVenta> _itemsVenta;
        private readonly VentaNegocio _ventaNegocio;

        private Panel _pnlMontoSuperior = null!;
        private Label _lblTituloMonto = null!;
        private Label _lblValorMonto = null!;

        private Label _lblSeccionMetodo = null!;
        private Button _btnEfectivo = null!;
        private Button _btnTransferencia = null!;
        private CheckBox chkGenerarTicket = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorFooter = null!;
        private Button _btnConfirmarCobro = null!;
        private Button _btnCancelar = null!;

        public FormCobrarVenta(decimal totalVenta, List<DetalleVenta> itemsVenta, VentaNegocio? ventaNegocio = null)
        {
            TotalVenta = totalVenta;
            _itemsVenta = itemsVenta ?? new List<DetalleVenta>();
            _ventaNegocio = ventaNegocio ?? new VentaNegocio();

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Confirmación de Pago - Kiosco Lite";
                this.ClientSize = new Size(480, 420);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;
                this.DoubleBuffered = true;
                this.BackColor = RecursosUI.ColorSuperficie;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;

                // 2. Instanciación de Controles
                _pnlMontoSuperior = new Panel();
                _lblTituloMonto = new Label();
                _lblValorMonto = new Label();

                _lblSeccionMetodo = new Label();
                _btnEfectivo = new Button();
                _btnTransferencia = new Button();
                chkGenerarTicket = new CheckBox();

                _pnlFooter = new Panel();
                _lblSeparadorFooter = new Label();
                _btnConfirmarCobro = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades y Estilos (RecursosUI)
                // --- Panel Superior Destacando el "MONTO TOTAL A COBRAR" ---
                _pnlMontoSuperior.Dock = DockStyle.Top;
                _pnlMontoSuperior.Size = new Size(this.ClientSize.Width, 118);
                _pnlMontoSuperior.BackColor = RecursosUI.ColorFondoPanelTotal;
                _pnlMontoSuperior.Padding = new Padding(24, 16, 24, 14);

                _lblTituloMonto.Text = "MONTO TOTAL A COBRAR";
                _lblTituloMonto.Font = RecursosUI.FuenteNegrita;
                _lblTituloMonto.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblTituloMonto.Dock = DockStyle.Top;
                _lblTituloMonto.Height = 24;
                _lblTituloMonto.TextAlign = ContentAlignment.MiddleCenter;

                _lblValorMonto.Text = $"$ {TotalVenta:N2}";
                _lblValorMonto.Font = RecursosUI.FuenteTotalCaja;
                _lblValorMonto.ForeColor = RecursosUI.ColorTextoTotalCaja;
                _lblValorMonto.Dock = DockStyle.Fill;
                _lblValorMonto.TextAlign = ContentAlignment.MiddleCenter;

                // --- Botones Grandes de Métodos de Pago ---
                _lblSeccionMetodo.Text = "Seleccione el Método de Pago (Teclas [1] o [2]):";
                _lblSeccionMetodo.Font = RecursosUI.FuenteNegrita;
                _lblSeccionMetodo.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblSeccionMetodo.AutoSize = true;
                _lblSeccionMetodo.Location = new Point(28, 140);

                _btnEfectivo.Text = "[ 1 ] EFECTIVO";
                _btnEfectivo.Location = new Point(28, 170);
                _btnEfectivo.Size = new Size(204, 64);
                _btnEfectivo.Font = RecursosUI.FuenteGrillaHeader;
                _btnEfectivo.FlatStyle = FlatStyle.Flat;
                _btnEfectivo.Cursor = Cursors.Hand;
                _btnEfectivo.TabIndex = 0;

                _btnTransferencia.Text = "[ 2 ] TRANSFERENCIA";
                _btnTransferencia.Location = new Point(248, 170);
                _btnTransferencia.Size = new Size(204, 64);
                _btnTransferencia.Font = RecursosUI.FuenteGrillaHeader;
                _btnTransferencia.FlatStyle = FlatStyle.Flat;
                _btnTransferencia.Cursor = Cursors.Hand;
                _btnTransferencia.TabIndex = 1;

                // --- CheckBox de Ticket ---
                chkGenerarTicket.Text = "Generar e Imprimir Ticket";
                chkGenerarTicket.Font = RecursosUI.FuenteNegrita;
                chkGenerarTicket.ForeColor = RecursosUI.ColorTextoPrincipal;
                chkGenerarTicket.AutoSize = true;
                chkGenerarTicket.Location = new Point(30, 260);
                chkGenerarTicket.Checked = true;
                chkGenerarTicket.Cursor = Cursors.Hand;
                chkGenerarTicket.TabIndex = 2;

                // --- Footer con Botón de Acción "CONFIRMAR COBRO (ENTER)" ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Size = new Size(this.ClientSize.Width, 84);
                _pnlFooter.BackColor = RecursosUI.ColorFondoVentana;

                _lblSeparadorFooter.Dock = DockStyle.Top;
                _lblSeparadorFooter.Height = 1;
                _lblSeparadorFooter.BackColor = RecursosUI.ColorSeparador;

                _btnConfirmarCobro.Text = "CONFIRMAR COBRO (ENTER)";
                _btnConfirmarCobro.Location = new Point(28, 18);
                _btnConfirmarCobro.Size = new Size(268, 48);
                _btnConfirmarCobro.FlatStyle = FlatStyle.Flat;
                _btnConfirmarCobro.FlatAppearance.BorderSize = 0;
                _btnConfirmarCobro.BackColor = RecursosUI.ColorExito;
                _btnConfirmarCobro.ForeColor = RecursosUI.ColorTextoClaro;
                _btnConfirmarCobro.Font = RecursosUI.FuenteGrillaHeader;
                _btnConfirmarCobro.Cursor = Cursors.Hand;
                _btnConfirmarCobro.TabIndex = 3;

                _btnCancelar.Text = "Cancelar [ESC]";
                _btnCancelar.Location = new Point(310, 18);
                _btnCancelar.Size = new Size(142, 48);
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 4;

                // Aplicar estado inicial de selección de medio de pago
                SeleccionarMedioPago("EFECTIVO");

                // 5. Suscripción a Eventos
                _btnEfectivo.Click += (s, e) => SeleccionarMedioPago("EFECTIVO");
                _btnTransferencia.Click += (s, e) => SeleccionarMedioPago("TRANSFERENCIA");
                _btnConfirmarCobro.Click += (s, e) => EjecutarCobroVenta();
                _btnCancelar.Click += (s, e) => CancelarYCerrar();

                // 7. Agregado de Controles al Contenedor
                _pnlMontoSuperior.Controls.Add(_lblValorMonto);
                _pnlMontoSuperior.Controls.Add(_lblTituloMonto);

                _pnlFooter.Controls.Add(_lblSeparadorFooter);
                _pnlFooter.Controls.Add(_btnConfirmarCobro);
                _pnlFooter.Controls.Add(_btnCancelar);

                this.Controls.Add(_lblSeccionMetodo);
                this.Controls.Add(_btnEfectivo);
                this.Controls.Add(_btnTransferencia);
                this.Controls.Add(chkGenerarTicket);
                this.Controls.Add(_pnlMontoSuperior);
                this.Controls.Add(_pnlFooter);

                this.ActiveControl = _btnConfirmarCobro;
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        /// <summary>
        /// Captura global de teclado: '1' Efectivo, '2' Transferencia, 'ENTER' Confirmar Cobro, 'ESC' Cancelar.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    SeleccionarMedioPago("EFECTIVO");
                    return true;

                case Keys.D2:
                case Keys.NumPad2:
                    SeleccionarMedioPago("TRANSFERENCIA");
                    return true;

                case Keys.Enter:
                    EjecutarCobroVenta();
                    return true;

                case Keys.Escape:
                    CancelarYCerrar();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SeleccionarMedioPago(string medio)
        {
            MedioPagoSeleccionado = medio;

            if (medio == "EFECTIVO")
            {
                _btnEfectivo.BackColor = RecursosUI.ColorPrimario;
                _btnEfectivo.ForeColor = RecursosUI.ColorTextoClaro;
                _btnEfectivo.FlatAppearance.BorderSize = 0;

                _btnTransferencia.BackColor = RecursosUI.ColorSuperficieSuave;
                _btnTransferencia.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnTransferencia.FlatAppearance.BorderSize = 1;
                _btnTransferencia.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
            }
            else
            {
                _btnTransferencia.BackColor = RecursosUI.ColorPrimario;
                _btnTransferencia.ForeColor = RecursosUI.ColorTextoClaro;
                _btnTransferencia.FlatAppearance.BorderSize = 0;

                _btnEfectivo.BackColor = RecursosUI.ColorSuperficieSuave;
                _btnEfectivo.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnEfectivo.FlatAppearance.BorderSize = 1;
                _btnEfectivo.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
            }
        }

        /// <summary>
        /// Registra la venta en la base de datos y, si chkGenerarTicket está marcado,
        /// muestra el visor térmico FormVisorTicket de forma modal.
        /// </summary>
        private void EjecutarCobroVenta()
        {
            try
            {
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    MedioPago = MedioPagoSeleccionado,
                    Total = TotalVenta,
                    RequiereTicket = chkGenerarTicket.Checked,
                    Detalles = new List<DetalleVenta>(_itemsVenta)
                };

                IdVentaRegistrada = _ventaNegocio.RegistrarVentaCompleta(venta);

                if (chkGenerarTicket.Checked)
                {
                    _ventaNegocio.GenerarTicketPDF(venta, IdVentaRegistrada);

                    using (var visorTicket = new FormVisorTicket(
                        nroTicket: IdVentaRegistrada,
                        fecha: venta.Fecha,
                        medioPago: venta.MedioPago,
                        listaProductos: venta.Detalles,
                        total: venta.Total))
                    {
                        visorTicket.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show(
                        this,
                        $"¡Venta #{IdVentaRegistrada} registrada con éxito!\n\nMedio de Pago: {venta.MedioPago}\nTotal Cobrado: $ {venta.Total:N2}",
                        "Venta Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "Error procesando la venta: " + ex.Message,
                    "Error en Transacción",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CancelarYCerrar()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
