using System;
using System.Drawing;

namespace KioscoLite.CapaPresentacion
{
    /// <summary>
    /// Gestor estático centralizado de recursos GDI+ (Colores, Fuentes, Pinceles y Plumas).
    /// Previene fugas de memoria (GDI Object Leaks) prohibiendo la instanciación directa en formularios.
    /// </summary>
    public static class RecursosUI
    {
        private static bool _inicializado;

        // =========================================================================
        // 1. PALETA DE COLORES (Flat / Material Design Minimalista de Alto Contraste)
        // =========================================================================

        // Fondos y Superficies
        public static readonly Color ColorFondoVentana = Color.FromArgb(240, 244, 248);     // Gris azulado muy claro Flat
        public static readonly Color ColorSuperficie = Color.FromArgb(255, 255, 255);       // Blanco puro - Tarjetas y paneles
        public static readonly Color ColorSuperficieSuave = Color.FromArgb(241, 245, 249);  // Slate 100 - Cabeceras de grillas y cajas
        public static readonly Color ColorEncabezado = Color.FromArgb(15, 23, 42);          // Slate 900 - Cabecera principal
        public static readonly Color ColorBarraEstado = Color.FromArgb(30, 41, 59);         // Slate 800 - Pie de ventana

        // Tipografía y Bordes
        public static readonly Color ColorTextoPrincipal = Color.FromArgb(15, 23, 42);      // Slate 900 - Lectura nítida
        public static readonly Color ColorTextoAzulOscuro = Color.FromArgb(30, 58, 138);    // Azul institucional #1E3A8A
        public static readonly Color ColorTextoSecundario = Color.FromArgb(71, 85, 105);    // Slate 600 - Subtítulos y descripciones
        public static readonly Color ColorTextoMuted = Color.FromArgb(100, 116, 139);       // Slate 500 - Placeholders y etiquetas suaves
        public static readonly Color ColorTextoClaro = Color.FromArgb(248, 250, 252);       // Slate 50  - Texto sobre fondos oscuros
        public static readonly Color ColorBorde = Color.FromArgb(203, 213, 225);            // Slate 300 - Bordes de controles
        public static readonly Color ColorSeparador = Color.FromArgb(226, 232, 240);        // Slate 200 - Líneas divisorias y grillas
        public static readonly Color ColorSeleccionGrilla = Color.FromArgb(224, 242, 254);  // Sky 100   - Fila seleccionada

        // Acentos Semánticos Generales
        public static readonly Color ColorPrimario = Color.FromArgb(37, 99, 235);           // Blue 600   - Acción principal del sistema
        public static readonly Color ColorPrimarioHover = Color.FromArgb(29, 78, 216);      // Blue 700
        public static readonly Color ColorExito = Color.FromArgb(16, 185, 129);             // Emerald 500- Botones de confirmación / cobro
        public static readonly Color ColorPeligro = Color.FromArgb(220, 38, 38);            // Red 600    - Eliminar / Errores
        public static readonly Color ColorPeligroHover = Color.FromArgb(185, 28, 28);       // Red 700
        public static readonly Color ColorNeutro = Color.FromArgb(148, 163, 184);           // Slate 400  - Botones secundarios
        public static readonly Color ColorAlertaStockFondo = Color.FromArgb(254, 226, 226); // Red 100    - Fondo fila stock negativo
        public static readonly Color ColorAlertaStockSel = Color.FromArgb(252, 165, 165);   // Red 300    - Selección fila stock negativo
        public static readonly Color ColorFondoPanelTotal = Color.FromArgb(236, 242, 248);  // Gris azulado suave - Fondo Panel Total Caja
        public static readonly Color ColorTextoTotalCaja = Color.FromArgb(4, 120, 87);      // Verde oscuro (Emerald 700) - Monto Total Caja
        public static readonly Color ColorResaltadoExitoFondo = Color.FromArgb(220, 252, 231); // Emerald 100 - Fondo suave columnas destacadas
        public static readonly Color ColorResaltadoExitoSel = Color.FromArgb(167, 243, 208);   // Emerald 200 - Selección columnas destacadas
        public static readonly Color ColorFondoTicketTermico = Color.FromArgb(255, 253, 231); // Amarillo muy pálido / Crema - Papel térmico

        // Paleta Armónica de Módulos Principales (Flat / Material Design Descanso Visual)
        public static readonly Color ColorCaja = Color.FromArgb(43, 108, 176);              // Steel Blue 700 - Módulo 1: Caja
        public static readonly Color ColorCajaHover = Color.FromArgb(44, 82, 130);          // Steel Blue 800
        public static readonly Color ColorInventario = Color.FromArgb(49, 151, 149);        // Teal 600       - Módulo 2: Inventario
        public static readonly Color ColorInventarioHover = Color.FromArgb(44, 122, 123);   // Teal 700
        public static readonly Color ColorMovimientos = Color.FromArgb(197, 106, 45);       // Terracotta 600 - Módulo 3: Movimientos
        public static readonly Color ColorMovimientosHover = Color.FromArgb(168, 85, 32);   // Terracotta 700
        public static readonly Color ColorReportes = Color.FromArgb(109, 93, 158);          // Slate Plum 600 - Módulo 4: Reportes
        public static readonly Color ColorReportesHover = Color.FromArgb(88, 74, 133);      // Slate Plum 700

