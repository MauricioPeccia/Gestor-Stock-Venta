using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Visor modal de Ticket Térmico de Kiosco Lite.
    /// Construido 100% de forma programática en InicializarComponentes() y consumiendo exclusivamente RecursosUI.cs.
    /// </summary>
    public sealed class FormVisorTicket : Form
    {
        private readonly int _nroTicket;
        private readonly DateTime _fecha;
        private readonly string _medioPago;
        private readonly List<DetalleVenta> _productos;
        private readonly decimal _total;

        private Panel _pnlTicketTermico = null!;
        private Label _lblContenidoTicket = null!;
        private Button _btnCerrar = null!;

        public FormVisorTicket(
            int nroTicket,
            DateTime fecha,
            string medioPago,
            List<DetalleVenta> listaProductos,
            decimal total)
        {
            _nroTicket = nroTicket;
            _fecha = fecha;
            _medioPago = medioPago;
            _productos = listaProductos ?? new List<DetalleVenta>();
            _total = total;

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = $"Comprobante de Venta #{_nroTicket:D6} - Kiosco Lite";
                this.ClientSize = new Size(440, 560);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;
                this.DoubleBuffered = true;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;

                // 2. Instanciación de Controles
                _pnlTicketTermico = new Panel();
                _lblContenidoTicket = new Label();
                _btnCerrar = new Button();

                // 3. Configuración de Propiedades y Estilos (Simulación de Papel Térmico)
                _pnlTicketTermico.Location = new Point(36, 20);
                _pnlTicketTermico.Size = new Size(368, 456);
                _pnlTicketTermico.BackColor = RecursosUI.ColorFondoTicketTermico;
                _pnlTicketTermico.BorderStyle = BorderStyle.FixedSingle;
                _pnlTicketTermico.AutoScroll = true;
                _pnlTicketTermico.Padding = new Padding(14, 14, 14, 14);

                _lblContenidoTicket.Font = RecursosUI.FuenteTicketTermico;
                _lblContenidoTicket.ForeColor = RecursosUI.ColorTextoPrincipal;
                _lblContenidoTicket.BackColor = RecursosUI.ColorFondoTicketTermico;
                _lblContenidoTicket.AutoSize = true;
                _lblContenidoTicket.Location = new Point(14, 14);
                _lblContenidoTicket.Text = ConstruirTextoTicketTermico();

                _btnCerrar.Text = "Cerrar Ticket [ENTER / ESC]";
                _btnCerrar.Location = new Point(36, 492);
                _btnCerrar.Size = new Size(368, 44);
                _btnCerrar.FlatStyle = FlatStyle.Flat;
                _btnCerrar.FlatAppearance.BorderSize = 0;
                _btnCerrar.BackColor = RecursosUI.ColorPrimario;
                _btnCerrar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnCerrar.Font = RecursosUI.FuenteNegrita;
                _btnCerrar.Cursor = Cursors.Hand;
                _btnCerrar.TabIndex = 0;

                // 5. Suscripción a Eventos
                _btnCerrar.Click += (s, e) => CerrarVisor();

                // 7. Agregado de Controles al Contenedor
                _pnlTicketTermico.Controls.Add(_lblContenidoTicket);
                this.Controls.Add(_pnlTicketTermico);
                this.Controls.Add(_btnCerrar);

                this.ActiveControl = _btnCerrar;
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private string ConstruirTextoTicketTermico()
        {
            const int anchoLinea = 38;
            string separadorDoble = new string('=', anchoLinea);
            string separadorSimple = new string('-', anchoLinea);

            var sb = new StringBuilder();
            sb.AppendLine(separadorDoble);
            sb.AppendLine(CentrarTexto("KIOSCO LITE", anchoLinea));
            sb.AppendLine(CentrarTexto("COMPROBANTE DE VENTA", anchoLinea));
            sb.AppendLine(separadorDoble);
            sb.AppendLine($"TICKET NRO : #{_nroTicket:D6}");
            sb.AppendLine($"FECHA      : {_fecha:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"PAGO       : {_medioPago.ToUpperInvariant()}");
            sb.AppendLine(separadorSimple);
            sb.AppendLine("CANT x PRODUCTO               SUBTOTAL");
            sb.AppendLine(separadorSimple);

            foreach (DetalleVenta item in _productos)
            {
                string cantTexto = item.EsFraccionado
                    ? $"{item.Cantidad:0.000}Kg"
                    : $"{item.Cantidad:0.##}u";

                string lineaProducto = $"{cantTexto} x {item.Descripcion}";
                string subtotalTexto = $"$ {item.Subtotal:N2}";

                int espacioDisponible = anchoLinea - subtotalTexto.Length - 1;
                if (lineaProducto.Length > espacioDisponible)
                {
                    lineaProducto = lineaProducto.Substring(0, Math.Max(1, espacioDisponible - 2)) + "..";
                }

                sb.AppendLine(lineaProducto.PadRight(espacioDisponible) + " " + subtotalTexto);
            }

            sb.AppendLine(separadorSimple);
            string etiquetaTotal = "TOTAL:";
            string valorTotal = $"$ {_total:N2}";
            sb.AppendLine(etiquetaTotal.PadRight(anchoLinea - valorTotal.Length) + valorTotal);
            sb.AppendLine(separadorDoble);
            sb.AppendLine(CentrarTexto("¡GRACIAS POR SU COMPRA!", anchoLinea));
            sb.AppendLine(separadorDoble);

            return sb.ToString();
        }

        private static string CentrarTexto(string texto, int anchoTotal)
        {
            if (string.IsNullOrEmpty(texto) || texto.Length >= anchoTotal)
                return texto;

            int espaciosIzquierda = (anchoTotal - texto.Length) / 2;
            return texto.PadLeft(espaciosIzquierda + texto.Length).PadRight(anchoTotal);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.Enter)
            {
                CerrarVisor();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CerrarVisor()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
