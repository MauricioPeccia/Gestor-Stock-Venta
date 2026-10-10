# SPEC.md — Especificación Técnica de Software
## Sistema de Gestión para Drugstore / Kiosco (Escritorio Windows, Offline)

| Campo | Valor |
|---|---|
| **Nombre en clave** | `DrugstorePOS` |
| **Versión del documento** | 1.0 |
| **Fecha** | 08/10/2026 |
| **Estado** | Aprobado para implementación |
| **Plataforma** | Windows 10/11 (x64) |
| **Stack** | C# / .NET (WinForms) / SQLite embebido |
| **Alcance** | Aplicación local, monousuario, 100% offline, sin autenticación |

---

## Tabla de contenidos

1. [Resumen ejecutivo y arquitectura general](#1-resumen-ejecutivo-y-arquitectura-general)
2. [Modelo y esquema de base de datos (SQLite)](#2-modelo-y-esquema-de-base-de-datos-sqlite)
3. [Definición de entidades y modelos de dominio](#3-definición-de-entidades-y-modelos-de-dominio)
4. [Contratos de servicios y casos de uso](#4-contratos-de-servicios-y-casos-de-uso)
5. [Especificación de interfaces de usuario](#5-especificación-de-interfaces-de-usuario)
6. [Formato del ticket de venta](#6-formato-del-ticket-de-venta)
7. [Consideraciones técnicas y manejo de errores](#7-consideraciones-técnicas-y-manejo-de-errores)
8. [Criterios de aceptación y plan de implementación](#8-criterios-de-aceptación-y-plan-de-implementación)

---

# 1. Resumen ejecutivo y arquitectura general

## 1.1 Objetivo

Construir una aplicación de escritorio para un drugstore/kiosco minorista que permita **controlar el inventario, actualizar precios, vender rápidamente desde un punto de venta (POS) y consultar reportes de gestión en pantalla**. La aplicación se ejecuta en una única PC, sin conexión a internet y sin login: al abrirla, el operador queda directamente en el sistema.

## 1.2 Alcance

### Dentro del alcance
- Gestión de productos: alta, baja (lógica), modificación, consulta rápida por código de barras o nombre, alerta de stock mínimo.
- Gestión de rubros (categorías) y proveedores (catálogos simples).
- Actualización de precios: edición manual rápida y actualización masiva por porcentaje filtrada por rubro y/o proveedor.
- Punto de venta: carrito, totales, pago en efectivo o débito/transferencia, cálculo de vuelto, ticket de texto plano para comandera de 58/80 mm.
- Anulación de ventas (con reposición de stock).
- Reportes en pantalla: ventas diarias/mensuales, ganancia estimada, productos más vendidos.
- Respaldo local de la base de datos.

### Fuera de alcance (v1.0)
- Autenticación, usuarios, roles o permisos.
- Facturación electrónica / integración fiscal (el ticket es un comprobante interno **no fiscal**).
- Sincronización en la nube, multi-sucursal o multi-PC.
- Exportación de reportes (PDF/Excel).
- Compras a proveedores / órdenes de compra (el ingreso de mercadería se registra como ajuste de stock).
- Cuenta corriente de clientes, fiado, descuentos por promoción y combos.
- Productos fraccionables (venta por peso). Todas las cantidades son **enteras** (ver §7.8, extensibilidad).

## 1.3 Decisiones técnicas clave

| Decisión | Elección | Justificación |
|---|---|---|
| Runtime | **.NET LTS vigente** (`net10.0-windows`; compatible con `net8.0-windows`) | Soporte a largo plazo, rendimiento y despliegue self-contained. |
| UI | **Windows Forms** | Madurez, `DataGridView` robusto, baja curva, atajos de teclado y foco nativos, ideal para POS con lector de código de barras (emula teclado). |
| Persistencia | **SQLite** vía `Microsoft.Data.Sqlite` | Archivo único embebido, sin servidor, transaccional (ACID). |
| Acceso a datos | **ADO.NET + Dapper** (micro-ORM) | SQL explícito y controlable, sin la sobrecarga de un ORM completo; transacciones simples. |
| Patrón | **Arquitectura N-Capas** + Repository + Unit of Work | Separación de responsabilidades, testeabilidad de la lógica de negocio. |
| UI pattern | **MVP pasivo** (Presenter por pantalla) | Evita lógica en los Forms; permite probar presenters sin UI. |
| Inyección de dependencias | `Microsoft.Extensions.DependencyInjection` | Composición centralizada en `Program.cs`. |
| Dinero | **`decimal` en C#** / **`INTEGER` en centavos** en SQLite | Elimina errores de punto flotante. Conversión únicamente en la capa de repositorio. |
| Fechas | `TEXT` ISO-8601 local (`yyyy-MM-dd HH:mm:ss`) | Orden lexicográfico = cronológico; consultas por rango indexables. |
| Logging | `Serilog` (archivo rotativo local) o `Microsoft.Extensions.Logging` + archivo | Diagnóstico offline. |
| Testing | `xUnit` + SQLite en memoria / archivo temporal | Pruebas de servicios y repositorios. |

## 1.4 Patrón de arquitectura: N-Capas

```
┌────────────────────────────────────────────────────────────────┐
│  Capa de Presentación  (DrugstorePOS.UI — WinForms)            │
│  Forms / UserControls  ←→  Presenters (MVP)                    │
├────────────────────────────────────────────────────────────────┤
│  Capa de Lógica de Negocio  (DrugstorePOS.Business)            │
│  Servicios de aplicación · Validaciones · Reglas · Casos de uso│
│  Formateador de ticket · Cálculo de precios/vuelto/reportes    │
├────────────────────────────────────────────────────────────────┤
│  Capa de Acceso a Datos  (DrugstorePOS.Data)                   │
│  Repositorios · Unit of Work · Migraciones · Backup            │
├────────────────────────────────────────────────────────────────┤
│  SQLite (archivo drugstore.db — WAL)                           │
└────────────────────────────────────────────────────────────────┘
        ▲ Todas las capas dependen de DrugstorePOS.Domain
          (entidades, enums, excepciones, interfaces/contratos)
```

**Reglas de dependencia (obligatorias):**
1. `Domain` no referencia a ningún otro proyecto.
2. `Business` referencia solo a `Domain` (usa interfaces de repositorio definidas en `Domain`).
3. `Data` referencia solo a `Domain` (implementa las interfaces).
4. `UI` referencia a `Domain` y `Business`; **no** referencia a `Data` directamente (la composición se hace en un *Composition Root* que sí conoce `Data`, ubicado en `Program.cs` mediante una extensión de DI de `Data`).
5. Ningún Form ejecuta SQL ni contiene reglas de negocio.

## 1.5 Estructura de la solución (.sln)

```
DrugstorePOS/
├── DrugstorePOS.sln
├── SPEC.md
├── Directory.Build.props                 # TargetFramework, Nullable, LangVersion, Analyzers
├── .editorconfig
├── src/
│   ├── DrugstorePOS.Domain/              # Class Library
│   │   ├── Entities/                     # Producto, Rubro, Proveedor, Venta, VentaItem, ...
│   │   ├── Enums/                        # MetodoPago, TipoMovimientoStock, EstadoVenta, ...
│   │   ├── Exceptions/                   # DomainException, StockInsuficienteException, ...
│   │   ├── Dtos/                         # Filtros, resultados de reportes, resultados de operación
│   │   └── Contracts/
│   │       ├── Repositories/             # IProductoRepository, IVentaRepository, ...
│   │       └── Services/                 # (interfaces de servicios, ver §4)
│   │
│   ├── DrugstorePOS.Business/            # Class Library
│   │   ├── Services/                     # ProductoService, PrecioService, VentaService, ...
│   │   ├── Validators/                   # Validación de entradas (sin dependencia de UI)
│   │   ├── Ticketing/                    # TicketFormatter (texto plano 32/48 columnas)
│   │   └── Common/                       # Money helpers, redondeo, normalización de texto
│   │
│   ├── DrugstorePOS.Data/                # Class Library
│   │   ├── Sqlite/
│   │   │   ├── SqliteConnectionFactory.cs
│   │   │   ├── SqliteUnitOfWork.cs
│   │   │   └── DatabaseInitializer.cs    # crea BD, aplica PRAGMAs y migraciones
│   │   ├── Repositories/                 # ProductoRepository, VentaRepository, ...
│   │   ├── Migrations/                   # 001_initial.sql, 002_*.sql (embebidos como recurso)
│   │   ├── Backup/                       # BackupService (API de backup de SQLite)
│   │   └── DependencyInjection.cs        # AddDataLayer(this IServiceCollection, ...)
│   │
│   └── DrugstorePOS.UI/                  # WinForms App (OutputType=WinExe)
│       ├── Program.cs                    # Composition Root, manejo global de excepciones
│       ├── MainForm.cs                   # Shell + navegación + atajos globales
│       ├── Views/
│       │   ├── Ventas/                   # VentasView (UserControl) + IVentasView
│       │   ├── Inventario/               # InventarioView, ProductoEditForm
│       │   ├── Precios/                  # PreciosView, ActualizacionMasivaForm
│       │   ├── Reportes/                 # ReportesView
│       │   ├── Catalogos/                # RubrosForm, ProveedoresForm
│       │   └── Sistema/                  # ConfiguracionForm, RespaldoForm, CobroForm
│       ├── Presenters/                   # VentasPresenter, InventarioPresenter, ...
│       ├── Printing/                     # RawTextPrinter (PrintDocument) / EscPosPrinter (opcional)
│       ├── Controls/                     # Controles reutilizables (MoneyTextBox, SearchBox)
│       └── Resources/                    # Íconos, sonidos (beep), etc.
│
└── tests/
    ├── DrugstorePOS.Business.Tests/
    └── DrugstorePOS.Data.Tests/
```

## 1.6 Paquetes NuGet

| Proyecto | Paquete | Uso |
|---|---|---|
| Data | `Microsoft.Data.Sqlite` | Proveedor ADO.NET para SQLite (incluye binarios nativos). |
| Data | `Dapper` | Mapeo ligero SQL → objetos. |
| Business / UI / Data | `Microsoft.Extensions.DependencyInjection` | DI. |
| UI / Data | `Serilog`, `Serilog.Sinks.File` | Logging a archivo. |
| UI | `ScottPlot.WinForms` *(o `System.Windows.Forms.DataVisualization`)* | Gráficos de barras/líneas en reportes. Alternativa sin dependencias: `DataGridView` + barras con `Panel`. |
| Tests | `xunit`, `FluentAssertions`, `Moq` (o `NSubstitute`) | Pruebas. |

## 1.7 Composition Root (resumen)

`Program.cs` debe: (1) habilitar `ApplicationConfiguration.Initialize()`, (2) configurar el logger, (3) registrar manejadores globales de excepciones, (4) construir el `ServiceProvider` (`AddDataLayer`, `AddBusinessLayer`, `AddPresenters`), (5) invocar `DatabaseInitializer.Initialize()` (crea el archivo, aplica migraciones y respaldo previo si hay migración pendiente), (6) ejecutar el backup automático diario si corresponde, (7) abrir `MainForm`. Se garantiza **instancia única** mediante `Mutex` con nombre (evita dos procesos escribiendo el mismo `.db`).

## 1.8 Requerimientos no funcionales

| ID | Requerimiento | Criterio medible |
|---|---|---|
| RNF-01 | Búsqueda por código de barras | Resultado en **< 50 ms** con 50.000 productos. |
| RNF-02 | Agregar producto al carrito (escaneo) | Percepción instantánea (< 100 ms) incluyendo refresco de grilla. |
| RNF-03 | Confirmación de venta (transacción + ticket) | < 500 ms con hasta 50 ítems. |
| RNF-04 | Arranque en frío | < 3 s en PC de gama baja (HDD no recomendado; SSD sugerido). |
| RNF-05 | Integridad | Ninguna venta puede quedar parcial (ver §7.1). |
| RNF-06 | Offline | Cero llamadas de red; sin telemetría. |
| RNF-07 | Operación 100% por teclado en el POS | Todas las acciones del flujo de venta con atajos (§5). |
| RNF-08 | Localización | UI en español (es-AR por defecto), moneda `$`, separador de miles `.` y decimal `,` (configurable por `CultureInfo`). |
| RNF-09 | Resolución mínima | 1366×768; UI escalable con DPI (`AutoScaleMode.Dpi`). |
| RNF-10 | Resiliencia | Cierre abrupto o corte de energía no corrompe la BD (WAL + transacciones). |

---

# 2. Modelo y esquema de base de datos (SQLite)

## 2.1 Convenciones

- **Archivo:** `%LocalAppData%\DrugstorePOS\data\drugstore.db` (ruta configurable; modo portable opcional junto al `.exe`).
- **Nombres:** tablas en `PascalCase` plural, columnas en `snake_case`.
- **PK:** `id INTEGER PRIMARY KEY AUTOINCREMENT` (alias de `rowid`).
- **Dinero:** `INTEGER` en **centavos** (sufijo `_centavos`). Ejemplo: `$1.234,50` → `123450`.
- **Booleanos:** `INTEGER` con `CHECK (col IN (0,1))`.
- **Fechas:** `TEXT` `yyyy-MM-dd HH:mm:ss` en hora local.
- **Borrado:** lógico (`activo = 0`) para productos, rubros y proveedores, para preservar el historial de ventas. Las ventas **no se borran**; se anulan.
- **Integridad referencial:** `PRAGMA foreign_keys = ON` en **cada** conexión.

## 2.2 Diagrama de tablas (modelo entidad-relación en texto)

```
┌──────────────┐ 1      N ┌─────────────────────────┐ N      1 ┌──────────────┐
│   Rubros     │──────────│        Productos        │──────────│ Proveedores  │
│──────────────│          │─────────────────────────│          │──────────────│
│ PK id        │          │ PK id                   │          │ PK id        │
│ nombre (UQ)  │          │ UQ codigo_barras (null) │          │ nombre (UQ)  │
│ activo       │          │    nombre               │          │ telefono     │
└──────────────┘          │    nombre_busqueda      │          │ notas        │
                          │ FK rubro_id             │          │ activo       │
                          │ FK proveedor_id (null)  │          └──────────────┘
                          │    precio_costo_centavos│
                          │    precio_venta_centavos│
                          │    stock_actual         │
                          │    stock_minimo         │
                          │    activo               │
                          │    fecha_alta / fecha_mod│
                          └───────┬───────┬─────────┘
                       1          │       │          1
        ┌─────────────────────────┘       └──────────────────────┐
        │ N                                                      │ N
┌───────┴─────────────┐                              ┌───────────┴──────────┐
│  MovimientosStock   │                              │   HistorialPrecios   │
│─────────────────────│                              │──────────────────────│
│ PK id               │                              │ PK id                │
│ FK producto_id      │                              │ FK producto_id       │
│    tipo             │                              │    costo_ant / nuevo │
│    cantidad (±)     │                              │    venta_ant / nuevo │
│    stock_resultante │                              │    origen, lote_id   │
│ FK venta_id (null)  │                              │    fecha_hora        │
│    motivo, fecha    │                              └──────────────────────┘
└───────┬─────────────┘
        │ N
        │ 0..1
┌───────┴─────────────┐ 1      N ┌─────────────────────────┐
│       Ventas        │──────────│       VentaItems        │
│─────────────────────│          │─────────────────────────│
│ PK id               │          │ PK id                   │
│ UQ numero           │          │ FK venta_id             │
│    fecha_hora       │          │ FK producto_id          │
│    total_centavos   │          │    codigo_barras (snap) │
│    costo_total_cent.│          │    nombre (snap)        │
│    metodo_pago      │          │    cantidad             │
│    monto_recibido   │          │    precio_unitario      │
│    vuelto           │          │    costo_unitario       │
│    estado           │          │    subtotal             │
│    datos anulación  │          └─────────────────────────┘
└─────────────────────┘

┌───────────────┐   ┌──────────────────┐
│ Configuracion │   │  (PRAGMA user_   │
│ clave PK/valor│   │   version = nº   │
└───────────────┘   │   de migración)  │
                    └──────────────────┘
```

## 2.3 Diccionario de datos

### 2.3.1 `Rubros`
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK, AUTOINCREMENT | Identificador. |
| `nombre` | TEXT | NOT NULL, UNIQUE, `COLLATE NOCASE` | Ej.: "Bebidas", "Golosinas", "Cigarrillos", "Farmacia", "Limpieza". |
| `activo` | INTEGER | NOT NULL, DEFAULT 1, CHECK (0/1) | Baja lógica. |

### 2.3.2 `Proveedores`
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK, AUTOINCREMENT | Identificador. |
| `nombre` | TEXT | NOT NULL, UNIQUE, `COLLATE NOCASE` | Razón social / nombre de fantasía. |
| `telefono` | TEXT | NULL | Contacto. |
| `notas` | TEXT | NULL | Observaciones. |
| `activo` | INTEGER | NOT NULL, DEFAULT 1 | Baja lógica. |

### 2.3.3 `Productos`
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK, AUTOINCREMENT | Identificador interno. |
| `codigo_barras` | TEXT | NULL, **UNIQUE parcial** | EAN-13/EAN-8/UPC o código interno. `NULL` si no posee. Se normaliza con `Trim()`. |
| `nombre` | TEXT | NOT NULL | Nombre comercial a mostrar. |
| `nombre_busqueda` | TEXT | NOT NULL | `nombre` en minúsculas y sin tildes, mantenido por la capa de negocio; usado para búsquedas. |
| `rubro_id` | INTEGER | NOT NULL, FK → `Rubros.id` | Rubro. |
| `proveedor_id` | INTEGER | NULL, FK → `Proveedores.id` | Proveedor habitual. |
| `precio_costo_centavos` | INTEGER | NOT NULL, DEFAULT 0, CHECK ≥ 0 | Costo unitario. |
| `precio_venta_centavos` | INTEGER | NOT NULL, CHECK ≥ 0 | Precio de venta al público. |
| `stock_actual` | INTEGER | NOT NULL, DEFAULT 0 | Unidades disponibles (puede ser negativo solo si la configuración lo permite). |
| `stock_minimo` | INTEGER | NOT NULL, DEFAULT 0, CHECK ≥ 0 | Umbral de alerta. |
| `activo` | INTEGER | NOT NULL, DEFAULT 1 | Baja lógica. |
| `fecha_alta` | TEXT | NOT NULL | Creación. |
| `fecha_modificacion` | TEXT | NOT NULL | Última modificación. |

### 2.3.4 `MovimientosStock` (kardex)
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK | |
| `producto_id` | INTEGER | NOT NULL, FK | |
| `tipo` | TEXT | NOT NULL, CHECK IN (`ALTA_INICIAL`,`INGRESO`,`AJUSTE`,`VENTA`,`ANULACION_VENTA`,`BAJA`) | Origen del movimiento. |
| `cantidad` | INTEGER | NOT NULL, CHECK ≠ 0 | **Delta con signo** (+ entra, − sale). |
| `stock_resultante` | INTEGER | NOT NULL | Stock luego del movimiento (auditoría). |
| `venta_id` | INTEGER | NULL, FK → `Ventas.id` | Si el movimiento proviene de una venta. |
| `motivo` | TEXT | NULL | Texto libre (ej.: "Rotura", "Vencimiento", "Recuento"). |
| `fecha_hora` | TEXT | NOT NULL | |

### 2.3.5 `HistorialPrecios`
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK | |
| `producto_id` | INTEGER | NOT NULL, FK | |
| `costo_anterior_centavos` / `costo_nuevo_centavos` | INTEGER | NOT NULL | |
| `venta_anterior_centavos` / `venta_nuevo_centavos` | INTEGER | NOT NULL | |
| `origen` | TEXT | NOT NULL, CHECK IN (`MANUAL`,`MASIVO`,`REVERSION`) | |
| `lote_id` | TEXT | NULL | GUID del lote de actualización masiva (permite revertir el lote completo). |
| `fecha_hora` | TEXT | NOT NULL | |

### 2.3.6 `Ventas`
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK | |
| `numero` | INTEGER | NOT NULL, UNIQUE | Correlativo visible en el ticket (inicia en 1). Asignado dentro de la transacción. |
| `fecha_hora` | TEXT | NOT NULL | Momento de confirmación. |
| `total_centavos` | INTEGER | NOT NULL, CHECK ≥ 0 | Total cobrado. |
| `costo_total_centavos` | INTEGER | NOT NULL, CHECK ≥ 0 | Σ (costo_unitario × cantidad) al momento de la venta (para ganancia estimada). |
| `metodo_pago` | TEXT | NOT NULL, CHECK IN (`EFECTIVO`,`DEBITO_TRANSFERENCIA`) | |
| `monto_recibido_centavos` | INTEGER | NULL | Solo efectivo. |
| `vuelto_centavos` | INTEGER | NOT NULL, DEFAULT 0 | Solo efectivo. |
| `estado` | TEXT | NOT NULL, DEFAULT `COMPLETADA`, CHECK IN (`COMPLETADA`,`ANULADA`) | |
| `fecha_anulacion` | TEXT | NULL | |
| `motivo_anulacion` | TEXT | NULL | |

### 2.3.7 `VentaItems`
| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `id` | INTEGER | PK | |
| `venta_id` | INTEGER | NOT NULL, FK → `Ventas.id` | |
| `producto_id` | INTEGER | NOT NULL, FK → `Productos.id` | |
| `codigo_barras` | TEXT | NULL | *Snapshot*. |
| `nombre` | TEXT | NOT NULL | *Snapshot* (el ticket histórico no cambia si se renombra el producto). |
| `cantidad` | INTEGER | NOT NULL, CHECK > 0 | |
| `precio_unitario_centavos` | INTEGER | NOT NULL, CHECK ≥ 0 | *Snapshot* del precio vigente al vender. |
| `costo_unitario_centavos` | INTEGER | NOT NULL, CHECK ≥ 0 | *Snapshot* del costo vigente al vender. |
| `subtotal_centavos` | INTEGER | NOT NULL, CHECK ≥ 0 | `cantidad × precio_unitario`. |

### 2.3.8 `Configuracion`
Tabla clave/valor para parámetros del sistema.

| Clave | Valor por defecto | Descripción |
|---|---|---|
| `comercio.nombre` | `MI DRUGSTORE` | Encabezado del ticket. |
| `comercio.direccion` | *(vacío)* | Línea 2 del ticket. |
| `comercio.telefono` | *(vacío)* | Línea 3 del ticket. |
| `ticket.ancho_papel_mm` | `58` | `58` → 32 columnas; `80` → 48 columnas. |
| `ticket.pie` | `¡GRACIAS POR SU COMPRA!` | Pie. |
| `ticket.sin_tildes` | `1` | Transliterar caracteres acentuados (compatibilidad con comanderas). |
| `ticket.imprimir_automatico` | `0` | Imprimir al confirmar sin preguntar. |
| `impresora.nombre` | *(vacío)* | Impresora de Windows asignada. |
| `stock.permitir_negativo` | `0` | Permite vender sin stock suficiente. |
| `precios.redondeo_multiplo_centavos` | `100` | Múltiplo de redondeo por defecto en actualización masiva ($1). |
| `backup.carpeta` | `%LocalAppData%\DrugstorePOS\backups` | Destino principal. |
| `backup.carpeta_secundaria` | *(vacío)* | Destino opcional (ej. pendrive). |
| `backup.retencion_diarios` | `14` | Cantidad de respaldos diarios conservados. |
| `backup.retencion_mensuales` | `6` | Cantidad de respaldos mensuales conservados. |
| `backup.ultimo_exitoso` | *(vacío)* | Marca de tiempo del último respaldo. |

## 2.4 Índices y optimización

| Índice | Tabla / columnas | Tipo | Propósito |
|---|---|---|---|
| `ux_productos_codigo_barras` | `Productos(codigo_barras) WHERE codigo_barras IS NOT NULL` | UNIQUE parcial | **Búsqueda exacta por código de barras en O(log n)** y unicidad. |
| `ix_productos_nombre_busqueda` | `Productos(nombre_busqueda)` | B-Tree | Búsqueda por prefijo (`LIKE 'texto%'`) rápida. |
| `ix_productos_rubro` | `Productos(rubro_id, activo)` | B-Tree | Filtros por rubro y actualización masiva. |
| `ix_productos_proveedor` | `Productos(proveedor_id, activo)` | B-Tree | Filtros por proveedor y actualización masiva. |
| `ix_productos_stock_bajo` | `Productos(stock_actual, stock_minimo) WHERE activo = 1` | B-Tree parcial | Listado de stock bajo. |
| `ix_ventas_fecha` | `Ventas(fecha_hora)` | B-Tree | Reportes por rango de fechas. |
| `ix_ventas_estado_fecha` | `Ventas(estado, fecha_hora)` | B-Tree | Reportes filtrando solo ventas completadas. |
| `ix_ventaitems_venta` | `VentaItems(venta_id)` | B-Tree | Carga de detalle de una venta. |
| `ix_ventaitems_producto` | `VentaItems(producto_id)` | B-Tree | Ranking de más vendidos. |
| `ix_movstock_producto_fecha` | `MovimientosStock(producto_id, fecha_hora)` | B-Tree | Kardex por producto. |
| `ix_histprecios_producto` | `HistorialPrecios(producto_id, fecha_hora)` | B-Tree | Historial de un producto. |
| `ix_histprecios_lote` | `HistorialPrecios(lote_id) WHERE lote_id IS NOT NULL` | B-Tree parcial | Reversión de lote. |

**Notas de consulta:**
- Búsqueda por texto: la capa de negocio normaliza el término (minúsculas, sin tildes) y ejecuta `nombre_busqueda LIKE @t || '%'` **UNION** `nombre_busqueda LIKE '% ' || @t || '%'` (coincidencia al inicio de cualquier palabra). Para catálogos > 100.000 productos, evaluar una tabla virtual FTS5.
- Reportes por fecha: **nunca** usar `date(fecha_hora) = ...` (invalida el índice). Usar rangos: `fecha_hora >= @desde AND fecha_hora < @hasta_exclusivo`.

## 2.5 Script SQL inicial (`Migrations/001_initial.sql`)

```sql
-- =====================================================================
-- DrugstorePOS - Migración 001 - Esquema inicial
-- Ejecutar con PRAGMA foreign_keys = ON en la conexión.
-- =====================================================================

BEGIN TRANSACTION;

-- ---------------------------------------------------------------------
-- Catálogos
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Rubros (
    id      INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre  TEXT    NOT NULL COLLATE NOCASE UNIQUE,
    activo  INTEGER NOT NULL DEFAULT 1 CHECK (activo IN (0, 1))
);

CREATE TABLE IF NOT EXISTS Proveedores (
    id        INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre    TEXT    NOT NULL COLLATE NOCASE UNIQUE,
    telefono  TEXT    NULL,
    notas     TEXT    NULL,
    activo    INTEGER NOT NULL DEFAULT 1 CHECK (activo IN (0, 1))
);

-- ---------------------------------------------------------------------
-- Productos
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Productos (
    id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    codigo_barras           TEXT    NULL,
    nombre                  TEXT    NOT NULL CHECK (length(trim(nombre)) > 0),
    nombre_busqueda         TEXT    NOT NULL,
    rubro_id                INTEGER NOT NULL,
    proveedor_id            INTEGER NULL,
    precio_costo_centavos   INTEGER NOT NULL DEFAULT 0 CHECK (precio_costo_centavos >= 0),
    precio_venta_centavos   INTEGER NOT NULL DEFAULT 0 CHECK (precio_venta_centavos >= 0),
    stock_actual            INTEGER NOT NULL DEFAULT 0,
    stock_minimo            INTEGER NOT NULL DEFAULT 0 CHECK (stock_minimo >= 0),
    activo                  INTEGER NOT NULL DEFAULT 1 CHECK (activo IN (0, 1)),
    fecha_alta              TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%d %H:%M:%S','now','localtime')),
    fecha_modificacion      TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%d %H:%M:%S','now','localtime')),
    FOREIGN KEY (rubro_id)     REFERENCES Rubros(id)       ON UPDATE CASCADE ON DELETE RESTRICT,
    FOREIGN KEY (proveedor_id) REFERENCES Proveedores(id)  ON UPDATE CASCADE ON DELETE SET NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_productos_codigo_barras
    ON Productos(codigo_barras) WHERE codigo_barras IS NOT NULL;
CREATE INDEX IF NOT EXISTS ix_productos_nombre_busqueda
    ON Productos(nombre_busqueda);
CREATE INDEX IF NOT EXISTS ix_productos_rubro
    ON Productos(rubro_id, activo);
CREATE INDEX IF NOT EXISTS ix_productos_proveedor
    ON Productos(proveedor_id, activo);
CREATE INDEX IF NOT EXISTS ix_productos_stock_bajo
    ON Productos(stock_actual, stock_minimo) WHERE activo = 1;

-- ---------------------------------------------------------------------
-- Ventas
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Ventas (
    id                       INTEGER PRIMARY KEY AUTOINCREMENT,
    numero                   INTEGER NOT NULL UNIQUE,
    fecha_hora               TEXT    NOT NULL,
    total_centavos           INTEGER NOT NULL CHECK (total_centavos >= 0),
    costo_total_centavos     INTEGER NOT NULL DEFAULT 0 CHECK (costo_total_centavos >= 0),
    metodo_pago              TEXT    NOT NULL CHECK (metodo_pago IN ('EFECTIVO','DEBITO_TRANSFERENCIA')),
    monto_recibido_centavos  INTEGER NULL CHECK (monto_recibido_centavos IS NULL OR monto_recibido_centavos >= 0),
    vuelto_centavos          INTEGER NOT NULL DEFAULT 0 CHECK (vuelto_centavos >= 0),
    estado                   TEXT    NOT NULL DEFAULT 'COMPLETADA' CHECK (estado IN ('COMPLETADA','ANULADA')),
    fecha_anulacion          TEXT    NULL,
    motivo_anulacion         TEXT    NULL
);

CREATE INDEX IF NOT EXISTS ix_ventas_fecha        ON Ventas(fecha_hora);
CREATE INDEX IF NOT EXISTS ix_ventas_estado_fecha ON Ventas(estado, fecha_hora);

CREATE TABLE IF NOT EXISTS VentaItems (
    id                         INTEGER PRIMARY KEY AUTOINCREMENT,
    venta_id                   INTEGER NOT NULL,
    producto_id                INTEGER NOT NULL,
    codigo_barras              TEXT    NULL,
    nombre                     TEXT    NOT NULL,
    cantidad                   INTEGER NOT NULL CHECK (cantidad > 0),
    precio_unitario_centavos   INTEGER NOT NULL CHECK (precio_unitario_centavos >= 0),
    costo_unitario_centavos    INTEGER NOT NULL DEFAULT 0 CHECK (costo_unitario_centavos >= 0),
    subtotal_centavos          INTEGER NOT NULL CHECK (subtotal_centavos >= 0),
    FOREIGN KEY (venta_id)    REFERENCES Ventas(id)    ON DELETE RESTRICT,
    FOREIGN KEY (producto_id) REFERENCES Productos(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS ix_ventaitems_venta    ON VentaItems(venta_id);
CREATE INDEX IF NOT EXISTS ix_ventaitems_producto ON VentaItems(producto_id);

-- ---------------------------------------------------------------------
-- Kardex de stock
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS MovimientosStock (
    id                INTEGER PRIMARY KEY AUTOINCREMENT,
    producto_id       INTEGER NOT NULL,
    tipo              TEXT    NOT NULL CHECK (tipo IN
                        ('ALTA_INICIAL','INGRESO','AJUSTE','VENTA','ANULACION_VENTA','BAJA')),
    cantidad          INTEGER NOT NULL CHECK (cantidad <> 0),
    stock_resultante  INTEGER NOT NULL,
    venta_id          INTEGER NULL,
    motivo            TEXT    NULL,
    fecha_hora        TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%d %H:%M:%S','now','localtime')),
    FOREIGN KEY (producto_id) REFERENCES Productos(id) ON DELETE RESTRICT,
    FOREIGN KEY (venta_id)    REFERENCES Ventas(id)    ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS ix_movstock_producto_fecha
    ON MovimientosStock(producto_id, fecha_hora);

-- ---------------------------------------------------------------------
-- Historial de precios
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS HistorialPrecios (
    id                        INTEGER PRIMARY KEY AUTOINCREMENT,
    producto_id               INTEGER NOT NULL,
    costo_anterior_centavos   INTEGER NOT NULL,
    costo_nuevo_centavos      INTEGER NOT NULL,
    venta_anterior_centavos   INTEGER NOT NULL,
    venta_nuevo_centavos      INTEGER NOT NULL,
    origen                    TEXT    NOT NULL CHECK (origen IN ('MANUAL','MASIVO','REVERSION')),
    lote_id                   TEXT    NULL,
    fecha_hora                TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%d %H:%M:%S','now','localtime')),
    FOREIGN KEY (producto_id) REFERENCES Productos(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS ix_histprecios_producto
    ON HistorialPrecios(producto_id, fecha_hora);
CREATE INDEX IF NOT EXISTS ix_histprecios_lote
    ON HistorialPrecios(lote_id) WHERE lote_id IS NOT NULL;

-- ---------------------------------------------------------------------
-- Configuración
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Configuracion (
    clave  TEXT PRIMARY KEY,
    valor  TEXT NOT NULL
);

INSERT OR IGNORE INTO Configuracion (clave, valor) VALUES
    ('comercio.nombre',                    'MI DRUGSTORE'),
    ('comercio.direccion',                 ''),
    ('comercio.telefono',                  ''),
    ('ticket.ancho_papel_mm',              '58'),
    ('ticket.pie',                         '¡GRACIAS POR SU COMPRA!'),
    ('ticket.sin_tildes',                  '1'),
    ('ticket.imprimir_automatico',         '0'),
    ('impresora.nombre',                   ''),
    ('stock.permitir_negativo',            '0'),
    ('precios.redondeo_multiplo_centavos', '100'),
    ('backup.retencion_diarios',           '14'),
    ('backup.retencion_mensuales',         '6');

-- Rubros iniciales de ejemplo
INSERT OR IGNORE INTO Rubros (nombre) VALUES
    ('Bebidas'), ('Golosinas'), ('Cigarrillos'), ('Fiambres y Lácteos'),
    ('Farmacia / Perfumería'), ('Limpieza'), ('Varios');

COMMIT;

PRAGMA user_version = 1;
```

## 2.6 Configuración de conexión (PRAGMAs)

Aplicar al abrir **cada** conexión (`SqliteConnectionFactory`):

```sql
PRAGMA foreign_keys = ON;        -- imprescindible: SQLite lo deja OFF por defecto
PRAGMA busy_timeout = 5000;      -- espera hasta 5 s ante bloqueo antes de fallar
PRAGMA synchronous  = NORMAL;    -- seguro en modo WAL y más rápido que FULL
```

Aplicar una sola vez (persistente en el archivo):

```sql
PRAGMA journal_mode = WAL;       -- lecturas no bloquean escrituras; resistente a cortes
```

> **Nota sobre WAL:** crea los archivos auxiliares `drugstore.db-wal` y `drugstore.db-shm`. Para respaldos **no** se debe copiar solo el `.db` en caliente; usar la API de backup (ver §7.5).

## 2.7 Versionado de esquema (migraciones)

- Se usa `PRAGMA user_version` como número de versión del esquema.
- Las migraciones son archivos `NNN_descripcion.sql` embebidos como recursos en `DrugstorePOS.Data`.
- `DatabaseInitializer`: lee `user_version`; por cada migración con número mayor, **realiza un respaldo previo**, ejecuta el script dentro de una transacción y actualiza `user_version`. Si falla, `ROLLBACK` y la aplicación informa el error sin abrir el sistema con un esquema inconsistente.
- Las migraciones son **solo hacia adelante** (no hay *down*); la reversión se logra restaurando el respaldo previo.

---

# 3. Definición de entidades y modelos de dominio

Todas las clases viven en `DrugstorePOS.Domain`. Se habilita `Nullable` (`<Nullable>enable</Nullable>`). El dinero se expresa en `decimal` (pesos con 2 decimales); la conversión a centavos `long` ocurre en `Data`.

## 3.1 Enumeraciones

```csharp
namespace DrugstorePOS.Domain.Enums;

public enum MetodoPago
{
    Efectivo = 1,
    DebitoTransferencia = 2
}

public enum EstadoVenta
{
    Completada = 1,
    Anulada = 2
}

public enum TipoMovimientoStock
{
    AltaInicial = 1,
    Ingreso = 2,
    Ajuste = 3,
    Venta = 4,
    AnulacionVenta = 5,
    Baja = 6
}

public enum OrigenCambioPrecio
{
    Manual = 1,
    Masivo = 2,
    Reversion = 3
}

public enum AplicarAjusteA
{
    SoloPrecioVenta = 1,
    CostoYPrecioVenta = 2
}

public enum ModoRedondeo
{
    SinRedondeo = 0,
    AlPesoMasCercano = 1,      // múltiplo de $1
    A5PesosMasCercano = 2,     // múltiplo de $5
    A10PesosMasCercano = 3     // múltiplo de $10
}

public enum PeriodoReporte
{
    Diario = 1,
    Mensual = 2
}
```

## 3.2 Entidades

```csharp
namespace DrugstorePOS.Domain.Entities;

public class Rubro
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}

public class Proveedor
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Notas { get; set; }
    public bool Activo { get; set; } = true;
}

public class Producto
{
    public long Id { get; set; }
    public string? CodigoBarras { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string NombreBusqueda { get; set; } = string.Empty;   // calculado por la capa de negocio
    public long RubroId { get; set; }
    public string? RubroNombre { get; set; }                     // solo lectura (JOIN)
    public long? ProveedorId { get; set; }
    public string? ProveedorNombre { get; set; }                 // solo lectura (JOIN)
    public decimal PrecioCosto { get; set; }
    public decimal PrecioVenta { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaAlta { get; set; }
    public DateTime FechaModificacion { get; set; }

    // Propiedades derivadas (no persistidas)
    public bool StockBajo => StockActual <= StockMinimo;
    public decimal MargenPorcentaje =>
        PrecioCosto > 0 ? Math.Round((PrecioVenta - PrecioCosto) / PrecioCosto * 100m, 2) : 0m;
}

public class Venta
{
    public long Id { get; set; }
    public int Numero { get; set; }
    public DateTime FechaHora { get; set; }
    public decimal Total { get; set; }
    public decimal CostoTotal { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public decimal? MontoRecibido { get; set; }
    public decimal Vuelto { get; set; }
    public EstadoVenta Estado { get; set; } = EstadoVenta.Completada;
    public DateTime? FechaAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }
    public List<VentaItem> Items { get; set; } = new();

    public decimal GananciaEstimada => Total - CostoTotal;
}

public class VentaItem
{
    public long Id { get; set; }
    public long VentaId { get; set; }
    public long ProductoId { get; set; }
    public string? CodigoBarras { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class MovimientoStock
{
    public long Id { get; set; }
    public long ProductoId { get; set; }
    public TipoMovimientoStock Tipo { get; set; }
    public int Cantidad { get; set; }              // delta con signo
    public int StockResultante { get; set; }
    public long? VentaId { get; set; }
    public string? Motivo { get; set; }
    public DateTime FechaHora { get; set; }
}

public class HistorialPrecio
{
    public long Id { get; set; }
    public long ProductoId { get; set; }
    public decimal CostoAnterior { get; set; }
    public decimal CostoNuevo { get; set; }
    public decimal VentaAnterior { get; set; }
    public decimal VentaNuevo { get; set; }
    public OrigenCambioPrecio Origen { get; set; }
    public Guid? LoteId { get; set; }
    public DateTime FechaHora { get; set; }
}
```

## 3.3 Modelos de aplicación (carrito, configuración, DTOs)

```csharp
namespace DrugstorePOS.Domain.Dtos;

// ---------- Carrito (solo en memoria; no se persiste hasta confirmar) ----------
public class CarritoItem
{
    public long ProductoId { get; init; }
    public string? CodigoBarras { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public decimal PrecioUnitario { get; init; }
    public decimal CostoUnitario { get; init; }
    public int StockDisponible { get; init; }
    public int Cantidad { get; set; }
    public decimal Subtotal => PrecioUnitario * Cantidad;
}

public class Carrito
{
    private readonly List<CarritoItem> _items = new();
    public IReadOnlyList<CarritoItem> Items => _items;
    public decimal Total => _items.Sum(i => i.Subtotal);
    public int CantidadUnidades => _items.Sum(i => i.Cantidad);
    public bool EstaVacio => _items.Count == 0;

    public void Agregar(CarritoItem item) { /* si existe ProductoId, suma Cantidad */ }
    public void CambiarCantidad(long productoId, int nuevaCantidad) { /* 0 => quitar */ }
    public void Quitar(long productoId) { }
    public void Vaciar() { }
}

// ---------- Solicitud de cobro ----------
public record ConfirmarVentaRequest(
    IReadOnlyList<CarritoItem> Items,
    MetodoPago MetodoPago,
    decimal? MontoRecibido);          // obligatorio si MetodoPago == Efectivo

public record ResultadoVenta(
    long VentaId,
    int NumeroVenta,
    decimal Total,
    decimal Vuelto,
    DateTime FechaHora);

// ---------- Filtros ----------
public record FiltroProductos(
    string? Texto = null,
    long? RubroId = null,
    long? ProveedorId = null,
    bool SoloStockBajo = false,
    bool IncluirInactivos = false,
    int Pagina = 1,
    int TamanoPagina = 200);

// ---------- Actualización masiva ----------
public record ParametrosActualizacionMasiva(
    long? RubroId,                         // al menos uno de RubroId/ProveedorId debe informarse
    long? ProveedorId,
    decimal Porcentaje,                    // ej. 10 = +10 %; -5 = -5 %
    AplicarAjusteA AplicarA,
    ModoRedondeo Redondeo);

public record PrevisualizacionPrecio(
    long ProductoId,
    string Nombre,
    decimal CostoActual,
    decimal CostoNuevo,
    decimal VentaActual,
    decimal VentaNueva);

public record ResultadoActualizacionMasiva(
    Guid LoteId,
    int ProductosActualizados,
    int ProductosOmitidos);

// ---------- Reportes ----------
public record ResumenVentas(
    DateTime Desde,
    DateTime HastaExclusivo,
    int CantidadVentas,
    int UnidadesVendidas,
    decimal TotalVendido,
    decimal CostoTotal,
    decimal GananciaEstimada,
    decimal TicketPromedio,
    decimal TotalEfectivo,
    decimal TotalDebitoTransferencia);

public record VentasPorPeriodo(
    DateTime Periodo,                      // día (diario) o primer día del mes (mensual)
    int CantidadVentas,
    decimal TotalVendido,
    decimal GananciaEstimada);

public record ProductoMasVendido(
    long ProductoId,
    string Nombre,
    int UnidadesVendidas,
    decimal TotalVendido,
    decimal GananciaEstimada);

// ---------- Resultado de operaciones (patrón Result) ----------
public record Resultado(bool Exito, string? Mensaje = null)
{
    public static Resultado Ok() => new(true);
    public static Resultado Error(string mensaje) => new(false, mensaje);
}

public record Resultado<T>(bool Exito, T? Valor, string? Mensaje = null)
{
    public static Resultado<T> Ok(T valor) => new(true, valor);
    public static Resultado<T> Error(string mensaje) => new(false, default, mensaje);
}
```

## 3.4 Excepciones de dominio

```csharp
namespace DrugstorePOS.Domain.Exceptions;

public class DomainException : Exception                       // base; mensaje apto para el usuario
{ public DomainException(string message) : base(message) { } }

public class ValidacionException : DomainException             // datos inválidos
{ public IReadOnlyList<string> Errores { get; }
  public ValidacionException(IEnumerable<string> errores)
      : base(string.Join(Environment.NewLine, errores)) => Errores = errores.ToList(); }

public class CodigoBarrasDuplicadoException : DomainException
{ public CodigoBarrasDuplicadoException(string codigo)
      : base($"Ya existe un producto con el código de barras {codigo}.") { } }

public class ProductoNoEncontradoException : DomainException
{ public ProductoNoEncontradoException(string criterio)
      : base($"No se encontró ningún producto para: {criterio}.") { } }

public class StockInsuficienteException : DomainException
{ public long ProductoId { get; }
  public StockInsuficienteException(long productoId, string nombre, int disponible, int solicitado)
      : base($"Stock insuficiente de \"{nombre}\": disponible {disponible}, solicitado {solicitado}.")
      => ProductoId = productoId; }

public class VentaYaAnuladaException : DomainException
{ public VentaYaAnuladaException(int numero) : base($"La venta N° {numero} ya fue anulada.") { } }

public class DataAccessException : Exception                   // errores técnicos de BD (no aptos para el usuario final)
{ public DataAccessException(string message, Exception inner) : base(message, inner) { } }
```

---

# 4. Contratos de servicios y casos de uso

> **Sincronía:** `Microsoft.Data.Sqlite` ejecuta I/O de forma síncrona (sus métodos `*Async` no aportan concurrencia real). Por simplicidad y previsibilidad, **todos los contratos son síncronos**. Las operaciones potencialmente largas (actualización masiva, reportes sobre histórico grande, backup) se invocan desde la UI con `Task.Run(...)` + indicador de progreso, evitando congelar la interfaz.

## 4.1 Contratos de acceso a datos (`Domain/Contracts/Repositories`)

```csharp
namespace DrugstorePOS.Domain.Contracts.Repositories;

public interface IProductoRepository
{
    Producto? ObtenerPorId(long id);
    Producto? ObtenerPorCodigoBarras(string codigoBarras);              // coincidencia exacta; solo activos por defecto
    bool ExisteCodigoBarras(string codigoBarras, long? excluirProductoId = null);
    IReadOnlyList<Producto> Buscar(string terminoNormalizado, int maxResultados = 30); // POS: autocompletar
    (IReadOnlyList<Producto> Items, int Total) Listar(FiltroProductos filtro);
    IReadOnlyList<Producto> ListarStockBajo();
    int ContarStockBajo();
    IReadOnlyList<Producto> ListarPorRubroOProveedor(long? rubroId, long? proveedorId); // base de actualización masiva

    long Insertar(Producto producto);
    void Actualizar(Producto producto);                                 // datos generales (no toca stock)
    void ActualizarPrecios(long productoId, decimal costo, decimal venta);
    void CambiarEstado(long productoId, bool activo);                   // baja/alta lógica

    // Descuento/ajuste atómico y condicionado. Devuelve false si no se cumplió la condición de stock.
    bool AjustarStock(long productoId, int delta, bool permitirNegativo);
    int ObtenerStock(long productoId);
}

public interface IRubroRepository
{
    IReadOnlyList<Rubro> Listar(bool incluirInactivos = false);
    Rubro? ObtenerPorId(long id);
    long Insertar(Rubro rubro);
    void Actualizar(Rubro rubro);
    bool TieneProductosActivos(long rubroId);
    void CambiarEstado(long rubroId, bool activo);
}

public interface IProveedorRepository
{
    IReadOnlyList<Proveedor> Listar(bool incluirInactivos = false);
    Proveedor? ObtenerPorId(long id);
    long Insertar(Proveedor proveedor);
    void Actualizar(Proveedor proveedor);
    bool TieneProductosActivos(long proveedorId);
    void CambiarEstado(long proveedorId, bool activo);
}

public interface IVentaRepository
{
    int ObtenerSiguienteNumero();                                       // MAX(numero)+1 dentro de la transacción
    long Insertar(Venta venta);                                         // inserta cabecera + ítems
    Venta? ObtenerPorId(long id, bool incluirItems = true);
    Venta? ObtenerPorNumero(int numero);
    IReadOnlyList<Venta> ListarPorRango(DateTime desde, DateTime hastaExclusivo, bool incluirAnuladas = true);
    void MarcarAnulada(long ventaId, DateTime fecha, string motivo);
}

public interface IMovimientoStockRepository
{
    void Registrar(MovimientoStock movimiento);
    IReadOnlyList<MovimientoStock> ListarPorProducto(long productoId, int maxResultados = 200);
}

public interface IHistorialPrecioRepository
{
    void Registrar(HistorialPrecio registro);
    IReadOnlyList<HistorialPrecio> ListarPorProducto(long productoId, int maxResultados = 100);
    IReadOnlyList<HistorialPrecio> ListarPorLote(Guid loteId);
    Guid? ObtenerUltimoLote();
}

public interface IReporteRepository
{
    ResumenVentas ObtenerResumen(DateTime desde, DateTime hastaExclusivo);
    IReadOnlyList<VentasPorPeriodo> VentasPorDia(DateTime desde, DateTime hastaExclusivo);
    IReadOnlyList<VentasPorPeriodo> VentasPorMes(DateTime desde, DateTime hastaExclusivo);
    IReadOnlyList<ProductoMasVendido> ProductosMasVendidos(DateTime desde, DateTime hastaExclusivo, int top);
}

public interface IConfiguracionRepository
{
    string? Obtener(string clave);
    void Guardar(string clave, string valor);
    IReadOnlyDictionary<string, string> ObtenerTodo();
}
```

## 4.2 Unit of Work y fábrica de conexiones

```csharp
namespace DrugstorePOS.Domain.Contracts.Repositories;

/// <summary>Agrupa repositorios que comparten UNA conexión y UNA transacción.</summary>
public interface IUnitOfWork : IDisposable
{
    IProductoRepository Productos { get; }
    IVentaRepository Ventas { get; }
    IMovimientoStockRepository Movimientos { get; }
    IHistorialPrecioRepository HistorialPrecios { get; }
    IRubroRepository Rubros { get; }
    IProveedorRepository Proveedores { get; }
    IConfiguracionRepository Configuracion { get; }

    void Commit();      // confirma la transacción
    void Rollback();    // revierte (también se invoca implícitamente en Dispose si no hubo Commit)
}

public interface IUnitOfWorkFactory
{
    /// <summary>Abre conexión e inicia <c>BEGIN IMMEDIATE</c> (transacción de escritura).</summary>
    IUnitOfWork CreateWritable();

    /// <summary>Abre conexión de solo lectura (sin transacción explícita).</summary>
    IUnitOfWork CreateReadOnly();
}
```

> Los repositorios de **consulta simple** (UI de inventario, reportes) pueden resolverse con `CreateReadOnly()`. Toda operación que escriba en **más de una tabla** usa `CreateWritable()` y un único `Commit()`.

## 4.3 Contratos de servicios (`Domain/Contracts/Services`)

```csharp
namespace DrugstorePOS.Domain.Contracts.Services;

public interface IProductoService
{
    // ----- Consulta rápida -----
    Producto? BuscarPorCodigoBarras(string codigoBarras);                   // POS e Inventario
    IReadOnlyList<Producto> BuscarPorNombre(string texto, int max = 30);    // normaliza tildes/mayúsculas
    (IReadOnlyList<Producto> Items, int Total) Listar(FiltroProductos filtro);

    // ----- ABM -----
    Resultado<long> Crear(Producto producto, int stockInicial);             // valida, registra ALTA_INICIAL
    Resultado Modificar(Producto producto);                                 // valida unicidad de código de barras
    Resultado DarDeBaja(long productoId);                                   // baja lógica; conserva historial
    Resultado Reactivar(long productoId);

    // ----- Stock -----
    Resultado RegistrarIngreso(long productoId, int cantidad, string? motivo);   // +cantidad (reposición)
    Resultado AjustarStock(long productoId, int nuevoStock, string motivo);      // fija stock (recuento/rotura)
    IReadOnlyList<Producto> ObtenerProductosConStockBajo();
    int ContarAlertasStockBajo();                                                // badge en el menú
    IReadOnlyList<MovimientoStock> ObtenerKardex(long productoId);
}

public interface ICatalogoService
{
    IReadOnlyList<Rubro> ListarRubros(bool incluirInactivos = false);
    Resultado<long> GuardarRubro(Rubro rubro);                              // alta o modificación
    Resultado DarDeBajaRubro(long rubroId);                                 // falla si tiene productos activos

    IReadOnlyList<Proveedor> ListarProveedores(bool incluirInactivos = false);
    Resultado<long> GuardarProveedor(Proveedor proveedor);
    Resultado DarDeBajaProveedor(long proveedorId);
}

public interface IPrecioService
{
    // ----- Manual rápido -----
    Resultado ActualizarPrecioManual(long productoId, decimal nuevoPrecioVenta, decimal? nuevoCosto = null);

    // ----- Masivo -----
    IReadOnlyList<PrevisualizacionPrecio> PrevisualizarActualizacionMasiva(ParametrosActualizacionMasiva p);
    Resultado<ResultadoActualizacionMasiva> AplicarActualizacionMasiva(ParametrosActualizacionMasiva p);
    Resultado RevertirUltimoLote();                                         // deshace el último lote masivo
    IReadOnlyList<HistorialPrecio> ObtenerHistorial(long productoId);

    // ----- Utilitario puro (testeable) -----
    decimal CalcularPrecioConPorcentaje(decimal precioActual, decimal porcentaje, ModoRedondeo redondeo);
}

public interface IVentaService
{
    // ----- Carrito (lógica pura, sin persistencia) -----
    Resultado<CarritoItem> AgregarAlCarrito(Carrito carrito, string codigoBarrasOProductoId, int cantidad = 1);
    Resultado<CarritoItem> AgregarAlCarrito(Carrito carrito, Producto producto, int cantidad = 1);
    Resultado ModificarCantidad(Carrito carrito, long productoId, int nuevaCantidad);
    void QuitarDelCarrito(Carrito carrito, long productoId);
    decimal CalcularVuelto(decimal total, decimal montoRecibido);           // lanza ValidacionException si recibido < total

    // ----- Confirmación (transaccional; ver §7.1) -----
    Resultado<ResultadoVenta> ConfirmarVenta(ConfirmarVentaRequest request);

    // ----- Post-venta -----
    Venta? ObtenerVenta(long ventaId);
    IReadOnlyList<Venta> ListarVentasDelDia(DateTime dia);
    Resultado AnularVenta(long ventaId, string motivo);                     // repone stock; transaccional
    string GenerarTicket(long ventaId);                                     // texto plano (ver §6)
}

public interface IReporteService
{
    ResumenVentas ObtenerResumenDiario(DateTime dia);
    ResumenVentas ObtenerResumenMensual(int anio, int mes);
    IReadOnlyList<VentasPorPeriodo> ObtenerVentasPorDiaDelMes(int anio, int mes);       // serie para gráfico
    IReadOnlyList<VentasPorPeriodo> ObtenerVentasPorMesDelAnio(int anio);               // serie para gráfico
    IReadOnlyList<ProductoMasVendido> ObtenerMasVendidos(DateTime desde, DateTime hastaExclusivo, int top = 10);
}

public interface ITicketFormatter
{
    /// <param name="anchoColumnas">32 (58 mm) o 48 (80 mm).</param>
    string Formatear(Venta venta, ConfiguracionTicket config, int anchoColumnas);
}

public record ConfiguracionTicket(
    string NombreComercio, string? Direccion, string? Telefono,
    string PieDePagina, bool SinTildes);

public interface ITicketPrinter                                              // implementado en UI/Printing
{
    IReadOnlyList<string> ListarImpresoras();
    Resultado Imprimir(string textoTicket, string? nombreImpresora = null);
}

public interface IConfiguracionService
{
    ConfiguracionSistema Obtener();
    Resultado Guardar(ConfiguracionSistema configuracion);
}

public interface IBackupService
{
    Resultado<string> CrearRespaldo(string? motivo = null);                 // devuelve ruta del archivo generado
    Resultado<string> CrearRespaldoSiCorresponde();                         // 1 vez cada 24 h
    IReadOnlyList<InfoRespaldo> ListarRespaldos();
    Resultado Restaurar(string rutaRespaldo);                               // requiere reinicio de la app
    void AplicarRetencion();
    Resultado<bool> VerificarIntegridad(string rutaBd);                     // PRAGMA integrity_check
}

public record InfoRespaldo(string Ruta, DateTime Fecha, long TamanoBytes, string? Motivo);
```

## 4.4 Casos de uso

> Formato: **Actor** siempre es el *Operador* (único usuario). Precondición común: la aplicación está abierta y la BD inicializada.

### CU-01 — Alta de producto
| Campo | Detalle |
|---|---|
| **Disparador** | Botón "Nuevo" (`Ctrl+N`) en Inventario. |
| **Flujo principal** | 1) Se abre `ProductoEditForm`. 2) El operador escanea o escribe el código de barras (opcional). 3) Completa nombre, rubro, proveedor, costo, venta, stock inicial y mínimo. 4) Guarda (`F10`/Enter en botón). 5) `IProductoService.Crear` valida, calcula `nombre_busqueda`, inserta producto y movimiento `ALTA_INICIAL` en **una transacción**. |
| **Validaciones** | Nombre obligatorio (≤ 120); rubro obligatorio; precios ≥ 0; venta ≥ costo (advertencia, no bloqueo); stock ≥ 0; código de barras único. |
| **Flujos alternativos** | Código duplicado → mensaje con acceso rápido a editar el producto existente. |
| **Postcondición** | Producto visible en búsqueda del POS. |

### CU-02 — Modificación de producto
Igual a CU-01 sin tocar el stock (el stock solo se cambia mediante CU-04/CU-05/ventas). El campo "Stock actual" se muestra solo-lectura con botón "Ajustar stock…".

### CU-03 — Baja de producto
Baja **lógica** (`activo = 0`) con confirmación. El producto deja de aparecer en el POS y la búsqueda normal; permanece en reportes históricos. Puede reactivarse activando el filtro "Incluir inactivos".

### CU-04 — Ingreso de mercadería
Desde Inventario: botón "Ingreso de stock" (`F5`) → buscar producto, informar cantidad > 0 y motivo opcional → `RegistrarIngreso` (suma stock + movimiento `INGRESO`). Opcionalmente permite actualizar el costo en el mismo paso.

### CU-05 — Ajuste de stock
Fija el stock a un valor contado (recuento físico, rotura, vencimiento). Motivo **obligatorio**. Registra movimiento `AJUSTE` con delta = nuevo − actual.

### CU-06 — Consulta rápida y alerta de stock mínimo
- Caja de búsqueda con foco por defecto: si el texto es numérico de 8–14 dígitos → búsqueda exacta por código de barras; si no → búsqueda por nombre.
- Al abrir el sistema y tras cada venta/ajuste se recalcula `ContarAlertasStockBajo()`; el menú muestra un **badge rojo** con la cantidad. La condición es `stock_actual <= stock_minimo` con producto activo.

### CU-07 — Actualización manual de precio
`PreciosView`: seleccionar producto, editar precio de venta (y opcionalmente costo) directamente en la grilla (celda editable) o con `F2`. Al confirmar con Enter: valida > 0, actualiza y registra `HistorialPrecios` (origen `MANUAL`).

### CU-08 — Actualización masiva por porcentaje
1. El operador elige filtro: rubro, proveedor, o ambos (**al menos uno obligatorio**, para evitar tocar todo el catálogo por error).
2. Informa el porcentaje (positivo o negativo), si aplica a *precio de venta* o a *costo y venta*, y el modo de redondeo.
3. Pulsa **Previsualizar**: la grilla muestra precio actual → nuevo por producto.
4. Pulsa **Aplicar** y confirma el diálogo ("Se modificarán N productos").
5. El sistema crea un **respaldo automático previo** (motivo `pre-actualizacion-masiva`), y en **una transacción** actualiza todos los precios y escribe `HistorialPrecios` con un `lote_id` común.
6. Opción "Revertir último lote" restaura los valores anteriores y registra origen `REVERSION`.

**Regla de cálculo:** `nuevo = precio × (1 + porcentaje/100)`; luego redondeo; `MidpointRounding.AwayFromZero`; resultado nunca menor a `0,01`. Los productos cuyo nuevo precio sea ≤ 0 se omiten y se informan.

### CU-09 — Venta (POS)
| Paso | Descripción |
|---|---|
| 1 | La vista de ventas abre con foco en el campo de escaneo. |
| 2 | Escanear/ingresar código → `AgregarAlCarrito`: si el producto ya está, incrementa cantidad. Si no existe, se muestra aviso sonoro/visual no bloqueante ("Producto no encontrado"). |
| 3 | Atajo de cantidad: escribir `5*` + código (o `5*` y luego escanear) agrega 5 unidades. |
| 4 | Edición del carrito: `+`/`-` cantidad, `Supr` quita línea, `Esc` limpia (con confirmación). |
| 5 | `F12` o botón **Cobrar** abre `CobroForm`: selector de método de pago (`F1` Efectivo / `F2` Débito-Transferencia). |
| 6 | Efectivo: se ingresa monto recibido; el vuelto se calcula en vivo; botones de billetes rápidos (+$1000, +$2000, +$5000, "Exacto"). |
| 7 | Confirmar (`Enter`): `ConfirmarVenta` ejecuta la transacción (§7.1). |
| 8 | Se muestra "Venta N° X — Vuelto $Y". Se ofrece/imprime ticket según configuración. El carrito se vacía y el foco regresa al escaneo. |

**Validaciones:** carrito no vacío; si `stock.permitir_negativo = 0`, la cantidad en carrito ≤ stock disponible (se valida al agregar **y** de nuevo al confirmar); monto recibido ≥ total en efectivo.

### CU-10 — Anulación de venta
Desde el listado "Ventas del día" (o buscando por N°): seleccionar venta → "Anular" → motivo obligatorio → confirmar. En una transacción: marca la venta `ANULADA`, repone stock de cada ítem y registra movimientos `ANULACION_VENTA`. Las ventas anuladas **se excluyen** de los reportes.

### CU-11 — Reimpresión de ticket
Desde "Ventas del día": regenerar el ticket de cualquier venta y reimprimir o copiar al portapapeles. Se marca "*** REIMPRESIÓN ***" en el encabezado.

### CU-12 — Reportes
Seleccionar período (Diario/Mensual) y fecha → `IReporteService` devuelve resumen, serie y ranking → la vista actualiza tarjetas, gráfico y grilla. Ver §5.5.

### CU-13 — Respaldo y restauración
Ver §7.5.

---

# 5. Especificación de interfaces de usuario

## 5.1 Principios de diseño

1. **Teclado primero:** el operador no debe necesitar el mouse en el flujo de venta.
2. **Foco inteligente:** el campo de escaneo recupera el foco automáticamente tras cada acción.
3. **Fuentes grandes** en total y vuelto (≥ 28 pt) legibles a distancia.
4. **Feedback inmediato:** sonido corto (`SystemSounds.Beep`/`Asterisk`) en producto no encontrado/stock insuficiente; mensajes no modales (barra de estado) para errores leves.
5. **Confirmaciones solo en acciones destructivas** (anular, baja, masiva, restaurar).
6. Estilos consistentes: `Font = Segoe UI 10pt` base; colores de alerta (rojo para stock bajo, verde para confirmaciones).
7. Todos los `DataGridView`: `ReadOnly = true` (salvo edición rápida de precios), `SelectionMode = FullRowSelect`, `AllowUserToAddRows = false`, `AutoGenerateColumns = false`, `RowHeadersVisible = false`, doble buffer activado (extensión por reflexión) para fluidez.

## 5.2 Navegación y shell (`MainForm`)

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ DrugstorePOS                                                       [_][□][X] │
├──────────────────────────────────────────────────────────────────────────────┤
│ [F1 Ventas]  [F2 Inventario]  [F3 Precios]  [F4 Reportes]  [⚙ Sistema ▾]     │
│                                         Stock bajo: (7)   Respaldo: hace 2 h │
├──────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│                  Panel de contenido (UserControl activo)                     │
│                                                                              │
├──────────────────────────────────────────────────────────────────────────────┤
│ Barra de estado: mensajes · fecha y hora · versión de BD                     │
└──────────────────────────────────────────────────────────────────────────────┘
```

- `MainForm` aloja **un `UserControl` activo** (`Dock = Fill`) en un `Panel`; se instancian bajo demanda y se **conservan** para no perder el carrito al cambiar de pestaña.
- **Pantalla inicial:** `VentasView` (foco en escaneo).
- **Menú "Sistema":** Rubros, Proveedores, Configuración (comercio/ticket/impresora), Respaldos (crear/restaurar), Acerca de.
- **Atajos globales:** `F1` Ventas · `F2` Inventario · `F3` Precios · `F4` Reportes · `Ctrl+B` Respaldo manual.
- **Cierre:** al cerrar, si hay carrito con ítems → confirmación; luego respaldo opcional "al salir" (configurable) y liberación de conexión/`Mutex`.

**Flujo de navegación:**

```
Inicio ──► VentasView ──F12──► CobroForm ──Enter──► (ticket) ──► VentasView
   │            └─ "Ventas del día" ──► VentasDelDiaForm ──► Anular / Reimprimir
   ├──► InventarioView ──Nuevo/Editar──► ProductoEditForm
   │            ├─ Ingreso de stock ──► IngresoStockForm
   │            ├─ Ajustar stock ──► AjusteStockForm
   │            └─ Ver kardex ──► KardexForm
   ├──► PreciosView ──"Actualización masiva"──► ActualizacionMasivaForm
   ├──► ReportesView
   └──► Sistema ──► RubrosForm | ProveedoresForm | ConfiguracionForm | RespaldoForm
```

## 5.3 `VentasView` — Punto de venta

```
┌───────────────────────────────────────────────────────────────────────────────────┐
│ Escanear / buscar:  [___________________________________] ( 🔍 )   Cant: [ 1 ]    │
│ (resultados de búsqueda por nombre en lista desplegable bajo el campo)            │
├──────────────────────────────────────────────────────────────┬────────────────────┤
│ #  Producto                 Cant   P.Unit      Subtotal  [x] │                    │
│ 1  Coca Cola 500ml            2    $1.800,00   $3.600,00     │      TOTAL         │
│ 2  Alfajor Triple             1      $900,00     $900,00     │                    │
│ 3  Chicles Beldent            3      $250,00     $750,00     │   $ 5.250,00       │
│                                                              │                    │
│                       (DataGridView carrito)                 │  Ítems: 3          │
│                                                              │  Unidades: 6       │
│                                                              │                    │
│                                                              │ [F12  COBRAR]      │
│                                                              │ [Esc  Cancelar]    │
│                                                              │ [F9   Ventas hoy]  │
├──────────────────────────────────────────────────────────────┴────────────────────┤
│ Último producto: Chicles Beldent — Stock: 41   |   Última venta: N° 000122 $...   │
└───────────────────────────────────────────────────────────────────────────────────┘
```

**Controles:**

| Control | Tipo | Comportamiento |
|---|---|---|
| `txtScan` | `TextBox` | Foco permanente. `Enter` ejecuta: numérico → código de barras; texto → búsqueda por nombre. Soporta prefijo `N*` (cantidad). Selecciona todo el texto tras procesar. |
| `lstResultados` | `ListBox`/`DataGridView` flotante | Aparece con ≥ 2 caracteres de texto (debounce 150 ms), flechas ↑↓ navegan, `Enter` agrega, `Esc` cierra. Máx. 30 filas. |
| `numCantidad` | `NumericUpDown` (1–999) | Cantidad a agregar en el próximo escaneo; vuelve a 1 tras agregar. |
| `dgvCarrito` | `DataGridView` | Columnas: `#`, `Producto`, `Cant`, `P.Unit`, `Subtotal`. Fila resaltada = línea activa. `+`/`-` (en teclado numérico) cambian cantidad; `Supr` elimina línea; doble clic/`F2` edita cantidad. |
| `lblTotal` | `Label` | Fuente 36 pt negrita, alineado a la derecha. |
| `lblResumen` | `Label` | Ítems y unidades. |
| `btnCobrar` | `Button` | `F12`. Deshabilitado si el carrito está vacío. |
| `btnCancelar` | `Button` | `Esc` (doble: primero limpia el texto de `txtScan`, luego vacía carrito con confirmación). |
| `btnVentasHoy` | `Button` | `F9`. Abre `VentasDelDiaForm`. |
| `lblEstado` | `Label` | Último producto agregado, stock restante, mensajes (producto no encontrado, stock insuficiente). |

**Reglas de interacción:**
- Escanear el mismo producto suma 1 a la cantidad existente (no duplica filas).
- Si `Cantidad > StockDisponible` y no se permite stock negativo → rechazo con aviso y sonido.
- Producto con `StockBajo` agregado al carrito → la fila se pinta en ámbar como aviso.
- Cualquier tecla de caracter imprimible lleva el foco a `txtScan` (compatibilidad con lectores que emulan teclado).

### `CobroForm` (diálogo modal)

```
┌──────────────────────── Cobrar venta ─────────────────────────┐
│  TOTAL A COBRAR:                          $ 5.250,00          │
│                                                               │
│  Método de pago:   (•) F1 Efectivo     ( ) F2 Débito/Transf.  │
│                                                               │
│  Monto recibido:   [  6.000,00  ]   [Exacto][+1000][+5000]    │
│                                                               │
│  VUELTO:                                  $   750,00          │
│                                                               │
│  [x] Imprimir ticket (F3)                                     │
│                                                               │
│            [ Enter  Confirmar ]        [ Esc  Volver ]        │
└───────────────────────────────────────────────────────────────┘
```

| Control | Detalle |
|---|---|
| `rbEfectivo` / `rbDebito` | `F1` / `F2`. Si es débito/transferencia, se oculta el monto recibido y el vuelto; el monto recibido = total. |
| `txtMontoRecibido` | `MoneyTextBox` (solo dígitos, `,`); precargado con el total; recalcula vuelto en cada `TextChanged`. Si `< total` → vuelto en rojo y botón Confirmar deshabilitado. |
| Botones rápidos | "Exacto", "+$1.000", "+$2.000", "+$5.000" (suman al monto escrito o definen billete). |
| `lblVuelto` | 28 pt, verde si ≥ 0. |
| `chkImprimir` | Valor inicial según `ticket.imprimir_automatico`. |
| `btnConfirmar` | `Enter`. Deshabilitado durante el procesamiento para evitar doble confirmación (*double-submit*). |

### `VentasDelDiaForm`
Grilla con N°, hora, total, método, estado; filtros por fecha; acciones **Ver detalle**, **Reimprimir ticket** (`Ctrl+P`), **Anular** (`Supr`, con motivo). Pie con total del día de ventas completadas.

## 5.4 `InventarioView` y `ProductoEditForm`

```
┌───────────────────────────────────────────────────────────────────────────────────┐
│ Buscar: [______________] Rubro: [Todos ▾] Proveedor: [Todos ▾]                    │
│ [x] Solo stock bajo (7)   [ ] Incluir inactivos        [Nuevo Ctrl+N] [Editar F2] │
│                                                    [Ingreso F5] [Ajustar F6] [Baja]│
├───────────────────────────────────────────────────────────────────────────────────┤
│ Código        Producto            Rubro      Prov.    Costo   Venta   Stock  Mín. │
│ 7790895000... Coca Cola 500ml     Bebidas    Dist.X  $1.100  $1.800    24     12  │
│ 7791234567... Alfajor Triple      Golosinas  Dist.Y    $550    $900     3      5  │ ← rojo
│ ...                                                                               │
├───────────────────────────────────────────────────────────────────────────────────┤
│ 1.248 productos · Mostrando 1–200      [◀ Anterior] [Siguiente ▶]                │
└───────────────────────────────────────────────────────────────────────────────────┘
```

| Control | Detalle |
|---|---|
| `txtBuscar` | Búsqueda con *debounce* 250 ms; reconoce código de barras (Enter exacto) o texto. |
| `cboRubro`, `cboProveedor` | `ComboBox` DropDownList con opción "Todos". |
| `chkStockBajo`, `chkInactivos` | Filtros booleanos. |
| `dgvProductos` | Columnas: Código, Producto, Rubro, Proveedor, Costo, Venta, Stock, Mínimo, Estado. Filas con `StockBajo` en fondo rojo claro; inactivas en gris tachado. Ordenable por columna. Paginación de 200. |
| Botones | `Nuevo`, `Editar`, `Ingreso de stock`, `Ajustar stock`, `Baja/Reactivar`, `Kardex`. Doble clic = Editar. |

**`ProductoEditForm` (modal):**

| Campo | Control | Reglas |
|---|---|---|
| Código de barras | `TextBox` (acepta escaneo) | Opcional; único; botón "Generar código interno" (prefijo `2000…`). |
| Nombre | `TextBox` (máx. 120) | Obligatorio. |
| Rubro | `ComboBox` + botón "+" | Obligatorio; permite alta rápida. |
| Proveedor | `ComboBox` + botón "+" | Opcional. |
| Precio costo | `MoneyTextBox` | ≥ 0. |
| Precio venta | `MoneyTextBox` | ≥ 0; muestra margen calculado en vivo. |
| Margen % | `Label` | `(venta − costo)/costo`; advertencia si es negativo. |
| Stock inicial | `NumericUpDown` | Solo alta. En edición: solo lectura + "Ajustar stock…". |
| Stock mínimo | `NumericUpDown` | ≥ 0. |
| Activo | `CheckBox` | Solo edición. |
| Botones | `Guardar` (`Ctrl+S`/`F10`), `Cancelar` (`Esc`) | |

## 5.5 `PreciosView` y `ActualizacionMasivaForm`

**`PreciosView` — edición manual rápida**

```
┌───────────────────────────────────────────────────────────────────────────────────┐
│ Buscar: [____________]  Rubro: [Todos ▾]  Proveedor: [Todos ▾]                    │
│                                   [Actualización masiva…]  [Revertir último lote] │
├───────────────────────────────────────────────────────────────────────────────────┤
│ Código      Producto           Costo       Venta (editable)    Margen   Últ. cambio│
│ 779089...   Coca Cola 500ml    $1.100,00   [ $1.800,00 ]       63,6%    05/10/2026 │
└───────────────────────────────────────────────────────────────────────────────────┘
```

- La columna **Venta** (y opcionalmente **Costo**) es **editable en la propia celda**. `Enter` confirma y persiste (con registro en historial) y el foco baja a la siguiente fila para carga continua. `Esc` cancela la edición.
- Validación de celda: numérico, ≥ 0, formato local.
- `F3` sobre una fila abre el historial de precios del producto.

**`ActualizacionMasivaForm`:**

```
┌──────────────────── Actualización masiva de precios ──────────────────────┐
│ Filtro (al menos uno):  Rubro: [Bebidas ▾]   Proveedor: [Todos ▾]         │
│ Porcentaje: [ 10,00 ] %   (negativo = baja)                               │
│ Aplicar a:  (•) Solo precio de venta   ( ) Costo y precio de venta        │
│ Redondeo:   [Al peso más cercano ▾]                                       │
│                                                [ Previsualizar ]          │
├───────────────────────────────────────────────────────────────────────────┤
│ Producto            Costo act → nuevo     Venta actual → nueva            │
│ Coca Cola 500ml     $1.100 → $1.100       $1.800 → $1.980                 │
│ ...                                        N productos afectados          │
├───────────────────────────────────────────────────────────────────────────┤
│ Se creará un respaldo automático antes de aplicar.                        │
│                                   [ Aplicar ]       [ Cancelar ]          │
└───────────────────────────────────────────────────────────────────────────┘
```

- `Aplicar` solo se habilita **después** de una previsualización vigente (si se cambia cualquier parámetro, se invalida).
- Se ejecuta en `Task.Run` con `ProgressBar` (marquee) y bloqueo del formulario.

## 5.6 `ReportesView`

```
┌───────────────────────────────────────────────────────────────────────────────────┐
│ Período: (•) Diario  ( ) Mensual     Fecha: [08/10/2026 ▾]       [ Actualizar F5 ]│
├──────────────┬──────────────┬──────────────┬──────────────┬───────────────────────┤
│ Total vendido│ Ganancia est.│ Nº de ventas │ Ticket prom. │ Efectivo | Déb./Transf│
│ $ 184.500    │ $ 61.200     │ 47           │ $ 3.925      │ $120.000 | $64.500    │
├──────────────┴──────────────┴──────────────┴──────────────┴───────────────────────┤
│ [Gráfico de barras: ventas por día del mes / por hora del día]                    │
├───────────────────────────────────────────┬───────────────────────────────────────┤
│ Top 10 productos más vendidos             │ Ventas por día (grilla)               │
│ #  Producto     Unid.   Total    Ganancia │ Fecha        Ventas    Total          │
└───────────────────────────────────────────┴───────────────────────────────────────┘
```

| Control | Detalle |
|---|---|
| `rbDiario` / `rbMensual` | Cambia el selector de fecha (`DateTimePicker` formato `dd/MM/yyyy` o `MMMM yyyy`). |
| Tarjetas (`Panel` + `Label`) | Total vendido, ganancia estimada, cantidad de ventas, ticket promedio, desglose por método de pago. |
| Gráfico | Barras: en modo mensual, ventas por día; en modo diario, ventas por franja horaria. Implementado con `ScottPlot` o `Chart`. |
| `dgvMasVendidos` | Top N (10 por defecto; selector 10/20/50), ordenado por unidades. |
| `dgvPeriodos` | Detalle por día (mensual) o por hora (diario). |

**Definiciones:**
- **Total vendido** = Σ `Ventas.total` con `estado = 'COMPLETADA'` en el rango.
- **Ganancia estimada** = Σ `(subtotal − cantidad × costo_unitario)` usando los **snapshots de costo** de cada línea. Es *estimada* porque el costo es el vigente al momento de la venta, no un costeo por lotes.
- **Más vendidos**: ranking por unidades vendidas (desempate por total vendido).
- Los reportes excluyen ventas `ANULADA`.

## 5.7 Formularios de sistema

| Formulario | Contenido |
|---|---|
| `RubrosForm` / `ProveedoresForm` | Grilla + alta/edición en línea o panel lateral; baja lógica con validación de productos activos asociados. |
| `ConfiguracionForm` | Datos del comercio, ancho de papel (58/80), impresora (lista de impresoras instaladas), pie del ticket, "sin tildes", impresión automática, permitir stock negativo, redondeo por defecto, carpetas de respaldo y retención. Botón "Imprimir ticket de prueba". |
| `RespaldoForm` | Lista de respaldos (fecha, tamaño, motivo), botones *Crear ahora*, *Restaurar seleccionado*, *Abrir carpeta*, *Verificar integridad*. |
| `IngresoStockForm` / `AjusteStockForm` | Producto (buscador), cantidad, motivo. |
| `KardexForm` | Movimientos del producto (fecha, tipo, cantidad, stock resultante, motivo). |

---

# 6. Formato del ticket de venta

## 6.1 Reglas generales

- **Texto plano**, fuente monoespaciada, salto de línea `\r\n`.
- **Ancho de línea:**
  - Papel 58 mm → **32 columnas** (Font A típica).
  - Papel 80 mm → **48 columnas** (Font A típica).
- Si la comandera usa Font B (más angosta) el ancho puede ser 42 (58 mm) o 64 (80 mm); el ancho es parametrizable en `ITicketFormatter` (parámetro `anchoColumnas`).
- Importes con formato `$ 1.234,56` (cultura `es-AR`); el ticket nunca excede el ancho (se **trunca/ajusta** el texto largo).
- **Nombres de producto largos:** se parten en múltiples líneas respetando palabras (*word-wrap*).
- **Tildes y ñ:** si `ticket.sin_tildes = 1`, se transliteran (á→a, ñ→n, ¡→"", ¿→"") para evitar caracteres corruptos en comanderas con página de códigos limitada. Si se envía como texto crudo, usar codificación **CP850** o **Windows-1252** según el modelo de la impresora.
- Al final se agregan **3 a 5 líneas en blanco** para que el papel avance más allá de la cuchilla.
- El ticket es un **comprobante interno no fiscal**.

## 6.2 Algoritmo de formateo (resumen)

1. **Encabezado:** nombre del comercio, dirección y teléfono, centrados (omitir líneas vacías).
2. **Separador** `=` × ancho.
3. **Datos de venta:** `Ticket N°` (6 dígitos con ceros), fecha `dd/MM/yyyy HH:mm`, método de pago.
4. **Separador** `-` × ancho.
5. **Ítems (58 mm / 32 col):** por cada línea → (a) nombre (con wrap), (b) `"{cant} x {precioUnit}"` alineado a la izquierda y `{subtotal}` alineado a la derecha.
6. **Separador** `-`.
7. **Totales:** `TOTAL` (y total de unidades). En efectivo: `RECIBIDO` y `VUELTO`.
8. **Pie:** mensaje de agradecimiento y leyenda "No válido como factura", centrados.
9. Líneas en blanco finales.

## 6.3 Plantilla — Papel 58 mm (32 columnas)

```
       DRUGSTORE EL PUNTO
      Av. Siempre Viva 123
       Tel: 381-555-0000
================================
TICKET N° 000123
Fecha: 08/10/2026 14:35
Pago : EFECTIVO
--------------------------------
Coca Cola 500ml
2 x $1.800,00          $3.600,00
Alfajor Triple
1 x $900,00              $900,00
Chicles Beldent
3 x $250,00              $750,00
--------------------------------
Unidades: 6
TOTAL                  $5.250,00
RECIBIDO               $6.000,00
VUELTO                   $750,00
================================
    ¡GRACIAS POR SU COMPRA!
     No valido como factura
```

*(Después de la última línea se agregan 4 saltos de línea.)*

## 6.4 Plantilla — Papel 80 mm (48 columnas)

```
                DRUGSTORE EL PUNTO
               Av. Siempre Viva 123
                Tel: 381-555-0000
================================================
TICKET N° 000123                 08/10/2026 14:35
Pago: EFECTIVO
------------------------------------------------
CAN DESCRIPCION                P.UNIT    IMPORTE
------------------------------------------------
  2 Coca Cola 500ml         $1.800,00  $3.600,00
  1 Alfajor Triple            $900,00    $900,00
  3 Chicles Beldent           $250,00    $750,00
------------------------------------------------
Unidades: 6
TOTAL                                    $5.250,00
RECIBIDO                                 $6.000,00
VUELTO                                     $750,00
================================================
            ¡GRACIAS POR SU COMPRA!
             No valido como factura
```

**Columnas del cuerpo (48):** `CAN` = 3 (der.) · espacio · `DESCRIPCION` = 23 (izq.) · espacio · `P.UNIT` = 9 (der.) · espacio · `IMPORTE` = 10 (der.).

## 6.5 Casos especiales

| Caso | Tratamiento |
|---|---|
| Reimpresión | Línea `*** REIMPRESION ***` tras el encabezado. |
| Venta anulada (al reimprimir) | Línea `*** VENTA ANULADA ***` en mayúsculas y separador doble. |
| Débito/Transferencia | Se omiten `RECIBIDO` y `VUELTO`; se muestra `Pago: DEBITO/TRANSF.`. |
| Importes grandes | Si el importe excede el espacio disponible, se reduce el texto del nombre, nunca el importe. |
| Más de 99 unidades en una línea | La columna cantidad se amplía a 4 dígitos en 80 mm; en 58 mm se muestra `{cant} x`. |

## 6.6 Impresión

- **Opción A (recomendada, simple):** `System.Drawing.Printing.PrintDocument` con fuente monoespaciada (`Consolas`/`Courier New`, 8–9 pt) y `PaperSize` personalizado (ancho 58/80 mm, alto continuo). Funciona con cualquier comandera que tenga driver de Windows.
- **Opción B (opcional, v1.1):** ESC/POS crudo (`Winspool` *RAW*): permite negrita, doble altura y corte automático (`GS V`). Aislado detrás de `ITicketPrinter` para no afectar el negocio.
- Vista previa en pantalla (`TextBox` multilínea monoespaciado) y botón **Copiar** como respaldo si no hay impresora.
- Si la impresión falla, la **venta no se revierte**: se informa el error y se ofrece reintentar o reimprimir desde "Ventas del día".

---

# 7. Consideraciones técnicas y manejo de errores

## 7.1 Transacción de confirmación de venta

**Objetivo:** que descontar stock, registrar la venta y los movimientos sea **atómico** (todo o nada), incluso ante corte de energía o excepción.

**Algoritmo `IVentaService.ConfirmarVenta`:**

```
1.  Validar request (carrito no vacío, cantidades > 0, método de pago, monto recibido).
2.  using var uow = _uowFactory.CreateWritable();      // abre conexión + "BEGIN IMMEDIATE"
3.  try {
4.     numero = uow.Ventas.ObtenerSiguienteNumero();    // MAX(numero)+1 (seguro por BEGIN IMMEDIATE)
5.     total = 0; costoTotal = 0; items = []
6.     por cada línea del carrito:
         a. producto = uow.Productos.ObtenerPorId(línea.ProductoId)
            → si no existe o está inactivo ⇒ ProductoNoEncontradoException
         b. PRECIO Y COSTO se leen de la BD (NO se confían los del carrito de la UI):
            precioUnit = producto.PrecioVenta; costoUnit = producto.PrecioCosto
         c. ok = uow.Productos.AjustarStock(producto.Id, -cantidad, permitirNegativo)
            → UPDATE Productos SET stock_actual = stock_actual - @c, fecha_modificacion = @f
               WHERE id = @id AND (stock_actual - @c >= 0 OR @permitirNegativo = 1);
            → si filas afectadas = 0 ⇒ StockInsuficienteException (ROLLBACK)
         d. subtotal = precioUnit × cantidad; acumular total y costoTotal
         e. agregar VentaItem (con snapshots de código, nombre, precio y costo)
7.     validar total > 0 y, si efectivo, montoRecibido >= total; vuelto = montoRecibido - total
8.     ventaId = uow.Ventas.Insertar(venta con items)
9.     por cada línea: uow.Movimientos.Registrar(tipo=VENTA, cantidad=-n, stock_resultante, venta_id)
10.    uow.Commit();                                    // "COMMIT" — punto de no retorno
11. } catch { uow.Rollback(); throw; }                  // Dispose también revierte si no hubo Commit
12. Retornar ResultadoVenta (el ticket y la impresión ocurren FUERA de la transacción).
```

**Reglas clave:**
- **`BEGIN IMMEDIATE`** adquiere el bloqueo de escritura desde el inicio: evita *deadlocks* de promoción de lectura→escritura y garantiza correlatividad de `numero`.
- El **descuento de stock es condicional dentro del `UPDATE`** (no se hace "leer, restar en C#, escribir"): elimina condiciones de carrera y valida el stock en el mismo instante en que se descuenta.
- El **precio se recalcula desde la BD al confirmar**: si el precio cambió mientras el carrito estaba abierto, prevalece el precio vigente (se informa al operador si difiere del mostrado en pantalla, antes de cobrar).
- La **impresión del ticket está fuera** de la transacción: un fallo de impresora jamás deja una venta a medias.
- Duración de la transacción: mínima (sin I/O de UI, sin impresión, sin diálogos dentro de ella).
- Anulación de venta (`AnularVenta`): misma estructura transaccional (marcar anulada + reponer stock + movimientos `ANULACION_VENTA`); si la venta ya está anulada → `VentaYaAnuladaException`.
- Actualización masiva: una única transacción para todos los productos + `HistorialPrecios`.
- Alta de producto: transacción para producto + movimiento `ALTA_INICIAL`.

**Pseudocódigo del patrón de transacción en `SqliteUnitOfWork` (referencia de diseño, no implementación final):**

```
CreateWritable():  conn.Open(); aplicar PRAGMAs; cmd("BEGIN IMMEDIATE").ExecuteNonQuery();
Commit():          cmd("COMMIT"); _completed = true;
Rollback():        si !_completed => cmd("ROLLBACK");
Dispose():         si !_completed => Rollback(); conn.Dispose();
```

## 7.2 Concurrencia y acceso al archivo

- **Una sola instancia** de la app (Mutex global `Global\DrugstorePOS`). Si ya está abierta, se trae al frente la existente.
- Conexiones **cortas**: se abre una conexión por operación/unidad de trabajo (el *pooling* de `Microsoft.Data.Sqlite` mitiga el costo). No mantener conexiones abiertas indefinidamente.
- `busy_timeout = 5000 ms` para tolerar bloqueos breves (p. ej. durante un backup o un antivirus).
- La ruta de la BD **no** debe estar en carpetas sincronizadas por servicios en la nube (OneDrive/Dropbox) ni en unidades de red: riesgo de corrupción.

## 7.3 Estrategia de manejo de errores

### 7.3.1 Clasificación

| Tipo | Ejemplos | Tratamiento al usuario | Log |
|---|---|---|---|
| **Validación / Dominio** (`DomainException` y derivadas) | Código duplicado, stock insuficiente, campo vacío. | Mensaje claro en español en la UI (MessageBox o barra de estado); la operación se cancela y el estado queda consistente. | `Warning` (sin stack trace). |
| **Acceso a datos** (`DataAccessException` envolviendo `SqliteException`) | `SQLITE_BUSY` (5), `SQLITE_CONSTRAINT` (19), `SQLITE_FULL` (13), `SQLITE_CORRUPT` (11), `SQLITE_READONLY` (8). | Mensaje genérico y accionable: "No se pudo guardar. Reintente; si persiste, contacte soporte." | `Error` + excepción completa. |
| **Impresión** | Impresora apagada/no configurada. | Aviso no bloqueante + opción de reintentar/copiar. La venta ya está guardada. | `Warning`. |
| **Inesperado** | `NullReference`, etc. | Diálogo "Ocurrió un error inesperado" con opción de copiar detalles; la app **no se cierra** salvo error fatal. | `Fatal/Error` con stack trace. |

### 7.3.2 Mapeo de errores SQLite (en capa Data)

| Código | Situación | Acción |
|---|---|---|
| `SQLITE_CONSTRAINT_UNIQUE` sobre `ux_productos_codigo_barras` | Código de barras duplicado (condición de carrera o validación omitida) | Lanzar `CodigoBarrasDuplicadoException`. |
| `SQLITE_CONSTRAINT_FOREIGNKEY` | Referencia inválida | `DataAccessException` + log. |
| `SQLITE_BUSY` / `SQLITE_LOCKED` | BD bloqueada más allá del timeout | Reintento automático (hasta 3 veces, 200 ms de espera); si falla, mensaje al usuario. |
| `SQLITE_FULL` | Disco lleno | Mensaje específico: "No hay espacio en disco". |
| `SQLITE_CORRUPT` / `SQLITE_NOTADB` | Archivo dañado | Bloquear escritura, ofrecer **restaurar el último respaldo válido** (ver §7.5). |

### 7.3.3 Manejadores globales (en `Program.cs`)

```
Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
Application.ThreadException           += (s, e) => Reportar(e.Exception);   // hilo de UI
AppDomain.CurrentDomain.UnhandledException += (s, e) => ReportarFatal(e.ExceptionObject);
TaskScheduler.UnobservedTaskException += (s, e) => { Reportar(e.Exception); e.SetObserved(); };
```

### 7.3.4 Logging

- Archivo rotativo diario en `%LocalAppData%\DrugstorePOS\logs\app-YYYYMMDD.log`, retención de 30 días.
- Niveles: `Information` (inicio/cierre, venta confirmada N°, backup), `Warning` (validaciones, impresión), `Error`, `Fatal`.
- **No registrar** datos innecesarios; sí el N° de venta, IDs y mensajes técnicos.

## 7.4 Validaciones transversales

| Regla | Detalle |
|---|---|
| Normalización de texto | `Trim()`, colapsar espacios múltiples; `nombre_busqueda` = minúsculas + sin diacríticos (`NormalizationForm.FormD`). |
| Código de barras | Solo caracteres alfanuméricos; longitud 4–20; se guarda tal cual (sin ceros agregados). Opcional: validar dígito verificador EAN-13 con advertencia (no bloqueo). |
| Dinero | `decimal`, 2 decimales, `MidpointRounding.AwayFromZero`; conversión `centavos = (long)Math.Round(valor * 100m, MidpointRounding.AwayFromZero)`. |
| Tope de cantidades | Cantidad por línea ≤ 9.999; total de venta ≤ 99.999.999,99. |
| Fecha/hora | Usar un `IClock` inyectable (`DateTime.Now`) para permitir pruebas deterministas. |

## 7.5 Estrategia de respaldo (backup) local

### 7.5.1 Método técnico

- **Prohibido** copiar el archivo `.db` con `File.Copy` mientras la app lo usa (con WAL se pierde consistencia).
- **Método principal:** API de backup en línea de SQLite, expuesta en `Microsoft.Data.Sqlite` como `SqliteConnection.BackupDatabase(destino)`; produce una copia consistente, coherente en un instante dado, sin detener la aplicación.
- **Método alternativo:** `VACUUM INTO 'ruta\drugstore_YYYYMMDD_HHmmss.db'` (genera copia compactada y consistente).
- Tras crear el respaldo: abrirlo en modo solo lectura y ejecutar `PRAGMA integrity_check;` (debe devolver `ok`). Si falla → eliminar el archivo, registrar `Error` y notificar.
- El respaldo se escribe primero con extensión temporal `.tmp` y se renombra al finalizar (evita archivos parciales).

### 7.5.2 Disparadores

| Disparador | Condición | Motivo registrado |
|---|---|---|
| Automático al iniciar | Último respaldo exitoso con más de 24 h | `auto-inicio` |
| Automático al cerrar | Opción habilitada en configuración (por defecto **activa**) | `auto-cierre` |
| Manual | Botón "Crear respaldo ahora" / `Ctrl+B` | `manual` |
| Previo a migración de esquema | Siempre que `user_version` < versión esperada | `pre-migracion-N` |
| Previo a actualización masiva de precios | Siempre | `pre-actualizacion-masiva` |
| Previo a restauración | Siempre (respaldo de seguridad del estado actual) | `pre-restauracion` |

### 7.5.3 Ubicación, nomenclatura y retención

- **Carpeta principal:** `%LocalAppData%\DrugstorePOS\backups\` (configurable).
- **Carpeta secundaria (recomendada):** unidad externa/pendrive configurada por el usuario; si está disponible, se copia allí el respaldo (si no está conectada, se omite sin error y se avisa con un mensaje no bloqueante).
- **Nombre:** `drugstore_YYYYMMDD_HHmmss_<motivo>.db`.
- **Retención (`AplicarRetencion`):** conservar los últimos **14 respaldos diarios** (uno por día, el más reciente) y **6 mensuales** (el último de cada mes); eliminar el resto. Los respaldos `pre-*` se conservan 30 días.
- Si el espacio libre del disco es < 3× el tamaño de la BD → no crear respaldo y avisar.

### 7.5.4 Restauración

1. El operador elige un respaldo en `RespaldoForm` y confirma (advertencia: se perderán datos posteriores a ese respaldo).
2. Se verifica `integrity_check` del respaldo seleccionado.
3. Se crea un respaldo de seguridad (`pre-restauracion`) de la BD actual.
4. Se cierran todas las conexiones, se limpia el *pool* (`SqliteConnection.ClearAllPools()`) y se eliminan `-wal` y `-shm`.
5. Se reemplaza `drugstore.db` por la copia seleccionada.
6. La aplicación **se reinicia** automáticamente.

### 7.5.5 Recuperación ante corrupción

Si `DatabaseInitializer` detecta `SQLITE_CORRUPT`/`SQLITE_NOTADB` o falla `PRAGMA quick_check` al arrancar: se muestra un diálogo con el último respaldo válido, ofreciendo **restaurar** o **cerrar**; nunca se continúa operando sobre una BD dañada.

## 7.6 Rendimiento

- Búsquedas por código de barras siempre contra el índice único (`WHERE codigo_barras = @c`).
- Paginación en grillas grandes (200 filas) en lugar de cargar todo el catálogo.
- Cálculo de ítems/totales del carrito en memoria (sin consultar la BD en cada pulsación).
- Reportes con agregaciones en SQL (`SUM`, `GROUP BY`), no en C#.
- Doble *buffering* en `DataGridView` y `SuspendLayout/ResumeLayout` en actualizaciones masivas de UI.
- `ANALYZE` / `PRAGMA optimize` al cerrar la aplicación y `VACUUM` mensual (opción en "Sistema → Mantenimiento") para compactar el archivo.

**Consultas de referencia de reportes (diseño):**

```sql
-- Resumen del rango (ventas completadas)
SELECT COUNT(*)                         AS cantidad_ventas,
       COALESCE(SUM(total_centavos),0)  AS total,
       COALESCE(SUM(costo_total_centavos),0) AS costo,
       COALESCE(SUM(CASE metodo_pago WHEN 'EFECTIVO' THEN total_centavos END),0) AS efectivo,
       COALESCE(SUM(CASE metodo_pago WHEN 'DEBITO_TRANSFERENCIA' THEN total_centavos END),0) AS debito
FROM Ventas
WHERE estado = 'COMPLETADA' AND fecha_hora >= @desde AND fecha_hora < @hasta;

-- Ventas por día (mensual)
SELECT substr(fecha_hora,1,10) AS dia, COUNT(*) AS ventas,
       SUM(total_centavos) AS total, SUM(total_centavos - costo_total_centavos) AS ganancia
FROM Ventas
WHERE estado = 'COMPLETADA' AND fecha_hora >= @desde AND fecha_hora < @hasta
GROUP BY dia ORDER BY dia;

-- Ventas por mes (anual)
SELECT substr(fecha_hora,1,7) AS mes, COUNT(*) AS ventas, SUM(total_centavos) AS total,
       SUM(total_centavos - costo_total_centavos) AS ganancia
FROM Ventas
WHERE estado = 'COMPLETADA' AND fecha_hora >= @desde AND fecha_hora < @hasta
GROUP BY mes ORDER BY mes;

-- Más vendidos
SELECT vi.producto_id, vi.nombre,
       SUM(vi.cantidad)          AS unidades,
       SUM(vi.subtotal_centavos) AS total,
       SUM(vi.subtotal_centavos - vi.cantidad * vi.costo_unitario_centavos) AS ganancia
FROM VentaItems vi
JOIN Ventas v ON v.id = vi.venta_id
WHERE v.estado = 'COMPLETADA' AND v.fecha_hora >= @desde AND v.fecha_hora < @hasta
GROUP BY vi.producto_id, vi.nombre
ORDER BY unidades DESC, total DESC
LIMIT @top;

-- Productos con stock bajo
SELECT * FROM Productos WHERE activo = 1 AND stock_actual <= stock_minimo ORDER BY (stock_actual - stock_minimo);
```

## 7.7 Seguridad y privacidad

- Aunque no hay login, la BD reside en el perfil del usuario de Windows (permisos NTFS por defecto).
- Sin conexiones de red ni telemetría; no se incluye ninguna dependencia que las realice.
- Uso exclusivo de **consultas parametrizadas** (`@param`) — prohibida la concatenación de SQL con datos de usuario.
- Los respaldos no se cifran en v1.0; se recomienda documentar al cliente que deben custodiarse físicamente.

## 7.8 Extensibilidad prevista (no implementar en v1.0)

| Evolución | Impacto de diseño ya contemplado |
|---|---|
| Productos por peso/fraccionados | Migrar `cantidad` y `stock` a `REAL`/milésimas; cambiar tipos en dominio. |
| Nuevos métodos de pago | `metodo_pago` con `CHECK`: agregar valores por migración; enum `MetodoPago`. |
| Compras a proveedores | Nuevas tablas `Compras`/`CompraItems` generando movimientos `INGRESO`. |
| Autenticación/roles | Capa de servicios ya aislada; agregar `IUsuarioActual`. |
| Facturación fiscal | `ITicketPrinter`/`ITicketFormatter` reemplazables; `Ventas` conserva datos suficientes. |
| Exportación de reportes | `IReporteService` ya devuelve DTOs listos para exportar. |

## 7.9 Estrategia de pruebas

| Nivel | Alcance | Herramientas |
|---|---|---|
| Unitarias (Business) | Cálculo de vuelto, redondeo de porcentajes, carrito, `TicketFormatter` (comparación de texto contra plantillas esperadas de 32 y 48 col), validaciones. | xUnit + Moq |
| Integración (Data) | Repositorios y `UnitOfWork` sobre una BD SQLite temporal; verificación de `ConfirmarVenta` (commit y rollback), unicidad de código de barras, índices. | xUnit + archivo `.db` temporal |
| Escenarios críticos | Venta con stock insuficiente en la 3.ª línea ⇒ **ninguna** línea descontada; corte simulado (excepción antes del `Commit`) ⇒ BD sin cambios; backup + restore ⇒ datos idénticos; actualización masiva + reversión ⇒ precios originales. | xUnit |
| UI manual (checklist) | Flujo POS completo solo con teclado; lector de código de barras real; impresión en comandera 58 y 80 mm; DPI 100/125/150 %. | Manual |

## 7.10 Despliegue

- Publicación **self-contained** `win-x64` (opcionalmente *single-file*), sin requerir instalar .NET en la PC del cliente.
- Instalador (MSIX o Inno Setup) que crea accesos directos; la BD **no** se guarda en la carpeta de instalación (`Program Files` es de solo lectura para el usuario estándar).
- Primer arranque: se crea la BD, se aplican migraciones, se insertan rubros y configuración por defecto y se muestra un asistente breve (nombre del comercio, ancho de papel, impresora).
- Actualizaciones: reemplazo de binarios + migraciones automáticas con respaldo previo.

---

# 8. Criterios de aceptación y plan de implementación

## 8.1 Criterios de aceptación (resumen)

| ID | Criterio |
|---|---|
| CA-01 | Escanear un código existente agrega el producto al carrito en < 100 ms; escanearlo de nuevo suma 1 unidad. |
| CA-02 | Una venta confirmada descuenta stock, crea la venta, los ítems y los movimientos en **una sola transacción**; ante cualquier error no queda ningún cambio parcial. |
| CA-03 | Con stock insuficiente (y sin permitir negativo) la venta es rechazada indicando el producto. |
| CA-04 | El vuelto = recibido − total, exacto al centavo; el botón Confirmar se deshabilita si recibido < total. |
| CA-05 | El ticket se genera en 32 y 48 columnas sin desbordes, con nombres largos ajustados. |
| CA-06 | La actualización masiva exige filtro (rubro y/o proveedor), muestra previsualización, genera respaldo previo, es atómica y reversible. |
| CA-07 | El badge de stock bajo refleja productos activos con `stock_actual <= stock_minimo`. |
| CA-08 | Reportes diario/mensual excluyen ventas anuladas y muestran total, ganancia estimada y Top N de productos. |
| CA-09 | Se genera respaldo automático al iniciar (si pasaron 24 h), verificado con `integrity_check`, y la restauración funciona. |
| CA-10 | La app funciona sin conexión a internet y sin ninguna pantalla de autenticación. |
| CA-11 | No se permite abrir una segunda instancia de la aplicación. |

## 8.2 Plan de implementación sugerido (hitos)

| Hito | Contenido | Entregable verificable |
|---|---|---|
| **H1 — Fundaciones** | Solución, proyectos, DI, `SqliteConnectionFactory`, `DatabaseInitializer`, migración 001, logging, manejo global de errores, instancia única. | App vacía que crea la BD y registra logs. |
| **H2 — Catálogos e Inventario** | Repositorios y servicios de Rubros, Proveedores, Productos, kardex; `InventarioView` + `ProductoEditForm`; alerta de stock bajo. | CU-01 a CU-06. |
| **H3 — Punto de venta** | `Carrito`, `VentaService`, `UnitOfWork`, `VentasView`, `CobroForm`, anulación. | CU-09, CU-10; pruebas de transacción. |
| **H4 — Ticket e impresión** | `TicketFormatter` 32/48 col, `ITicketPrinter` (PrintDocument), configuración de comercio/impresora. | CU-11; CA-05. |
| **H5 — Precios** | `PreciosView`, edición manual, actualización masiva con previsualización, historial y reversión. | CU-07, CU-08; CA-06. |
| **H6 — Reportes** | `ReporteRepository`, `ReportesView` con tarjetas, gráfico y ranking. | CU-12; CA-08. |
| **H7 — Respaldo y endurecimiento** | `BackupService`, retención, restauración, verificación de integridad; pruebas de escenarios críticos; instalador. | CU-13; CA-09, CA-11. |

---

*Fin del documento — `SPEC.md` v1.0*
