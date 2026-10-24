using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Formulario modal para la ejecución y auditoría visual de las Pruebas de Integración (Ciclo Completo de Producto).
    /// Construido 100% de forma programática en InicializarComponentes() y consumiendo exclusivamente RecursosUI.cs.
    /// </summary>
    public sealed class FormPruebasIntegracion : Form
    {
        private Panel _pnlHeader = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;
        private Label _lblSeparadorHeader = null!;

        private Panel _pnlContenedorLog = null!;
        private Label _lblTituloLog = null!;
        private RichTextBox _rtbLogResultados = null!;

        private Panel _pnlFooter = null!;
        private Label _lblSeparadorFooter = null!;
        private Button _btnEjecutarPrueba = null!;
        private Button _btnLimpiar = null!;
        private Button _btnCerrar = null!;

        private readonly PruebaIntegracionNegocio _pruebaIntegracionNegocio;
        private bool _ejecutando;

        public FormPruebasIntegracion()
        {
            _pruebaIntegracionNegocio = new PruebaIntegracionNegocio();
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario (Flat Design + Navegación 100% por Teclado)
                this.Text = "Pruebas de Integración del Sistema - Kiosco Lite";
                this.ClientSize = new Size(880, 560);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;
                this.DoubleBuffered = true;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;

                // 2. Instanciación de Controles
                _pnlHeader = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();
                _lblSeparadorHeader = new Label();

                _pnlContenedorLog = new Panel();
                _lblTituloLog = new Label();
                _rtbLogResultados = new RichTextBox();

                _pnlFooter = new Panel();
                _lblSeparadorFooter = new Label();
                _btnEjecutarPrueba = new Button();
                _btnLimpiar = new Button();
                _btnCerrar = new Button();

                // 3. Configuración de Propiedades y Estilos (Cero GDI+ local, 100% RecursosUI)
                // --- Encabezado ---
                _pnlHeader.Dock = DockStyle.Top;
                _pnlHeader.Height = 82;
                _pnlHeader.BackColor = RecursosUI.ColorEncabezado;

                _lblTitulo.Text = "Auditoría y Pruebas de Integración (E2E)";
                _lblTitulo.Font = RecursosUI.FuenteTituloModulo;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoClaro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(24, 14);

                _lblSubtitulo.Text = "Simulación transaccional: Alta Producto (Stock 0) -> Compra (+50 u. / -Caja) -> Venta (-5 u. / +Subtotal)";
                _lblSubtitulo.Font = RecursosUI.FuenteEstandar;
                _lblSubtitulo.ForeColor = RecursosUI.ColorBorde;
                _lblSubtitulo.AutoSize = true;
                _lblSubtitulo.Location = new Point(26, 48);

                _lblSeparadorHeader.Dock = DockStyle.Bottom;
                _lblSeparadorHeader.Height = 1;
                _lblSeparadorHeader.BackColor = RecursosUI.ColorSeparador;

                // --- Contenedor Central de Bitácora (Log) ---
                _pnlContenedorLog.Location = new Point(24, 100);
                _pnlContenedorLog.Size = new Size(832, 368);
                _pnlContenedorLog.BackColor = RecursosUI.ColorSuperficie;

                _lblTituloLog.Text = "BITÁCORA DE EJECUCIÓN TRANSACCIONAL (SOLO LECTURA)   |   Presione [F5] para iniciar el ciclo";
                _lblTituloLog.Font = RecursosUI.FuentePequenaNegrita;
                _lblTituloLog.ForeColor = RecursosUI.ColorTextoSecundario;
                _lblTituloLog.Location = new Point(16, 12);
                _lblTituloLog.Size = new Size(800, 18);

                _rtbLogResultados.Location = new Point(16, 36);
                _rtbLogResultados.Size = new Size(800, 316);
                _rtbLogResultados.BackColor = RecursosUI.ColorSuperficieSuave;
                _rtbLogResultados.ForeColor = RecursosUI.ColorTextoPrincipal;
                _rtbLogResultados.Font = RecursosUI.FuenteMonospace;
                _rtbLogResultados.BorderStyle = BorderStyle.None;
                _rtbLogResultados.ReadOnly = true;
                _rtbLogResultados.WordWrap = true;
                _rtbLogResultados.ScrollBars = RichTextBoxScrollBars.Vertical;
                _rtbLogResultados.TabIndex = 0;
                _rtbLogResultados.TabStop = true;
                _rtbLogResultados.Text =
                    "Listo para ejecutar la Prueba de Integración.\n" +
                    "Presione la tecla [F5] o haga clic en 'Ejecutar Prueba (F5)' para simular el ciclo completo del producto.";

                // --- Pie de Ventana (Botones y Atajos) ---
                _pnlFooter.Dock = DockStyle.Bottom;
                _pnlFooter.Height = 72;
                _pnlFooter.BackColor = RecursosUI.ColorSuperficie;

                _lblSeparadorFooter.Dock = DockStyle.Top;
                _lblSeparadorFooter.Height = 1;
                _lblSeparadorFooter.BackColor = RecursosUI.ColorSeparador;

                _btnEjecutarPrueba.Text = "[ F5 ]   Ejecutar Prueba de Integración";
                _btnEjecutarPrueba.Location = new Point(24, 15);
                _btnEjecutarPrueba.Size = new Size(290, 42);
                _btnEjecutarPrueba.FlatStyle = FlatStyle.Flat;
                _btnEjecutarPrueba.FlatAppearance.BorderSize = 0;
                _btnEjecutarPrueba.BackColor = RecursosUI.ColorExito;
                _btnEjecutarPrueba.ForeColor = RecursosUI.ColorTextoClaro;
                _btnEjecutarPrueba.Font = RecursosUI.FuenteNegrita;
                _btnEjecutarPrueba.Cursor = Cursors.Hand;
                _btnEjecutarPrueba.TabIndex = 1;
                _btnEjecutarPrueba.TabStop = true;

                _btnLimpiar.Text = "Limpiar Log";
                _btnLimpiar.Location = new Point(328, 15);
                _btnLimpiar.Size = new Size(140, 42);
                _btnLimpiar.FlatStyle = FlatStyle.Flat;
                _btnLimpiar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnLimpiar.BackColor = RecursosUI.ColorSuperficie;
                _btnLimpiar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnLimpiar.Font = RecursosUI.FuenteEstandar;
                _btnLimpiar.Cursor = Cursors.Hand;
                _btnLimpiar.TabIndex = 2;
                _btnLimpiar.TabStop = true;

                _btnCerrar.Text = "[ ESC ]   Cerrar y Volver";
                _btnCerrar.Location = new Point(656, 15);
                _btnCerrar.Size = new Size(200, 42);
                _btnCerrar.FlatStyle = FlatStyle.Flat;
                _btnCerrar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCerrar.BackColor = RecursosUI.ColorSuperficieSuave;
                _btnCerrar.ForeColor = RecursosUI.ColorTextoPrincipal;
                _btnCerrar.Font = RecursosUI.FuenteNegrita;
                _btnCerrar.Cursor = Cursors.Hand;
                _btnCerrar.TabIndex = 3;
                _btnCerrar.TabStop = true;

                // 5. Suscripción a Eventos
                _btnEjecutarPrueba.Click += (s, e) => EjecutarSimulacion();
                _btnLimpiar.Click += (s, e) => LimpiarBitacora();
                _btnCerrar.Click += (s, e) => CerrarFormulario();

                // 7. Agregado de Controles al Contenedor
                _pnlHeader.Controls.Add(_lblTitulo);
                _pnlHeader.Controls.Add(_lblSubtitulo);
                _pnlHeader.Controls.Add(_lblSeparadorHeader);

                _pnlContenedorLog.Controls.Add(_lblTituloLog);
                _pnlContenedorLog.Controls.Add(_rtbLogResultados);

                _pnlFooter.Controls.Add(_lblSeparadorFooter);
                _pnlFooter.Controls.Add(_btnEjecutarPrueba);
                _pnlFooter.Controls.Add(_btnLimpiar);
                _pnlFooter.Controls.Add(_btnCerrar);

                this.Controls.Add(_pnlContenedorLog);
                this.Controls.Add(_pnlHeader);
                this.Controls.Add(_pnlFooter);

                this.ActiveControl = _btnEjecutarPrueba;
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        /// <summary>
        /// Captura global de teclado: F5 ejecuta el test de integración y ESC destruye el formulario modal.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                EjecutarSimulacion();
                return true;
            }

            if (keyData == Keys.Escape)
            {
                CerrarFormulario();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EjecutarSimulacion()
        {
            if (_ejecutando) return;

            _ejecutando = true;
            _btnEjecutarPrueba.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                _rtbLogResultados.Clear();
                List<string> resultados = _pruebaIntegracionNegocio.EjecutarCicloCompletoProducto();

                foreach (string linea in resultados)
                {
                    AgregarLineaConEstilo(linea);
                }

                _rtbLogResultados.SelectionStart = _rtbLogResultados.TextLength;
                _rtbLogResultados.ScrollToCaret();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                _btnEjecutarPrueba.Enabled = true;
                _btnEjecutarPrueba.Focus();
                _ejecutando = false;
            }
        }

        private void AgregarLineaConEstilo(string linea)
        {
            _rtbLogResultados.SelectionStart = _rtbLogResultados.TextLength;
            _rtbLogResultados.SelectionLength = 0;

            if (linea.StartsWith("OK:", StringComparison.OrdinalIgnoreCase))
            {
                _rtbLogResultados.SelectionColor = RecursosUI.ColorInventario;
            }
            else if (linea.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase) ||
                     linea.StartsWith("EXCEPCIÓN", StringComparison.OrdinalIgnoreCase))
            {
                _rtbLogResultados.SelectionColor = RecursosUI.ColorPeligro;
            }
            else if (linea.Contains("==="))
            {
                _rtbLogResultados.SelectionColor = RecursosUI.ColorTextoAzulOscuro;
            }
            else
            {
                _rtbLogResultados.SelectionColor = RecursosUI.ColorTextoPrincipal;
            }

            _rtbLogResultados.AppendText(linea + Environment.NewLine + Environment.NewLine);
            _rtbLogResultados.SelectionColor = RecursosUI.ColorTextoPrincipal;
        }

        private void LimpiarBitacora()
        {
            _rtbLogResultados.Clear();
            _rtbLogResultados.ForeColor = RecursosUI.ColorTextoPrincipal;
            _rtbLogResultados.Text = "Bitácora limpia. Presione [F5] para ejecutar un nuevo ciclo de integración.";
            _btnEjecutarPrueba.Focus();
        }

        private void CerrarFormulario()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
