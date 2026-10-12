using System.Reflection;

namespace UI
{
    public static class ThemeHelper
    {
        // Paleta formal y moderna (Slate & Indigo / Emerald)
        public static readonly Color FondoGeneral = Color.FromArgb(241, 245, 249);       // Slate 100
        public static readonly Color FondoCard = Color.White;
        public static readonly Color EncabezadoOscuro = Color.FromArgb(15, 23, 42);      // Slate 900
        public static readonly Color SubtituloOscuro = Color.FromArgb(71, 85, 105);      // Slate 600
        public static readonly Color BordeSuave = Color.FromArgb(203, 213, 225);         // Slate 300

        public static readonly Color ColorPrimario = Color.FromArgb(30, 41, 59);         // Slate 800
        public static readonly Color ColorExito = Color.FromArgb(16, 149, 104);          // Emerald 600
        public static readonly Color ColorAccion = Color.FromArgb(37, 99, 235);          // Blue 600
        public static readonly Color ColorPeligro = Color.FromArgb(220, 38, 38);         // Red 600
        public static readonly Color ColorAlerta = Color.FromArgb(217, 119, 6);          // Amber 600
        public static readonly Color ColorInfo = Color.FromArgb(79, 70, 229);            // Indigo 600
        public static readonly Color ColorNeutro = Color.FromArgb(100, 116, 139);        // Slate 500

        public static readonly Font FuenteTitulo = new("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font FuenteSubtitulo = new("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font FuenteControles = new("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font FuenteBotones = new("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FuenteNumericaGrande = new("Segoe UI", 22F, FontStyle.Bold);

        /// <summary>
        /// Aplica diseño formal y optimización sin parpadeos a un DataGridView.
        /// </summary>
        public static void FormatearGrilla(DataGridView dgv)
        {
            // Habilitar DoubleBuffered para navegación fluida con flechas sin parpadeo
            try
            {
                var dgvType = dgv.GetType();
                var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi?.SetValue(dgv, true, null);
            }
            catch { }

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgv.RowTemplate.Height = 32;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
        }

        /// <summary>
        /// Aplica estilo formal a un botón.
        /// </summary>
        public static void FormatearBoton(Button btn, Color fondo, Color texto)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = fondo;
            btn.ForeColor = texto;
            btn.Font = FuenteBotones;
            btn.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Validación para admitir exclusivamente dígitos y un único separador decimal.
        /// </summary>
        public static void ValidarSoloDecimales(KeyPressEventArgs e, string textoActual)
        {
            // Permitir teclas de control como Backspace
            if (char.IsControl(e.KeyChar)) return;

            // Reemplazar punto por coma según cultura
            if (e.KeyChar is '.' or ',')
            {
                e.KeyChar = ',';
                if (textoActual.Contains(','))
                {
                    e.Handled = true; // Ya existe una coma
                }
                return;
            }

            // Solo permitir dígitos
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Validación para admitir exclusivamente números enteros (para unidades enteras o IDs).
        /// </summary>
        public static void ValidarSoloEnteros(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Conecta un TextBox con un DataGridView para navegación fluida por flechitas (Arriba / Abajo).
        /// Si está en el TextBox y presiona Abajo -> Salta a la grilla.
        /// Si está en la grilla y presiona Arriba en la primera fila -> Vuelve al TextBox.
        /// </summary>
        public static void VincularNavegacionFlechas(TextBox txt, DataGridView dgv, Action? onSeleccionar = null)
        {
            txt.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Down && dgv.Rows.Count > 0)
                {
                    e.SuppressKeyPress = true;
                    dgv.Focus();
                    if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0)
                    {
                        dgv.Rows[0].Selected = true;
                    }
                }
            };

            dgv.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Up && dgv.CurrentRow != null && dgv.CurrentRow.Index == 0)
                {
                    e.SuppressKeyPress = true;
                    txt.Focus();
                    txt.SelectAll();
                }
                else if (e.KeyCode == Keys.Enter && onSeleccionar != null)
                {
                    e.SuppressKeyPress = true;
                    onSeleccionar();
                }
            };
        }
    }
}

