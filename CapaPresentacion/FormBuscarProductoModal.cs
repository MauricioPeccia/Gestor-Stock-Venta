using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Modal de búsqueda rápida de catálogo en memoria (F5).
    /// 100% programático, operado por teclado y libre de asignaciones GDI+ directas.
    /// </summary>
    public sealed class FormBuscarProductoModal : Form
    {
        public Producto? ProductoSeleccionado { get; private set; }

        private readonly List<Producto> _catalogo;
        private readonly string _filtroInicial;

        private Panel _pnlTop = null!;
        private Label _lblTitulo = null!;
        private TextBox _txtFiltro = null!;
        private DataGridView _dgvResultados = null!;
        private Panel _pnlBottom = null!;
        private Button _btnSeleccionar = null!;
        private Button _btnCancelar = null!;

        public FormBuscarProductoModal(List<Producto> catalogo, string filtroInicial = "")
        {
            _catalogo = catalogo ?? new List<Producto>();
            _filtroInicial = filtroInicial ?? string.Empty;

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.SuspendLayout();
            try
            {
                // 1. Configuración del Formulario
                this.Text = "Búsqueda Rápida de Productos (F5)";
                this.ClientSize = new Size(740, 490);
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackColor = RecursosUI.ColorSuperficie;
                this.Font = RecursosUI.FuenteEstandar;
                this.ForeColor = RecursosUI.ColorTextoPrincipal;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.KeyPreview = true;
                this.DoubleBuffered = true;

                // 2. Instanciación de Controles
                _pnlTop = new Panel();
                _lblTitulo = new Label();
                _txtFiltro = new TextBox();
                _dgvResultados = new DataGridView();
                _pnlBottom = new Panel();
                _btnSeleccionar = new Button();
                _btnCancelar = new Button();

                // 3. Configuración de Propiedades y Estilos
                _pnlTop.Dock = DockStyle.Top;
                _pnlTop.Height = 82;
                _pnlTop.BackColor = RecursosUI.ColorSuperficie;
                _pnlTop.Padding = new Padding(16);

                _lblTitulo.Text = "Catálogo Rápido de Productos (Escriba para filtrar sin acentos y presione ENTER)";
                _lblTitulo.Font = RecursosUI.FuenteEncabezado;
                _lblTitulo.ForeColor = RecursosUI.ColorTextoAzulOscuro;
                _lblTitulo.AutoSize = true;
                _lblTitulo.Location = new Point(16, 12);

                _txtFiltro.Font = RecursosUI.FuenteInput;
                _txtFiltro.ForeColor = RecursosUI.ColorTextoPrincipal;
                _txtFiltro.BackColor = RecursosUI.ColorSuperficie;
                _txtFiltro.BorderStyle = BorderStyle.FixedSingle;
                _txtFiltro.Location = new Point(16, 42);
                _txtFiltro.Size = new Size(708, 28);
                _txtFiltro.TabIndex = 0;

                // Grilla de Resultados
                _dgvResultados.Dock = DockStyle.Fill;
                _dgvResultados.BackgroundColor = RecursosUI.ColorSuperficie;
                _dgvResultados.BorderStyle = BorderStyle.None;
                _dgvResultados.EnableHeadersVisualStyles = false;
                _dgvResultados.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                _dgvResultados.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                _dgvResultados.GridColor = RecursosUI.ColorSeparador;
                _dgvResultados.ColumnHeadersDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvResultados.ColumnHeadersDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvResultados.ColumnHeadersDefaultCellStyle.Font = RecursosUI.FuenteGrillaHeader;
                _dgvResultados.ColumnHeadersHeight = 38;
                _dgvResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                _dgvResultados.DefaultCellStyle.Font = RecursosUI.FuenteEstandar;
                _dgvResultados.DefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvResultados.DefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvResultados.DefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvResultados.AlternatingRowsDefaultCellStyle.BackColor = RecursosUI.ColorSuperficieSuave;
                _dgvResultados.AlternatingRowsDefaultCellStyle.ForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvResultados.AlternatingRowsDefaultCellStyle.SelectionBackColor = RecursosUI.ColorSeleccionGrilla;
                _dgvResultados.AlternatingRowsDefaultCellStyle.SelectionForeColor = RecursosUI.ColorTextoPrincipal;
                _dgvResultados.RowTemplate.Height = 32;
                _dgvResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _dgvResultados.MultiSelect = false;
                _dgvResultados.ReadOnly = true;
                _dgvResultados.AllowUserToAddRows = false;
                _dgvResultados.AllowUserToDeleteRows = false;
                _dgvResultados.AllowUserToResizeRows = false;
                _dgvResultados.RowHeadersVisible = false;
                _dgvResultados.TabIndex = 1;

                _dgvResultados.Columns.Add("Id", "ID");
                _dgvResultados.Columns.Add("Nombre", "Descripción del Producto");
                _dgvResultados.Columns.Add("Codigo", "Código");
                _dgvResultados.Columns.Add("Venta", "Precio Venta");
                _dgvResultados.Columns.Add("Stock", "Stock");

                _dgvResultados.Columns["Id"]!.Visible = false;
                _dgvResultados.Columns["Nombre"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                _dgvResultados.Columns["Codigo"]!.Width = 140;
                _dgvResultados.Columns["Venta"]!.Width = 130;
                _dgvResultados.Columns["Stock"]!.Width = 120;
                _dgvResultados.Columns["Venta"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _dgvResultados.Columns["Stock"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Barra inferior
                _pnlBottom.Dock = DockStyle.Bottom;
                _pnlBottom.Height = 62;
                _pnlBottom.BackColor = RecursosUI.ColorFondoVentana;

                _btnSeleccionar.Text = "Seleccionar (ENTER)";
                _btnSeleccionar.BackColor = RecursosUI.ColorPrimario;
                _btnSeleccionar.ForeColor = RecursosUI.ColorTextoClaro;
                _btnSeleccionar.FlatStyle = FlatStyle.Flat;
                _btnSeleccionar.FlatAppearance.BorderSize = 0;
                _btnSeleccionar.Font = RecursosUI.FuenteNegrita;
                _btnSeleccionar.Location = new Point(16, 12);
                _btnSeleccionar.Size = new Size(190, 38);
                _btnSeleccionar.Cursor = Cursors.Hand;
                _btnSeleccionar.TabIndex = 2;

                _btnCancelar.Text = "Cerrar (ESC)";
                _btnCancelar.BackColor = RecursosUI.ColorSuperficie;
                _btnCancelar.ForeColor = RecursosUI.ColorTextoSecundario;
                _btnCancelar.FlatStyle = FlatStyle.Flat;
                _btnCancelar.FlatAppearance.BorderColor = RecursosUI.ColorBorde;
                _btnCancelar.Font = RecursosUI.FuenteNegrita;
                _btnCancelar.Location = new Point(218, 12);
                _btnCancelar.Size = new Size(140, 38);
                _btnCancelar.Cursor = Cursors.Hand;
                _btnCancelar.TabIndex = 3;

                // 5. Suscripción a Eventos
                _txtFiltro.TextChanged += TxtFiltro_TextChanged;
                _txtFiltro.KeyDown += TxtFiltro_KeyDown;
                _dgvResultados.KeyDown += DgvResultados_KeyDown;
                _dgvResultados.CellDoubleClick += (s, e) => ElegirSeleccionado();
                _btnSeleccionar.Click += (s, e) => ElegirSeleccionado();
                _btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                this.Load += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(_filtroInicial))
                    {
                        _txtFiltro.Text = _filtroInicial;
                        _txtFiltro.SelectionStart = _txtFiltro.Text.Length;
                    }
                    FiltrarEnMemoria(_txtFiltro.Text);
                    this.ActiveControl = _txtFiltro;
                };

                // 7. Agregado de Controles
                _pnlTop.Controls.Add(_lblTitulo);
                _pnlTop.Controls.Add(_txtFiltro);

                _pnlBottom.Controls.Add(_btnSeleccionar);
                _pnlBottom.Controls.Add(_btnCancelar);

                this.Controls.Add(_dgvResultados);
                this.Controls.Add(_pnlBottom);
                this.Controls.Add(_pnlTop);
            }
            finally
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        /// <summary>
        /// Limpia los signos diacríticos (acentos, diéresis) descomponiendo en FormD
        /// y descartando caracteres UnicodeCategory.NonSpacingMark.
        /// </summary>
        private static string QuitarAcentos(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(textoNormalizado.Length);

            foreach (char c in textoNormalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void TxtFiltro_TextChanged(object? sender, EventArgs e)
        {
            FiltrarEnMemoria(_txtFiltro.Text);
        }

        private void EnfocarCuadroBusqueda()
        {
            _txtFiltro.Focus();
            _txtFiltro.SelectionStart = _txtFiltro.Text.Length;
            _txtFiltro.SelectionLength = 0;
        }

        private void TxtFiltro_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && _dgvResultados.Rows.Count > 0)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _dgvResultados.Focus();

                int targetRow = 0;
                if (_dgvResultados.SelectedRows.Count > 0 && _dgvResultados.SelectedRows[0].Index < _dgvResultados.Rows.Count - 1)
                {
                    targetRow = _dgvResultados.SelectedRows[0].Index;
                }

                _dgvResultados.CurrentCell = _dgvResultados.Rows[targetRow].Cells["Nombre"];
                _dgvResultados.Rows[targetRow].Selected = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ElegirSeleccionado();
            }
        }

        private void DgvResultados_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ElegirSeleccionado();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                EnfocarCuadroBusqueda();
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                int filaActual = _dgvResultados.CurrentRow?.Index
                    ?? (_dgvResultados.SelectedRows.Count > 0 ? _dgvResultados.SelectedRows[0].Index : 0);

                if (filaActual <= 0)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    EnfocarCuadroBusqueda();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (_dgvResultados.Focused)
                {
                    EnfocarCuadroBusqueda();
                    return true;
                }

                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FiltrarEnMemoria(string busqueda)
        {
            _dgvResultados.Rows.Clear();
            string terminoLimpio = QuitarAcentos(busqueda?.Trim());

            var filtrados = string.IsNullOrWhiteSpace(terminoLimpio)
                ? _catalogo.Take(60).ToList()
                : _catalogo.Where(p =>
                    QuitarAcentos(p.Nombre).IndexOf(terminoLimpio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    QuitarAcentos(p.CodigoBarras).IndexOf(terminoLimpio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    QuitarAcentos(p.CodigoInterno).IndexOf(terminoLimpio, StringComparison.OrdinalIgnoreCase) >= 0
                ).Take(60).ToList();

            foreach (var p in filtrados)
            {
                _dgvResultados.Rows.Add(
                    p.Id,
                    p.Nombre,
                    !string.IsNullOrWhiteSpace(p.CodigoBarras) ? p.CodigoBarras : p.CodigoInterno,
                    $"$ {p.PrecioVenta:N2}",
                    p.EsFraccionado ? $"{p.StockActual:0.000} Kg" : p.StockActual.ToString("0.##")
                );
            }

            if (_dgvResultados.Rows.Count > 0)
            {
                _dgvResultados.CurrentCell = _dgvResultados.Rows[0].Cells["Nombre"];
                _dgvResultados.Rows[0].Selected = true;
            }
        }

        private void ElegirSeleccionado()
        {
            if (_dgvResultados.SelectedRows.Count == 0 && _dgvResultados.CurrentRow == null) return;

            DataGridViewRow fila = _dgvResultados.SelectedRows.Count > 0
                ? _dgvResultados.SelectedRows[0]
                : _dgvResultados.CurrentRow!;

            int idProd = Convert.ToInt32(fila.Cells["Id"].Value);
            ProductoSeleccionado = _catalogo.FirstOrDefault(p => p.Id == idProd);

            if (ProductoSeleccionado != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
