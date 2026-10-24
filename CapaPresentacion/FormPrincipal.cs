using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using KioscoLite.CapaNegocio;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Menú Principal de Kiosco Lite.
    /// Construido 100% de forma programática bajo diseño Flat minimalista y navegación por teclado (1-4, F5, CTRL+B y ESC).
    /// </summary>
    public sealed class FormPrincipal : Form
    {
        private const string TextoBarraEstadoDefault =
            "Atajos:  [1] Caja   [2] Inventario   [3] Movimientos   [4] Reportes   [F5] Test   [Ctrl+B] Backup";

        private Panel _pnlEncabezado = null!;
        private Label _lblTitulo = null!;
        private Label _lblSubtitulo = null!;

        private Panel _pnlContenedorModulos = null!;
        private Button _btnCaja = null!;
        private Button _btnInventario = null!;
        private Button _btnMovimientos = null!;
        private Button _btnReportes = null!;

        private Panel _pnlBarraEstado = null!;
        private Label _lblEstadoControles = null!;
        private Label _lblEstadoSalir = null!;

        private readonly GestorBackupNegocio _gestorBackup;

        public FormPrincipal()
        {
            _gestorBackup = new GestorBackupNegocio();
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Kiosco Lite - Sistema de Gestión Comercial";
                this.ClientSize = new Size(760, 480);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Maximized;
                this.MaximizeBox = false;
                this.MinimizeBox = true;
                this.KeyPreview = true;
                this.BackColor = RecursosUI.ColorFondoVentana;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.DoubleBuffered = true;

                // 2. Instanciación de Controles
                _pnlEncabezado = new Panel();
                _lblTitulo = new Label();
                _lblSubtitulo = new Label();

                _pnlContenedorModulos = new Panel();
                _btnCaja = new Button();
                _btnInventario = new Button();
                _btnMovimientos = new Button();
                _btnReportes = new Button();

                _pnlBarraEstado = new Panel();
                _lblEstadoControles = new Label();
                _lblEstadoSalir = new Label();

                // 3. Configuración de Propiedades y Estilos (Flat Design + RecursosUI)
                // --- Encabezado ---
                _pnlEncabezado.Dock = DockStyle.Top;
                _pnlEncabezado.Height = 82;
                _pnlEncabezado.BackColor = RecursosUI.ColorEncabezado;

                _lblTitulo.Text = "KIOSCO LITE";
                _lblTitulo.Font = RecursosUI.FuenteTituloModulo;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoClaro;
                _lblTitulo.Location = new Point(28, 14);
                _lblTitulo.Size = new Size(450, 32);

                _lblSubtitulo.Text = "Terminal de Punto de Venta y Control de Stock  |  Navegación Rápida por Teclado";
                _lblSubtitulo.Font = RecursosUI.FuenteSubtituloApp;
                _lblSubtitulo.ForeColor = RecursosUI.ColorBorde;
                _lblSubtitulo.Location = new Point(30, 48);
                _lblSubtitulo.Size = new Size(680, 20);

                // --- Contenedor Central de Módulos ---
                _pnlContenedorModulos.Location = new Point(28, 106);
                _pnlContenedorModulos.Size = new Size(704, 316);
                _pnlContenedorModulos.Anchor = AnchorStyles.None;
                _pnlContenedorModulos.BackColor = RecursosUI.ColorFondoVentana;

                ConfigurarBotonModulo(
                    boton: _btnCaja,
                    texto: "[ 1 ]   CAJA (VENTAS)\n\nTerminal rápida y cobro",
                    colorFondo: RecursosUI.ColorCaja,
                    colorHover: RecursosUI.ColorCajaHover,
                    ubicacion: new Point(0, 0),
                    tabIndex: 0
                );

                ConfigurarBotonModulo(
                    boton: _btnInventario,
                    texto: "[ 2 ]   INVENTARIO (STOCK)\n\nCatálogo y ajuste de stock",
                    colorFondo: RecursosUI.ColorInventario,
                    colorHover: RecursosUI.ColorInventarioHover,
                    ubicacion: new Point(360, 0),
                    tabIndex: 1
                );

                ConfigurarBotonModulo(
                    boton: _btnMovimientos,
                    texto: "[ 3 ]   MOVIMIENTOS DE CAJA\n\nEgresos, retiros y compras",
                    colorFondo: RecursosUI.ColorMovimientos,
                    colorHover: RecursosUI.ColorMovimientosHover,
                    ubicacion: new Point(0, 164),
                    tabIndex: 2
                );

                ConfigurarBotonModulo(
                    boton: _btnReportes,
                    texto: "[ 4 ]   REPORTES\n\nCierres y métricas de ventas",
                    colorFondo: RecursosUI.ColorReportes,
                    colorHover: RecursosUI.ColorReportesHover,
                    ubicacion: new Point(360, 164),
                    tabIndex: 3
                );

                // --- Barra de Estado Inferior ---
                _pnlBarraEstado.Dock = DockStyle.Bottom;
                _pnlBarraEstado.Height = 36;
                _pnlBarraEstado.BackColor = RecursosUI.ColorBarraEstado;

                _lblEstadoControles.Text = TextoBarraEstadoDefault;
                _lblEstadoControles.Font = RecursosUI.FuentePequena;
                _lblEstadoControles.ForeColor = RecursosUI.ColorTextoClaro;
                _lblEstadoControles.Location = new Point(28, 9);
                _lblEstadoControles.Size = new Size(540, 18);

                _lblEstadoSalir.Text = "[ESC] Salir del Sistema";
                _lblEstadoSalir.Font = RecursosUI.FuenteNegrita;
                _lblEstadoSalir.ForeColor = RecursosUI.ColorTextoClaro;
                _lblEstadoSalir.TextAlign = ContentAlignment.MiddleRight;
                _lblEstadoSalir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                _lblEstadoSalir.Location = new Point(570, 8);
                _lblEstadoSalir.Size = new Size(162, 20);

                // 5. Suscripción a Eventos
                _btnCaja.Click += (s, e) => AbrirModuloCaja();
                _btnInventario.Click += (s, e) => AbrirModuloInventario();
                _btnMovimientos.Click += (s, e) => AbrirModuloMovimientos();
                _btnReportes.Click += (s, e) => AbrirModuloReportes();

                // 7. Agregado de Controles a Contenedores
                _pnlEncabezado.Controls.Add(_lblTitulo);
                _pnlEncabezado.Controls.Add(_lblSubtitulo);

                _pnlContenedorModulos.Controls.Add(_btnCaja);
                _pnlContenedorModulos.Controls.Add(_btnInventario);
                _pnlContenedorModulos.Controls.Add(_btnMovimientos);
                _pnlContenedorModulos.Controls.Add(_btnReportes);

                _pnlBarraEstado.Controls.Add(_lblEstadoControles);
                _pnlBarraEstado.Controls.Add(_lblEstadoSalir);

                this.Controls.Add(_pnlContenedorModulos);
                this.Controls.Add(_pnlEncabezado);
                this.Controls.Add(_pnlBarraEstado);

                this.ActiveControl = _btnCaja;
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private static void ConfigurarBotonModulo(
            Button boton,
            string texto,
            Color colorFondo,
            Color colorHover,
            Point ubicacion,
            int tabIndex)
        {
            boton.Text = texto;
            boton.Location = ubicacion;
            boton.Size = new Size(344, 148);
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = colorHover;
            boton.FlatAppearance.MouseDownBackColor = colorHover;
            boton.BackColor = colorFondo;
            boton.ForeColor = RecursosUI.ColorTextoClaro;
            boton.Font = RecursosUI.FuenteBotonModulo;
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.Padding = new Padding(18, 0, 14, 0);
            boton.TabIndex = tabIndex;
            boton.TabStop = true;
            boton.Cursor = Cursors.Hand;
            boton.UseVisualStyleBackColor = false;
        }

        /// <summary>
        /// Captura global de teclado para activación directa por números (1-4, F5 Pruebas, CTRL+B Backup) y salida con ESC.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.B))
            {
                _ = EjecutarBackupSilenciosoAsync();
                return true;
            }

            switch (keyData)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    AbrirModuloCaja();
                    return true;

                case Keys.D2:
                case Keys.NumPad2:
                    AbrirModuloInventario();
                    return true;

                case Keys.D3:
                case Keys.NumPad3:
                    AbrirModuloMovimientos();
                    return true;

                case Keys.D4:
                case Keys.NumPad4:
                    AbrirModuloReportes();
                    return true;

                case Keys.D5:
                case Keys.NumPad5:
                case Keys.F5:
                    AbrirModuloPruebasIntegracion();
                    return true;

                case Keys.Escape:
                    ConfirmarSalida();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Ejecuta el backup de mysqldump en un hilo secundario y muestra un aviso visual discreto
        /// y no bloqueante en la barra de estado inferior apoyándose en RecursosUI.cs.
        /// </summary>
        private async Task EjecutarBackupSilenciosoAsync()
        {
            if (this.IsDisposed) return;

            _pnlBarraEstado.BackColor = RecursosUI.ColorMovimientos;
            _lblEstadoControles.Font = RecursosUI.FuentePequenaNegrita;
            _lblEstadoControles.Text = "⏳ Ejecutando respaldo silencioso de base de datos (mysqldump)...";

            ResultadoBackup resultado = await _gestorBackup.EjecutarRespaldoSilencioso(soloSiNoExisteHoy: false);

            if (this.IsDisposed) return;

            _pnlBarraEstado.BackColor = resultado.Exito ? RecursosUI.ColorInventario : RecursosUI.ColorPeligro;
            _lblEstadoControles.Text = resultado.Exito
                ? $"✔ {resultado.Mensaje}"
                : $"✖ {resultado.Mensaje}";

            await Task.Delay(4500);

            if (!this.IsDisposed)
            {
                _pnlBarraEstado.BackColor = RecursosUI.ColorBarraEstado;
                _lblEstadoControles.Font = RecursosUI.FuentePequena;
                _lblEstadoControles.Text = TextoBarraEstadoDefault;
            }
        }

        private void AbrirModuloCaja()
        {
            _btnCaja.Focus();
            using (var frm = new FormCaja())
            {
                frm.ShowDialog(this);
            }
            Optimizador.LiberarMemoria();
        }

        private void AbrirModuloInventario()
        {
            _btnInventario.Focus();
            using (var frm = new FrmProductos())
            {
                frm.ShowDialog(this);
            }
            Optimizador.LiberarMemoria();
        }

        private void AbrirModuloMovimientos()
        {
            _btnMovimientos.Focus();
            using (var frm = new FormMovimientos())
            {
                frm.ShowDialog(this);
            }
            Optimizador.LiberarMemoria();
        }

        private void AbrirModuloReportes()
        {
            _btnReportes.Focus();
            using (var frm = new FormReportes())
            {
                frm.ShowDialog(this);
            }
            Optimizador.LiberarMemoria();
        }

        private void AbrirModuloPruebasIntegracion()
        {
            using (var frm = new FormPruebasIntegracion())
            {
                frm.ShowDialog(this);
            }
            Optimizador.LiberarMemoria();
        }

        private void ConfirmarSalida()
        {
            DialogResult respuesta = MessageBox.Show(
                this,
                "¿Está seguro de que desea salir de Kiosco Lite?",
                "Confirmar Salida (ESC)",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1
            );

            if (respuesta == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