        // =========================================================================
        // 2. FUENTES TIPOGRÁFICAS COMPARTIDAS (Únicas instancias en toda la sesión)
        // =========================================================================
        public static Font FuenteTotalCaja { get; private set; } = null!;
        public static Font FuenteDisplayTotal { get; private set; } = null!;
        public static Font FuenteTituloModulo { get; private set; } = null!;
        public static Font FuenteTituloApp { get; private set; } = null!;
        public static Font FuenteInputGrande { get; private set; } = null!;
        public static Font FuenteEncabezado { get; private set; } = null!;
        public static Font FuenteBotonModulo { get; private set; } = null!;
        public static Font FuenteAtajoNumero { get; private set; } = null!;
        public static Font FuenteGrillaHeader { get; private set; } = null!;
        public static Font FuenteInput { get; private set; } = null!;
        public static Font FuenteSubtituloApp { get; private set; } = null!;
        public static Font FuenteEstandar { get; private set; } = null!;
        public static Font FuenteNegrita { get; private set; } = null!;
        public static Font FuentePequena { get; private set; } = null!;
        public static Font FuentePequenaNegrita { get; private set; } = null!;
        public static Font FuenteMonospace { get; private set; } = null!;
        public static Font FuenteTicketTermico { get; private set; } = null!;
        public static Font FuenteTicketNegrita { get; private set; } = null!;

        // =========================================================================
        // 3. PINCELES Y PLUMAS GDI+ COMPARTIDOS
        // =========================================================================
        public static SolidBrush BrushTextoPrincipal { get; private set; } = null!;
        public static SolidBrush BrushTextoSecundario { get; private set; } = null!;
        public static SolidBrush BrushSuperficie { get; private set; } = null!;
        public static Pen PenBorde { get; private set; } = null!;
        public static Pen PenFocoPrimario { get; private set; } = null!;

        /// <summary>
        /// Pre-carga en memoria las instancias únicas de recursos GDI+ al iniciar la aplicación.
        /// </summary>
        public static void Inicializar()
        {
            if (_inicializado) return;

            FuenteTotalCaja = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteDisplayTotal = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteTituloModulo = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteTituloApp = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteInputGrande = new Font("Segoe UI", 13.5F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteEncabezado = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteBotonModulo = new Font("Segoe UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteAtajoNumero = new Font("Consolas", 14F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteGrillaHeader = new Font("Segoe UI", 10.25F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteInput = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            FuenteSubtituloApp = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            FuenteEstandar = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            FuenteNegrita = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            FuentePequena = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            FuentePequenaNegrita = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            FuenteMonospace = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            FuenteTicketTermico = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            FuenteTicketNegrita = new Font("Consolas", 10.5F, FontStyle.Bold, GraphicsUnit.Point);

            BrushTextoPrincipal = new SolidBrush(ColorTextoPrincipal);
            BrushTextoSecundario = new SolidBrush(ColorTextoSecundario);
            BrushSuperficie = new SolidBrush(ColorSuperficie);
            PenBorde = new Pen(ColorBorde, 1f);
            PenFocoPrimario = new Pen(ColorPrimario, 2f);

            _inicializado = true;
        }

        /// <summary>
        /// Libera de forma determinista todos los handles GDI+ al cerrar el sistema.
        /// </summary>
        public static void LiberarRecursos()
        {
            if (!_inicializado) return;

            FuenteTotalCaja?.Dispose();
            FuenteDisplayTotal?.Dispose();
            FuenteTituloModulo?.Dispose();
            FuenteTituloApp?.Dispose();
            FuenteInputGrande?.Dispose();
            FuenteEncabezado?.Dispose();
            FuenteBotonModulo?.Dispose();
            FuenteAtajoNumero?.Dispose();
            FuenteGrillaHeader?.Dispose();
            FuenteInput?.Dispose();
            FuenteSubtituloApp?.Dispose();
            FuenteEstandar?.Dispose();
            FuenteNegrita?.Dispose();
            FuentePequena?.Dispose();
            FuentePequenaNegrita?.Dispose();
            FuenteMonospace?.Dispose();
            FuenteTicketTermico?.Dispose();
            FuenteTicketNegrita?.Dispose();

            BrushTextoPrincipal?.Dispose();
            BrushTextoSecundario?.Dispose();
            BrushSuperficie?.Dispose();
            PenBorde?.Dispose();
            PenFocoPrimario?.Dispose();

            _inicializado = false;
        }
    }
}
