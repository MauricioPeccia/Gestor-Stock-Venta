# Guía actualizada: sistema de stock y caja para kiosco/drugstore (C# + MySQL)

Con lo que me contaste, esta versión **reemplaza** a la anterior y es más simple.

## Qué cambia respecto a la primera versión

| Tema | Antes | Ahora |
|---|---|---|
| Login, roles, usuarios | Sí | **Se eliminan.** Un solo usuario (el dueño), sin pantalla de login |
| Cajas | Tabla de cajas + sesiones por turno | **Una sola caja, una "jornada" por día** |
| Proveedor en compras | Obligatorio | **Opcional** (`NULL`) |
| Medios de pago | Tabla configurable | Solo **Efectivo y Transferencia** (un `ENUM`) |
| Facturación, clientes, cta. cte. | Reservado | **Fuera de alcance** (versión 2) |
| Reportes | PDF/Excel/pantalla | **Solo PDF**, de uso ocasional |
| Proyectos de la solución | 4 | **3** (Entidades, Servicios, UI) |

**Decisiones que tomé por mí mismo** (decime si alguna no te sirve):

1. **Una jornada por fecha:** la base impide dos jornadas con la misma fecha (`UNIQUE`). Si el kiosco abre de madrugada y cierra pasada la medianoche, las ventas pertenecen a la jornada que siga **abierta**, no a la fecha del reloj. Así el "día" del dueño coincide con su día de trabajo real.
2. **Si se olvidan de cerrar la caja:** al abrir el programa, si hay una jornada abierta de un día anterior, el sistema obliga a cerrarla antes de seguir.
3. **Cantidades:** productos en `UN` se venden en enteros; productos en `KG`/`LT` aceptan decimales (ej. 0,250 kg).
4. **Vender sin stock cargado:** en un kiosco es común que el stock no esté 100 % al día. Si falta stock, el sistema **avisa y pregunta** si querés vender igual, en vez de bloquear. El stock queda negativo y eso sirve como señal de que hay que ajustar.
5. **Código de barras opcional:** golosinas sueltas, por ejemplo, no tienen código. Se buscan por nombre.
6. **Compras pagadas de la caja:** al cargar una compra hay un tilde "Pagada con efectivo de la caja". Si está marcado, se genera automáticamente un egreso en la jornada abierta, para que el arqueo cierre.

---

## FASE 1. Preparar el entorno

1. Instalar **Visual Studio 2022** (o más nuevo) con la carga de trabajo **"Desarrollo de escritorio de .NET"**.
2. Instalar **MySQL Server 8.x** en la computadora del negocio y **MySQL Workbench**.
   - Elegí "Server only" o "Developer Default". Puerto `3306`.
   - En "Windows Service", dejá marcado **iniciar MySQL al arrancar Windows**. Sin esto, si se apaga la PC, el sistema no abre.
   - Anotá la contraseña de `root` y guardala fuera de la PC (papel o gestor de contraseñas).
3. Crear la base y un usuario propio de la aplicación (en Workbench, conectado como `root`):

```sql
CREATE DATABASE kiosco
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

CREATE USER 'app_kiosco'@'localhost' IDENTIFIED BY 'CambiaEstaClave_2025!';
GRANT SELECT, INSERT, UPDATE, DELETE ON kiosco.* TO 'app_kiosco'@'localhost';
FLUSH PRIVILEGES;
```

   El usuario de la app **no** tiene `DROP` ni `ALTER`. Si algo sale mal en el programa, no puede destruir la estructura.
4. Crear un repositorio **Git** local y hacer un commit al terminar cada fase.

> Nota honesta: para una sola PC y un solo usuario, SQLite alcanzaría y evitaría instalar un servidor. Pero ya elegiste MySQL, y migrar después a más cajas sería más fácil. Seguimos con MySQL.

---

## FASE 2. Base de datos

### 2.1 Reglas de diseño

- **Dinero:** `DECIMAL(12,2)` en MySQL y `decimal` en C#. Nunca `float`/`double`.
- **Cantidades:** `DECIMAL(12,3)` (permite 0,250 kg).
- **Motor InnoDB** (transacciones y claves foráneas).
- **Borrado lógico:** productos, categorías y proveedores se "desactivan", no se borran, porque las ventas históricas dependen de ellos.
- **Stock con doble registro:** `productos.stock_actual` (consulta rápida) + `movimientos_stock` (historial que explica cada cambio).
- **Precio histórico:** `venta_detalle` guarda precio y costo **del momento de la venta**.
- **Hora:** siempre hora local de la PC. El C# manda `DateTime.Now`, así todo queda consistente.

### 2.2 Script completo

Ejecutalo en Workbench sobre la base `kiosco`:

```sql
USE kiosco;

-- ============ CATÁLOGO ============
CREATE TABLE categorias (
  id      INT AUTO_INCREMENT PRIMARY KEY,
  nombre  VARCHAR(100) NOT NULL UNIQUE,
  activo  TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB;

CREATE TABLE proveedores (
  id            INT AUTO_INCREMENT PRIMARY KEY,
  nombre        VARCHAR(150) NOT NULL,
  telefono      VARCHAR(50)  NULL,
  observaciones VARCHAR(255) NULL,
  activo        TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB;

CREATE TABLE productos (
  id            INT AUTO_INCREMENT PRIMARY KEY,
  codigo        VARCHAR(50) NULL UNIQUE,          -- NULL permitido: varios NULL no chocan
  nombre        VARCHAR(150) NOT NULL,
  categoria_id  INT NULL,
  unidad        ENUM('UN','KG','LT') NOT NULL DEFAULT 'UN',
  precio_costo  DECIMAL(12,2) NOT NULL DEFAULT 0,
  precio_venta  DECIMAL(12,2) NOT NULL DEFAULT 0,
  stock_actual  DECIMAL(12,3) NOT NULL DEFAULT 0,
  stock_minimo  DECIMAL(12,3) NOT NULL DEFAULT 0,
  activo        TINYINT(1) NOT NULL DEFAULT 1,
  creado_en     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (categoria_id) REFERENCES categorias(id),
  INDEX idx_prod_nombre (nombre)
) ENGINE=InnoDB;

CREATE TABLE historial_precios (
  id                BIGINT AUTO_INCREMENT PRIMARY KEY,
  producto_id       INT NOT NULL,
  costo_anterior    DECIMAL(12,2) NOT NULL,
  costo_nuevo       DECIMAL(12,2) NOT NULL,
  venta_anterior    DECIMAL(12,2) NOT NULL,
  venta_nuevo       DECIMAL(12,2) NOT NULL,
  fecha             DATETIME NOT NULL,
  FOREIGN KEY (producto_id) REFERENCES productos(id),
  INDEX idx_hp_prod (producto_id, fecha)
) ENGINE=InnoDB;

-- ============ STOCK (historial) ============
CREATE TABLE movimientos_stock (
  id              BIGINT AUTO_INCREMENT PRIMARY KEY,
  producto_id     INT NOT NULL,
  tipo            ENUM('COMPRA','VENTA','AJUSTE_POS','AJUSTE_NEG','ANULACION_VENTA') NOT NULL,
  cantidad        DECIMAL(12,3) NOT NULL,        -- siempre positiva; el tipo define el signo
  stock_anterior  DECIMAL(12,3) NOT NULL,
  stock_nuevo     DECIMAL(12,3) NOT NULL,
  referencia_tipo VARCHAR(20) NULL,              -- 'VENTA','COMPRA'
  referencia_id   BIGINT NULL,
  motivo          VARCHAR(255) NULL,
  fecha           DATETIME NOT NULL,
  FOREIGN KEY (producto_id) REFERENCES productos(id),
  INDEX idx_mov_prod_fecha (producto_id, fecha)
) ENGINE=InnoDB;

-- ============ CAJA (una jornada por día) ============
CREATE TABLE jornadas_caja (
  id                 BIGINT AUTO_INCREMENT PRIMARY KEY,
  fecha              DATE NOT NULL UNIQUE,        -- una sola jornada por fecha
  apertura_en        DATETIME NOT NULL,
  monto_apertura     DECIMAL(12,2) NOT NULL,
  cierre_en          DATETIME NULL,
  efectivo_esperado  DECIMAL(12,2) NULL,
  efectivo_contado   DECIMAL(12,2) NULL,
  diferencia         DECIMAL(12,2) NULL,
  estado             ENUM('ABIERTA','CERRADA') NOT NULL DEFAULT 'ABIERTA',
  observaciones      VARCHAR(255) NULL,
  INDEX idx_jor_estado (estado)
) ENGINE=InnoDB;

-- ============ COMPRAS ============
CREATE TABLE compras (
  id               BIGINT AUTO_INCREMENT PRIMARY KEY,
  proveedor_id     INT NULL,                      -- OPCIONAL
  nro_comprobante  VARCHAR(50) NULL,
  fecha            DATETIME NOT NULL,
  total            DECIMAL(12,2) NOT NULL,
  pagada_con_caja  TINYINT(1) NOT NULL DEFAULT 0,
  observaciones    VARCHAR(255) NULL,
  FOREIGN KEY (proveedor_id) REFERENCES proveedores(id)
) ENGINE=InnoDB;

CREATE TABLE compra_detalle (
  id              BIGINT AUTO_INCREMENT PRIMARY KEY,
  compra_id       BIGINT NOT NULL,
  producto_id     INT NOT NULL,
  cantidad        DECIMAL(12,3) NOT NULL,
  costo_unitario  DECIMAL(12,2) NOT NULL,
  subtotal        DECIMAL(12,2) NOT NULL,
  FOREIGN KEY (compra_id)   REFERENCES compras(id),
  FOREIGN KEY (producto_id) REFERENCES productos(id)
) ENGINE=InnoDB;

-- ============ VENTAS ============
CREATE TABLE ventas (
  id                BIGINT AUTO_INCREMENT PRIMARY KEY,
  jornada_id        BIGINT NOT NULL,
  fecha             DATETIME NOT NULL,
  subtotal          DECIMAL(12,2) NOT NULL,
  descuento         DECIMAL(12,2) NOT NULL DEFAULT 0,
  total             DECIMAL(12,2) NOT NULL,
  estado            ENUM('COMPLETADA','ANULADA') NOT NULL DEFAULT 'COMPLETADA',
  fecha_anulacion   DATETIME NULL,
  motivo_anulacion  VARCHAR(255) NULL,
  FOREIGN KEY (jornada_id) REFERENCES jornadas_caja(id),
  INDEX idx_ventas_fecha (fecha),
  INDEX idx_ventas_jornada (jornada_id)
) ENGINE=InnoDB;

CREATE TABLE venta_detalle (
  id               BIGINT AUTO_INCREMENT PRIMARY KEY,
  venta_id         BIGINT NOT NULL,
  producto_id      INT NOT NULL,
  cantidad         DECIMAL(12,3) NOT NULL,
  precio_unitario  DECIMAL(12,2) NOT NULL,        -- histórico
  costo_unitario   DECIMAL(12,2) NOT NULL,        -- histórico
  subtotal         DECIMAL(12,2) NOT NULL,
  FOREIGN KEY (venta_id)    REFERENCES ventas(id),
  FOREIGN KEY (producto_id) REFERENCES productos(id)
) ENGINE=InnoDB;

CREATE TABLE venta_pagos (
  id        BIGINT AUTO_INCREMENT PRIMARY KEY,
  venta_id  BIGINT NOT NULL,
  metodo    ENUM('EFECTIVO','TRANSFERENCIA') NOT NULL,
  monto     DECIMAL(12,2) NOT NULL,               -- monto APLICADO a la venta (sin vuelto)
  FOREIGN KEY (venta_id) REFERENCES ventas(id)
) ENGINE=InnoDB;

CREATE TABLE movimientos_caja (
  id          BIGINT AUTO_INCREMENT PRIMARY KEY,
  jornada_id  BIGINT NOT NULL,
  tipo        ENUM('INGRESO','EGRESO') NOT NULL,
  concepto    VARCHAR(150) NOT NULL,
  monto       DECIMAL(12,2) NOT NULL,
  origen      ENUM('MANUAL','COMPRA') NOT NULL DEFAULT 'MANUAL',
  compra_id   BIGINT NULL,
  fecha       DATETIME NOT NULL,
  FOREIGN KEY (jornada_id) REFERENCES jornadas_caja(id),
  FOREIGN KEY (compra_id)  REFERENCES compras(id)
) ENGINE=InnoDB;

-- ============ CONFIGURACIÓN (para el encabezado de los PDF) ============
CREATE TABLE configuracion (
  clave  VARCHAR(50) PRIMARY KEY,
  valor  VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

INSERT INTO configuracion (clave, valor) VALUES
  ('nombre_negocio', 'Mi Kiosco'),
  ('direccion', '');
```

### 2.3 Por qué está así

- **`jornadas_caja.fecha UNIQUE`**: garantiza a nivel base de datos "una jornada por día", aunque el programa tuviera un error.
- **`venta_pagos.monto` guarda lo aplicado, no lo recibido.** Si la venta es $1.800 y el cliente da $2.000, se guarda $1.800 en efectivo. El vuelto es solo un cálculo de pantalla. Si guardaras $2.000, el arqueo daría $200 de más.
- **`compras.proveedor_id NULL`**: cumple el pedido de proveedor opcional. Los reportes deben tratar `NULL` como "Sin proveedor".
- **`movimientos_caja.origen`**: distingue los egresos automáticos (por compras) de los manuales (retiros del dueño, gastos).
- **Transferencias no entran en el arqueo físico**, pero sí en el resumen del día.

---

## FASE 3. Crear la solución

1. Visual Studio → **Crear un proyecto → Solución en blanco**, llamada `Kiosco`.
2. Agregar 3 proyectos:

| Proyecto | Tipo | Contiene |
|---|---|---|
| `Kiosco.Entidades` | Biblioteca de clases (.NET 8) | Clases simples que representan los datos. Sin SQL. |
| `Kiosco.Servicios` | Biblioteca de clases (.NET 8) | Acceso a MySQL + reglas de negocio + transacciones. |
| `Kiosco.UI` | Aplicación de Windows Forms (.NET 8) | Pantallas. **Sin SQL.** |

3. Referencias (clic derecho en el proyecto → Agregar → Referencia de proyecto):
   - `Servicios` → `Entidades`
   - `UI` → `Servicios` y `Entidades`
4. Paquetes NuGet:
   - En `Servicios`: `MySqlConnector`, `Dapper`
   - En `UI`: `QuestPDF` (para los PDF) y `Microsoft.Extensions.Configuration.Json`
5. Carpetas:

```
Entidades/  Producto.cs, Venta.cs, JornadaCaja.cs, Compra.cs, ...
Servicios/  Conexion.cs, StockHelper.cs, ProductoServicio.cs, VentaServicio.cs,
            CompraServicio.cs, CajaServicio.cs, Excepciones.cs
UI/         Program.cs, appsettings.json, Forms/...
```

---

## FASE 4. Entidades (proyecto `Kiosco.Entidades`)

```csharp
namespace Kiosco.Entidades;

public class Producto
{
    public int Id { get; set; }
    public string? Codigo { get; set; }
    public string Nombre { get; set; } = "";
    public int? CategoriaId { get; set; }
    public string Unidad { get; set; } = "UN";   // UN, KG, LT
    public decimal PrecioCosto { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public bool Activo { get; set; } = true;
}

public class VentaItem
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = "";
    public string Unidad { get; set; } = "UN";
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Math.Round(Cantidad * PrecioUnitario, 2);
}

public class PagoVenta
{
    public string Metodo { get; set; } = "EFECTIVO";   // EFECTIVO | TRANSFERENCIA
    public decimal Monto { get; set; }
}

public class Venta
{
    public long Id { get; set; }
    public decimal Descuento { get; set; }
    public List<VentaItem> Items { get; set; } = new();
    public List<PagoVenta> Pagos { get; set; } = new();
    public decimal Subtotal => Items.Sum(i => i.Subtotal);
    public decimal Total => Subtotal - Descuento;
}

public class CompraItem
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal Subtotal => Math.Round(Cantidad * CostoUnitario, 2);
}

public class Compra
{
    public int? ProveedorId { get; set; }          // opcional
    public string? NroComprobante { get; set; }
    public string? Observaciones { get; set; }
    public bool PagadaConCaja { get; set; }
    public List<CompraItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
}

public class JornadaCaja
{
    public long Id { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime AperturaEn { get; set; }
    public decimal MontoApertura { get; set; }
    public DateTime? CierreEn { get; set; }
    public decimal? EfectivoEsperado { get; set; }
    public decimal? EfectivoContado { get; set; }
    public decimal? Diferencia { get; set; }
    public string Estado { get; set; } = "ABIERTA";
    public string? Observaciones { get; set; }
}

public class ResumenJornada
{
    public decimal MontoApertura { get; set; }
    public decimal VentasEfectivo { get; set; }
    public decimal VentasTransferencia { get; set; }
    public int CantidadVentas { get; set; }
    public int CantidadAnuladas { get; set; }
    public decimal Ingresos { get; set; }
    public decimal Egresos { get; set; }
    public decimal GananciaBruta { get; set; }

    public decimal EfectivoEsperado => MontoApertura + VentasEfectivo + Ingresos - Egresos;
    public decimal TotalVendido => VentasEfectivo + VentasTransferencia;
}
```

---

## FASE 5. Conexión y arranque

### 5.1 `appsettings.json` (en `Kiosco.UI`)

Clic derecho → Propiedades → **Copiar al directorio de salida: Copiar si es posterior**.

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Port=3306;Database=kiosco;User=app_kiosco;Password=CambiaEstaClave_2025!;"
  }
}
```

### 5.2 `Conexion.cs` (en `Kiosco.Servicios`)

```csharp
using MySqlConnector;

namespace Kiosco.Servicios;

public static class Conexion
{
    public static string Cadena { get; set; } = "";

    public static async Task<MySqlConnection> AbrirAsync()
    {
        var cn = new MySqlConnection(Cadena);
        await cn.OpenAsync();
        return cn;
    }
}
```

### 5.3 `Excepciones.cs`

```csharp
namespace Kiosco.Servicios;

public class StockInsuficienteException : Exception
{
    public string Producto { get; }
    public decimal Disponible { get; }
    public StockInsuficienteException(string producto, decimal disponible)
        : base($"Stock insuficiente de '{producto}'. Disponible: {disponible}")
    {
        Producto = producto;
        Disponible = disponible;
    }
}

public class NegocioException : Exception
{
    public NegocioException(string mensaje) : base(mensaje) { }
}
```

`NegocioException` es para errores "esperables" (caja cerrada, datos inválidos). La UI muestra su mensaje tal cual. Cualquier otra excepción es un error técnico: se registra en un log y se muestra un mensaje genérico.

### 5.4 `Program.cs` (en `Kiosco.UI`)

```csharp
using Kiosco.Servicios;
using Microsoft.Extensions.Configuration;

namespace Kiosco.UI;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Dapper: precio_venta -> PrecioVenta
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        Conexion.Cadena = config.GetConnectionString("Default")!;

        // Log de errores no controlados
        Application.ThreadException += (s, e) => RegistrarError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (s, e) => RegistrarError((Exception)e.ExceptionObject);

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        Application.Run(new FrmPrincipal());
    }

    static void RegistrarError(Exception ex)
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "errores.log");
        File.AppendAllText(ruta, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}\n\n");
        MessageBox.Show("Ocurrió un error inesperado. Los detalles quedaron guardados en errores.log.",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

Sobre la licencia de QuestPDF: la edición "Community" es gratuita para negocios chicos, pero revisá las condiciones vigentes en su sitio antes de usarla comercialmente.

---

## FASE 6. Servicios

### 6.1 `StockHelper.cs`: la única puerta para tocar el stock

Todo cambio de stock (venta, compra, ajuste, anulación) pasa por este método. Así la lógica está escrita **una sola vez** y nunca se modifica `stock_actual` por otro camino.

```csharp
using Dapper;
using Kiosco.Entidades;
using MySqlConnector;

namespace Kiosco.Servicios;

internal static class StockHelper
{
    /// delta > 0 suma stock, delta < 0 resta. Devuelve el producto con su estado ANTERIOR al cambio.
    public static async Task<Producto> AplicarAsync(
        MySqlConnection cn, MySqlTransaction tx,
        int productoId, string tipo, decimal delta,
        string? refTipo, long? refId, string? motivo, bool permitirNegativo)
    {
        // FOR UPDATE bloquea la fila hasta terminar la transacción
        var prod = await cn.QuerySingleAsync<Producto>(
            "SELECT * FROM productos WHERE id=@productoId FOR UPDATE",
            new { productoId }, tx);

        var nuevo = prod.StockActual + delta;
        if (nuevo < 0 && !permitirNegativo)
            throw new StockInsuficienteException(prod.Nombre, prod.StockActual);

        await cn.ExecuteAsync(
            "UPDATE productos SET stock_actual=@nuevo WHERE id=@productoId",
            new { nuevo, productoId }, tx);

        await cn.ExecuteAsync(
            @"INSERT INTO movimientos_stock
              (producto_id,tipo,cantidad,stock_anterior,stock_nuevo,
               referencia_tipo,referencia_id,motivo,fecha)
              VALUES (@productoId,@tipo,@cantidad,@anterior,@nuevo,
                      @refTipo,@refId,@motivo,@fecha)",
            new
            {
                productoId, tipo,
                cantidad = Math.Abs(delta),
                anterior = prod.StockActual, nuevo,
                refTipo, refId, motivo,
                fecha = DateTime.Now
            }, tx);

        return prod;
    }
}
```

Con un solo usuario no habrá dos cajeros pisándose, pero el `FOR UPDATE` no molesta y te protege si en el futuro agregás una segunda PC.

### 6.2 `ProductoServicio.cs`

Métodos que necesita:

```csharp
using Dapper;
using Kiosco.Entidades;
using MySqlConnector;

namespace Kiosco.Servicios;

public class ProductoServicio
{
    public async Task<Producto?> ObtenerPorCodigoAsync(string codigo)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryFirstOrDefaultAsync<Producto>(
            "SELECT * FROM productos WHERE codigo=@codigo AND activo=1", new { codigo });
    }

    public async Task<List<Producto>> BuscarAsync(string texto)
    {
        await using var cn = await Conexion.AbrirAsync();
        var r = await cn.QueryAsync<Producto>(
            @"SELECT * FROM productos
              WHERE activo=1 AND (nombre LIKE @t OR codigo LIKE @t)
              ORDER BY nombre LIMIT 100",
            new { t = $"%{texto}%" });
        return r.ToList();
    }

    public async Task<int> CrearAsync(Producto p, decimal stockInicial)
    {
        Validar(p);
        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var id = await cn.ExecuteScalarAsync<int>(
                @"INSERT INTO productos
                  (codigo,nombre,categoria_id,unidad,precio_costo,precio_venta,stock_minimo)
                  VALUES (@Codigo,@Nombre,@CategoriaId,@Unidad,@PrecioCosto,@PrecioVenta,@StockMinimo);
                  SELECT LAST_INSERT_ID();",
                new { Codigo = string.IsNullOrWhiteSpace(p.Codigo) ? null : p.Codigo.Trim(),
                      p.Nombre, p.CategoriaId, p.Unidad, p.PrecioCosto, p.PrecioVenta, p.StockMinimo }, tx);

            if (stockInicial > 0)
                await StockHelper.AplicarAsync(cn, tx, id, "AJUSTE_POS", stockInicial,
                    null, null, "Stock inicial", true);

            await tx.CommitAsync();
            return id;
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            await tx.RollbackAsync();
            throw new NegocioException("Ya existe un producto con ese código.");
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task ActualizarAsync(Producto p)
    {
        Validar(p);
        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var antes = await cn.QuerySingleAsync<Producto>(
                "SELECT * FROM productos WHERE id=@Id FOR UPDATE", new { p.Id }, tx);

            if (antes.PrecioCosto != p.PrecioCosto || antes.PrecioVenta != p.PrecioVenta)
                await cn.ExecuteAsync(
                    @"INSERT INTO historial_precios
                      (producto_id,costo_anterior,costo_nuevo,venta_anterior,venta_nuevo,fecha)
                      VALUES (@Id,@ca,@cn,@va,@vn,@f)",
                    new { p.Id, ca = antes.PrecioCosto, cn = p.PrecioCosto,
                          va = antes.PrecioVenta, vn = p.PrecioVenta, f = DateTime.Now }, tx);

            // OJO: no se actualiza stock_actual acá, solo con movimientos
            await cn.ExecuteAsync(
                @"UPDATE productos SET codigo=@Codigo, nombre=@Nombre, categoria_id=@CategoriaId,
                  unidad=@Unidad, precio_costo=@PrecioCosto, precio_venta=@PrecioVenta,
                  stock_minimo=@StockMinimo WHERE id=@Id",
                new { Codigo = string.IsNullOrWhiteSpace(p.Codigo) ? null : p.Codigo.Trim(),
                      p.Nombre, p.CategoriaId, p.Unidad, p.PrecioCosto, p.PrecioVenta,
                      p.StockMinimo, p.Id }, tx);
            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task DesactivarAsync(int id)
    {
        await using var cn = await Conexion.AbrirAsync();
        await cn.ExecuteAsync("UPDATE productos SET activo=0 WHERE id=@id", new { id });
    }

    /// Ajuste manual (rotura, vencimiento, conteo físico). Motivo obligatorio.
    public async Task AjustarStockAsync(int productoId, decimal nuevoStockReal, string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new NegocioException("El motivo del ajuste es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var actual = await cn.ExecuteScalarAsync<decimal>(
                "SELECT stock_actual FROM productos WHERE id=@productoId FOR UPDATE",
                new { productoId }, tx);
            var delta = nuevoStockReal - actual;
            if (delta != 0)
                await StockHelper.AplicarAsync(cn, tx, productoId,
                    delta > 0 ? "AJUSTE_POS" : "AJUSTE_NEG", delta, null, null, motivo, true);
            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    /// Aumento masivo de precios (útil con inflación). categoriaId null = todos.
    public async Task<int> AumentarPreciosAsync(decimal porcentaje, int? categoriaId, int redondearA = 10)
    {
        if (porcentaje == 0) throw new NegocioException("El porcentaje no puede ser 0.");
        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var f = 1 + porcentaje / 100m;
            // Guardar historial antes de modificar
            await cn.ExecuteAsync(
                @"INSERT INTO historial_precios
                  (producto_id,costo_anterior,costo_nuevo,venta_anterior,venta_nuevo,fecha)
                  SELECT id, precio_costo, precio_costo,
                         precio_venta, ROUND(precio_venta*@f/@r)*@r, @fecha
                  FROM productos
                  WHERE activo=1 AND (@cat IS NULL OR categoria_id=@cat)",
                new { f, r = redondearA, fecha = DateTime.Now, cat = categoriaId }, tx);

            var filas = await cn.ExecuteAsync(
                @"UPDATE productos SET precio_venta = ROUND(precio_venta*@f/@r)*@r
                  WHERE activo=1 AND (@cat IS NULL OR categoria_id=@cat)",
                new { f, r = redondearA, cat = categoriaId }, tx);
            await tx.CommitAsync();
            return filas;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task<List<Producto>> BajoStockMinimoAsync()
    {
        await using var cn = await Conexion.AbrirAsync();
        var r = await cn.QueryAsync<Producto>(
            "SELECT * FROM productos WHERE activo=1 AND stock_actual <= stock_minimo ORDER BY nombre");
        return r.ToList();
    }

    private static void Validar(Producto p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre)) throw new NegocioException("El nombre es obligatorio.");
        if (p.PrecioVenta < 0 || p.PrecioCosto < 0) throw new NegocioException("Los precios no pueden ser negativos.");
    }
}
```

Puntos a notar:
- **Código vacío se guarda como `NULL`**, no como texto vacío. Dos códigos `""` chocarían con el `UNIQUE`; dos `NULL` no.
- **El stock inicial es un movimiento**, no un valor puesto a mano: queda registro de dónde salió.
- **Aumento masivo:** el redondeo (`redondearA`) evita precios como $1.347,26. El historial queda guardado por si querés revertir.

### 6.3 `CajaServicio.cs` (jornada diaria)

```csharp
using Dapper;
using Kiosco.Entidades;
using MySqlConnector;

namespace Kiosco.Servicios;

public class CajaServicio
{
    public async Task<JornadaCaja?> ObtenerAbiertaAsync()
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryFirstOrDefaultAsync<JornadaCaja>(
            "SELECT * FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1");
    }

    public async Task<JornadaCaja?> ObtenerPorFechaAsync(DateTime fecha)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryFirstOrDefaultAsync<JornadaCaja>(
            "SELECT * FROM jornadas_caja WHERE fecha=@f", new { f = fecha.Date });
    }

    public async Task<long> AbrirAsync(decimal montoApertura)
    {
        if (montoApertura < 0) throw new NegocioException("El monto inicial no puede ser negativo.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var abierta = await cn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM jornadas_caja WHERE estado='ABIERTA' FOR UPDATE", transaction: tx);
            if (abierta > 0) throw new NegocioException("Ya hay una caja abierta. Cerrala primero.");

            var hoy = DateTime.Today;
            var existe = await cn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM jornadas_caja WHERE fecha=@hoy", new { hoy }, tx);
            if (existe > 0)
                throw new NegocioException("La caja de hoy ya fue abierta y cerrada. " +
                    "Si necesitás seguir vendiendo, reabrila desde Caja.");

            var id = await cn.ExecuteScalarAsync<long>(
                @"INSERT INTO jornadas_caja (fecha,apertura_en,monto_apertura)
                  VALUES (@hoy,@ahora,@montoApertura);
                  SELECT LAST_INSERT_ID();",
                new { hoy, ahora = DateTime.Now, montoApertura }, tx);
            await tx.CommitAsync();
            return id;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task RegistrarMovimientoAsync(string tipo, string concepto, decimal monto)
    {
        if (tipo != "INGRESO" && tipo != "EGRESO") throw new NegocioException("Tipo inválido.");
        if (string.IsNullOrWhiteSpace(concepto)) throw new NegocioException("El concepto es obligatorio.");
        if (monto <= 0) throw new NegocioException("El monto debe ser mayor a cero.");

        await using var cn = await Conexion.AbrirAsync();
        var jornadaId = await cn.ExecuteScalarAsync<long?>(
            "SELECT id FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1")
            ?? throw new NegocioException("No hay una caja abierta.");

        await cn.ExecuteAsync(
            @"INSERT INTO movimientos_caja (jornada_id,tipo,concepto,monto,fecha)
              VALUES (@jornadaId,@tipo,@concepto,@monto,@fecha)",
            new { jornadaId, tipo, concepto = concepto.Trim(), monto, fecha = DateTime.Now });
    }

    public async Task<ResumenJornada> ObtenerResumenAsync(long jornadaId)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await ResumenAsync(cn, null, jornadaId);
    }

    private static Task<ResumenJornada> ResumenAsync(MySqlConnection cn, MySqlTransaction? tx, long id) =>
        cn.QuerySingleAsync<ResumenJornada>(
            @"SELECT
                j.monto_apertura AS MontoApertura,
                (SELECT COALESCE(SUM(vp.monto),0) FROM venta_pagos vp
                   JOIN ventas v ON v.id=vp.venta_id
                  WHERE v.jornada_id=j.id AND v.estado='COMPLETADA' AND vp.metodo='EFECTIVO') AS VentasEfectivo,
                (SELECT COALESCE(SUM(vp.monto),0) FROM venta_pagos vp
                   JOIN ventas v ON v.id=vp.venta_id
                  WHERE v.jornada_id=j.id AND v.estado='COMPLETADA' AND vp.metodo='TRANSFERENCIA') AS VentasTransferencia,
                (SELECT COUNT(*) FROM ventas WHERE jornada_id=j.id AND estado='COMPLETADA') AS CantidadVentas,
                (SELECT COUNT(*) FROM ventas WHERE jornada_id=j.id AND estado='ANULADA') AS CantidadAnuladas,
                (SELECT COALESCE(SUM(monto),0) FROM movimientos_caja
                  WHERE jornada_id=j.id AND tipo='INGRESO') AS Ingresos,
                (SELECT COALESCE(SUM(monto),0) FROM movimientos_caja
                  WHERE jornada_id=j.id AND tipo='EGRESO') AS Egresos,
                (SELECT COALESCE(SUM((d.precio_unitario-d.costo_unitario)*d.cantidad),0)
                   FROM venta_detalle d JOIN ventas v ON v.id=d.venta_id
                  WHERE v.jornada_id=j.id AND v.estado='COMPLETADA') AS GananciaBruta
              FROM jornadas_caja j WHERE j.id=@id",
            new { id }, tx);

    /// Arqueo ciego: el dueño cuenta el efectivo y lo ingresa SIN ver el esperado.
    public async Task<ResumenJornada> CerrarAsync(long jornadaId, decimal efectivoContado, string? observaciones)
    {
        if (efectivoContado < 0) throw new NegocioException("El efectivo contado no puede ser negativo.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var estado = await cn.ExecuteScalarAsync<string>(
                "SELECT estado FROM jornadas_caja WHERE id=@jornadaId FOR UPDATE", new { jornadaId }, tx);
            if (estado != "ABIERTA") throw new NegocioException("Esa caja ya está cerrada.");

            var r = await ResumenAsync(cn, tx, jornadaId);
            await cn.ExecuteAsync(
                @"UPDATE jornadas_caja SET estado='CERRADA', cierre_en=@ahora,
                    efectivo_esperado=@esp, efectivo_contado=@cont,
                    diferencia=@dif, observaciones=@obs
                  WHERE id=@jornadaId",
                new { ahora = DateTime.Now, esp = r.EfectivoEsperado, cont = efectivoContado,
                      dif = efectivoContado - r.EfectivoEsperado, obs = observaciones, jornadaId }, tx);
            await tx.CommitAsync();
            return r;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    /// Para corregir una jornada cerrada por error (no puede haber otra abierta).
    public async Task ReabrirAsync(long jornadaId)
    {
        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            if (await cn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM jornadas_caja WHERE estado='ABIERTA' FOR UPDATE", transaction: tx) > 0)
                throw new NegocioException("Hay otra caja abierta. Cerrala antes de reabrir.");

            await cn.ExecuteAsync(
                @"UPDATE jornadas_caja SET estado='ABIERTA', cierre_en=NULL, efectivo_esperado=NULL,
                  efectivo_contado=NULL, diferencia=NULL WHERE id=@jornadaId", new { jornadaId }, tx);
            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }
}
```

**Fórmula del arqueo:**

```
efectivo_esperado = monto_apertura
                  + ventas cobradas en EFECTIVO (no anuladas)
                  + ingresos manuales
                  − egresos (manuales + compras pagadas de la caja)

diferencia = efectivo_contado − efectivo_esperado
   (negativa = falta plata, positiva = sobra)
```

### 6.4 `VentaServicio.cs`

Orden de la transacción:

1. Validar (ítems, cantidades, pagos = total).
2. Buscar la jornada abierta.
3. Insertar cabecera.
4. Por cada ítem: descontar stock (vía `StockHelper`) y guardar detalle con el **costo del momento**.
5. Guardar pagos.
6. `COMMIT`. Si algo falla: `ROLLBACK` y no queda nada a medias.

```csharp
using Dapper;
using Kiosco.Entidades;

namespace Kiosco.Servicios;

public class VentaServicio
{
    public async Task<long> RegistrarAsync(Venta v, bool permitirSinStock = false)
    {
        if (v.Items.Count == 0) throw new NegocioException("La venta no tiene productos.");
        if (v.Items.Any(i => i.Cantidad <= 0)) throw new NegocioException("Hay cantidades inválidas.");
        if (v.Items.Any(i => i.Unidad == "UN" && i.Cantidad % 1 != 0))
            throw new NegocioException("Los productos por unidad deben venderse en cantidades enteras.");
        if (v.Descuento < 0 || v.Descuento > v.Subtotal) throw new NegocioException("Descuento inválido.");
        if (v.Pagos.Any(p => p.Monto <= 0)) throw new NegocioException("Hay pagos con monto inválido.");
        if (v.Pagos.Sum(p => p.Monto) != v.Total)
            throw new NegocioException("La suma de los pagos no coincide con el total.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var jornadaId = await cn.ExecuteScalarAsync<long?>(
                "SELECT id FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1 FOR UPDATE", transaction: tx)
                ?? throw new NegocioException("No hay una caja abierta. Abrila para poder vender.");

            var ventaId = await cn.ExecuteScalarAsync<long>(
                @"INSERT INTO ventas (jornada_id,fecha,subtotal,descuento,total)
                  VALUES (@jornadaId,@fecha,@sub,@desc,@total);
                  SELECT LAST_INSERT_ID();",
                new { jornadaId, fecha = DateTime.Now, sub = v.Subtotal, desc = v.Descuento, total = v.Total }, tx);

            foreach (var it in v.Items)
            {
                var prod = await StockHelper.AplicarAsync(cn, tx, it.ProductoId, "VENTA",
                    -it.Cantidad, "VENTA", ventaId, null, permitirSinStock);

                await cn.ExecuteAsync(
                    @"INSERT INTO venta_detalle
                      (venta_id,producto_id,cantidad,precio_unitario,costo_unitario,subtotal)
                      VALUES (@ventaId,@ProductoId,@Cantidad,@PrecioUnitario,@costo,@sub)",
                    new { ventaId, it.ProductoId, it.Cantidad, it.PrecioUnitario,
                          costo = prod.PrecioCosto, sub = it.Subtotal }, tx);
            }

            foreach (var p in v.Pagos)
                await cn.ExecuteAsync(
                    "INSERT INTO venta_pagos (venta_id,metodo,monto) VALUES (@ventaId,@Metodo,@Monto)",
                    new { ventaId, p.Metodo, p.Monto }, tx);

            await tx.CommitAsync();
            return ventaId;
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    private record InfoVenta(string Estado, string JornadaEstado);

    /// Solo se anulan ventas de la jornada ABIERTA (si ya cerró, hay que reabrirla primero:
    /// así nunca cambia el arqueo de un día ya cerrado sin que te des cuenta).
    public async Task AnularAsync(long ventaId, string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo)) throw new NegocioException("El motivo es obligatorio.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            var info = await cn.QuerySingleOrDefaultAsync<InfoVenta>(
                @"SELECT v.estado AS Estado, j.estado AS JornadaEstado
                  FROM ventas v JOIN jornadas_caja j ON j.id=v.jornada_id
                  WHERE v.id=@ventaId FOR UPDATE", new { ventaId }, tx)
                ?? throw new NegocioException("La venta no existe.");

            if (info.Estado == "ANULADA") throw new NegocioException("La venta ya está anulada.");
            if (info.JornadaEstado != "ABIERTA")
                throw new NegocioException("Esa venta pertenece a una caja cerrada. Reabrí esa jornada para anularla.");

            var items = await cn.QueryAsync<(int producto_id, decimal cantidad)>(
                "SELECT producto_id, cantidad FROM venta_detalle WHERE venta_id=@ventaId",
                new { ventaId }, tx);

            foreach (var (productoId, cantidad) in items)
                await StockHelper.AplicarAsync(cn, tx, productoId, "ANULACION_VENTA",
                    cantidad, "VENTA", ventaId, motivo, true);

            await cn.ExecuteAsync(
                @"UPDATE ventas SET estado='ANULADA', fecha_anulacion=@f, motivo_anulacion=@motivo
                  WHERE id=@ventaId",
                new { f = DateTime.Now, motivo = motivo.Trim(), ventaId }, tx);

            await tx.CommitAsync();
        }
        catch { await tx.RollbackAsync(); throw; }
    }

    public async Task<IEnumerable<dynamic>> VentasDeJornadaAsync(long jornadaId)
    {
        await using var cn = await Conexion.AbrirAsync();
        return await cn.QueryAsync(
            "SELECT id, fecha, total, estado FROM ventas WHERE jornada_id=@jornadaId ORDER BY id DESC",
            new { jornadaId });
    }
}
```

### 6.5 `CompraServicio.cs` (proveedor opcional)

```csharp
using Dapper;
using Kiosco.Entidades;

namespace Kiosco.Servicios;

public class CompraServicio
{
    public async Task<long> RegistrarAsync(Compra c)
    {
        if (c.Items.Count == 0) throw new NegocioException("La compra no tiene productos.");
        if (c.Items.Any(i => i.Cantidad <= 0 || i.CostoUnitario < 0))
            throw new NegocioException("Hay cantidades o costos inválidos.");

        await using var cn = await Conexion.AbrirAsync();
        await using var tx = await cn.BeginTransactionAsync();
        try
        {
            long? jornadaId = null;
            if (c.PagadaConCaja)
            {
                jornadaId = await cn.ExecuteScalarAsync<long?>(
                    "SELECT id FROM jornadas_caja WHERE estado='ABIERTA' LIMIT 1 FOR UPDATE", transaction: tx)
                    ?? throw new NegocioException("Para pagar con la caja, primero abrila.");
            }

            var compraId = await cn.ExecuteScalarAsync<long>(
                @"INSERT INTO compras (proveedor_id,nro_comprobante,fecha,total,pagada_con_caja,observaciones)
                  VALUES (@ProveedorId,@NroComprobante,@fecha,@total,@pagada,@Observaciones);
                  SELECT LAST_INSERT_ID();",
                new { c.ProveedorId,                 // null = sin proveedor
                      c.NroComprobante, fecha = DateTime.Now, total = c.Total,
                      pagada = c.PagadaConCaja, c.Observaciones }, tx);

            foreach (var it in c.Items)
            {
                var antes = await StockHelper.AplicarAsync(cn, tx, it.ProductoId, "COMPRA",
                    it.Cantidad, "COMPRA", compraId, null, true);

                await cn.ExecuteAsync(
                    @"INSERT INTO compra_detalle (compra_id,producto_id,cantidad,costo_unitario,subtotal)
                      VALUES (@compraId,@ProductoId,@Cantidad,@CostoUnitario,@sub)",
                    new { compraId, it.ProductoId, it.Cantidad, it.CostoUnitario, sub = it.Subtotal }, tx);

                // Política simple: el costo del producto pasa a ser el último costo de compra
                if (antes.PrecioCosto != it.CostoUnitario)
                {
                    await cn.ExecuteAsync(
                        @"INSERT INTO historial_precios
                          (producto_id,costo_anterior,costo_nuevo,venta_anterior,venta_nuevo,fecha)
                          VALUES (@pid,@ca,@cn,@pv,@pv,@f)",
                        new { pid = it.ProductoId, ca = antes.PrecioCosto, cn = it.CostoUnitario,
                              pv = antes.PrecioVenta, f = DateTime.Now }, tx);
                    await cn.ExecuteAsync(
                        "UPDATE productos SET precio_costo=@c WHERE id=@pid",
                        new { c = it.CostoUnitario, pid = it.ProductoId }, tx);
                }
            }

            if (c.PagadaConCaja)
                await cn.ExecuteAsync(
                    @"INSERT INTO movimientos_caja (jornada_id,tipo,concepto,monto,origen,compra_id,fecha)
                      VALUES (@jornadaId,'EGRESO','Compra de mercadería',@total,'COMPRA',@compraId,@f)",
                    new { jornadaId, total = c.Total, compraId, f = DateTime.Now }, tx);

            await tx.CommitAsync();
            return compraId;
        }
        catch { await tx.RollbackAsync(); throw; }
    }
}
```

**Sobre el costo:** "último costo de compra" es lo más simple. Si el costo cambia seguido y querés ganancias más precisas, en una versión 2 se puede pasar a costo promedio ponderado.

**Corregir una compra cargada mal (v1):** no hay "anular compra". Se corrige con un ajuste de stock con motivo. Si lo querés, se agrega después.

---

## FASE 7. Interfaz (Windows Forms)

### 7.1 Pantallas y orden de desarrollo

| Orden | Pantalla | Contenido |
|---|---|---|
| 1 | `FrmPrincipal` | Botones/menú, estado de caja (ABIERTA/CERRADA con fecha), alerta de productos bajo mínimo |
| 2 | `FrmProductos` | Grilla con búsqueda, alta/edición/desactivar, aumento masivo de precios |
| 3 | `FrmCategorias`, `FrmProveedores` | ABM simples |
| 4 | `FrmCaja` | Abrir, movimientos manuales, cerrar con arqueo, reabrir |
| 5 | `FrmCompras` | Carga de mercadería (proveedor opcional, tilde "pagada con caja") |
| 6 | `FrmAjusteStock` | Elegir producto, stock real contado, motivo |
| 7 | `FrmVenta` | Punto de venta |
| 8 | `FrmHistorialVentas` | Ventas de la jornada y anulación |
| 9 | `FrmReportes` | Genera los PDF |

### 7.2 Flujo al iniciar el programa (en `FrmPrincipal_Load`)

```csharp
var caja = await new CajaServicio().ObtenerAbiertaAsync();

if (caja == null)
{
    lblEstadoCaja.Text = "CAJA CERRADA";
    btnVender.Enabled = false;     // o dejarlo habilitado y ofrecer abrir caja al intentar vender
}
else if (caja.Fecha.Date < DateTime.Today)
{
    MessageBox.Show($"La caja del {caja.Fecha:dd/MM/yyyy} quedó sin cerrar. " +
                    "Hay que cerrarla antes de seguir.", "Caja pendiente");
    new FrmCaja().ShowDialog();    // abre la pantalla de cierre
}
else
{
    lblEstadoCaja.Text = $"CAJA ABIERTA - {caja.Fecha:dd/MM/yyyy}";
}
```

Excepción lógica: si el kiosco trabaja pasada la medianoche, al abrir a la tarde siguiente aparece el aviso solo si realmente quedó abierta de un día anterior al actual, que es lo que querés.

### 7.3 Pantalla de venta (la más importante)

**Distribución:**
- Arriba: campo `txtCodigo` con foco permanente (el lector de barras escribe el código + Enter).
- Centro: `DataGridView` con Producto, Cantidad, Precio, Subtotal.
- Derecha/abajo: **TOTAL grande**, botones Cobrar (F5), Quitar ítem (Supr), Cancelar venta (Esc), Buscar producto (F2).

**Lógica al presionar Enter en el código:**

```csharp
private readonly BindingList<VentaItem> _items = new();
private readonly ProductoServicio _productos = new();

private async void txtCodigo_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode != Keys.Enter) return;
    e.SuppressKeyPress = true;                 // evita el "ding" de Windows

    var texto = txtCodigo.Text.Trim();
    txtCodigo.Clear();
    if (texto == "") return;

    var prod = await _productos.ObtenerPorCodigoAsync(texto);
    if (prod == null)
    {
        MessageBox.Show("Producto no encontrado.");
        return;
    }

    decimal cantidad = 1;
    if (prod.Unidad != "UN")
    {
        // Para KG: pedir el peso (ej. se tipea 250 y se interpreta como gramos)
        cantidad = PedirCantidadPesable(prod);   // devuelve 0 si cancela
        if (cantidad <= 0) return;
    }

    var existente = _items.FirstOrDefault(i => i.ProductoId == prod.Id);
    if (existente != null && prod.Unidad == "UN") existente.Cantidad += cantidad;
    else _items.Add(new VentaItem
    {
        ProductoId = prod.Id, Nombre = prod.Nombre, Unidad = prod.Unidad,
        Cantidad = cantidad, PrecioUnitario = prod.PrecioVenta
    });

    _items.ResetBindings();     // Subtotal es calculado: hay que refrescar la grilla
    ActualizarTotal();
    txtCodigo.Focus();
}
```

**Ventana de cobro:**
1. Muestra el total.
2. Dos campos: "Efectivo recibido" y "Transferencia". Si el dueño escribe solo el efectivo recibido, el resto se asume transferencia (o viceversa).
3. Cálculo:
   - `aplicadoEfectivo = min(efectivoRecibido, total − transferencia)`
   - `vuelto = efectivoRecibido − aplicadoEfectivo`
4. Se arma `Venta.Pagos` con los montos **aplicados** (nunca el recibido) y se llama a `RegistrarAsync`.
5. Si lanza `StockInsuficienteException`, mostrar: *"No hay stock registrado de X (disponible: N). ¿Vender igual?"*. Si acepta, repetir con `permitirSinStock: true`.
6. Si lanza `NegocioException`, mostrar el mensaje y **no limpiar** la pantalla.
7. Deshabilitar el botón mientras procesa (evita doble clic = venta duplicada).

**Buenas prácticas de UI:**
- La UI **nunca** contiene SQL ni cálculos de dinero propios. Todo pasa por los servicios.
- Eventos con `async void` + `await`, así la ventana no se congela.
- Para pesables, aceptar el peso en gramos y convertir (`250` → `0,250 kg`) es más rápido que tipear decimales con coma/punto.
- Atajo útil: si tipean `3*código`, agregar 3 unidades.

---

## FASE 8. Reportes en PDF (QuestPDF)

Reportes sugeridos (todos con el nombre del negocio de la tabla `configuracion` en el encabezado):

1. **Cierre de caja del día** (el más importante).
2. **Ventas por rango de fechas** (con totales por día).
3. **Lista de stock actual** y **faltantes** (stock ≤ mínimo).
4. **Productos más vendidos** (rango de fechas).

Ejemplo del cierre de caja:

```csharp
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Kiosco.Entidades;

public static class PdfCierreCaja
{
    public static void Generar(string ruta, string negocio, JornadaCaja j, ResumenJornada r)
    {
        Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(t => t.FontSize(11));

                page.Header().Column(c =>
                {
                    c.Item().Text(negocio).FontSize(20).Bold();
                    c.Item().Text($"Cierre de caja - {j.Fecha:dd/MM/yyyy}").FontSize(14);
                });

                page.Content().PaddingVertical(15).Table(t =>
                {
                    t.ColumnsDefinition(c => { c.RelativeColumn(3); c.RelativeColumn(1); });

                    void Fila(string concepto, decimal monto, bool negrita = false)
                    {
                        var a = t.Cell().PaddingVertical(3).Text(concepto);
                        var b = t.Cell().PaddingVertical(3).AlignRight().Text(monto.ToString("C2"));
                        if (negrita) { a.Bold(); b.Bold(); }
                    }

                    Fila("Efectivo inicial", r.MontoApertura);
                    Fila("Ventas en efectivo", r.VentasEfectivo);
                    Fila("Ingresos manuales", r.Ingresos);
                    Fila("Egresos (retiros, compras, gastos)", -r.Egresos);
                    Fila("Efectivo esperado", r.EfectivoEsperado, true);
                    Fila("Efectivo contado", j.EfectivoContado ?? 0, true);
                    Fila("Diferencia", j.Diferencia ?? 0, true);

                    t.Cell().ColumnSpan(2).PaddingTop(15).Text("Resumen del día").Bold();
                    Fila("Ventas por transferencia", r.VentasTransferencia);
                    Fila("Total vendido", r.TotalVendido, true);
                    Fila("Ganancia bruta estimada", r.GananciaBruta);
                    t.Cell().ColumnSpan(2).Text(
                        $"Ventas: {r.CantidadVentas}  |  Anuladas: {r.CantidadAnuladas}");
                });

                page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9);
            });
        }).GeneratePdf(ruta);
    }
}
```

Usá `SaveFileDialog` para elegir dónde guardar y `Process.Start(new ProcessStartInfo(ruta){ UseShellExecute = true })` para abrirlo al terminar.

Los demás reportes siguen el mismo patrón (una consulta SQL en un servicio + un método que arma el PDF).

---

## FASE 9. Pruebas (hacerlas con datos inventados antes de usar en serio)

1. **Venta normal:** vender 3 productos, verificar en Workbench que `stock_actual` bajó, que hay filas en `movimientos_stock`, `venta_detalle` y `venta_pagos`.
2. **Rollback:** forzar un error a mitad de la venta (por ejemplo, un `throw` temporal después del primer ítem) y comprobar que **no quedó ninguna fila** a medias.
3. **Pesables:** vender 0,250 kg a $12.000/kg y comprobar $3.000,00 exactos. Probar también 0,333 kg para ver el redondeo.
4. **Pagos mixtos:** vender $1.800 con $1.000 en transferencia + $1.000 en efectivo (vuelto $200). Verificar que se guardó $800 en efectivo.
5. **Compra sin proveedor** y **con proveedor**; con y sin el tilde "pagada con caja" (verificar el egreso).
6. **Ciclo de caja completo:** abrir con $5.000, vender en efectivo y transferencia, hacer un retiro, anular una venta, cerrar. Calcular a mano el esperado y compararlo con el del sistema.
7. **Cierre olvidado:** dejar la jornada abierta, cambiar la fecha de la PC al día siguiente y abrir el programa; debe obligar a cerrar.
8. **Reabrir** una jornada cerrada y volver a cerrarla.
9. **Vender sin stock:** debe preguntar y, si acepta, dejar el stock negativo.
10. **Prueba con volumen:** cargar 5.000 productos y 50.000 ventas de prueba con un script, para verificar que las búsquedas siguen siendo rápidas.

---

## FASE 10. Backup e instalación (crítico: una sola PC = un solo punto de falla)

1. **Backup automático diario** con `mysqldump`. Crear un archivo `backup.bat`:

```bat
@echo off
set FECHA=%date:~-4%%date:~3,2%%date:~0,2%
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe" -u root -p"TU_CLAVE" --single-transaction kiosco > "D:\Backups\kiosco_%FECHA%.sql"
```

   - Programarlo en el **Programador de tareas de Windows** (diario, a una hora en que el kiosco esté cerrado).
   - Guardar los respaldos en **otro disco o pendrive**, y de ser posible también en la nube (Google Drive/OneDrive sincronizando la carpeta).
   - **Probá restaurar** al menos una vez en otra PC (`mysql -u root -p kiosco < archivo.sql`). Un backup sin probar no es un backup.
   - El formato de `%date%` depende de la configuración regional de Windows; revisá que el nombre del archivo salga bien.
2. **Publicar la aplicación:** clic derecho en `Kiosco.UI` → Publicar → carpeta, "Autocontenido" para no depender de que .NET esté instalado. Copiar la carpeta a la PC del negocio y crear un acceso directo en el escritorio.
3. Agregar un botón **"Hacer backup ahora"** en la pantalla principal (ejecuta el `.bat` o el `mysqldump` desde C#) para tener un respaldo manual antes de cambios grandes, como un aumento masivo de precios.
4. Un **UPS/estabilizador** en la PC evita bases corruptas por cortes de luz, algo común en un local.
5. Si algún día se quiere acceso al sistema por otra persona, recién ahí se agrega login. La estructura queda lista para sumarlo.

---

## Hoja de ruta (orden real de trabajo)

| Etapa | Qué hacer | Está listo cuando... |
|---|---|---|
| 1 | Fases 1-2 | La base existe y probaste INSERT/SELECT a mano en Workbench |
| 2 | Fases 3-5 | Una app de consola/Form vacío conecta y lista productos |
| 3 | `ProductoServicio` + `FrmProductos` + categorías/proveedores | Podés crear y editar productos con stock inicial |
| 4 | `CajaServicio` + `FrmCaja` | Abrir/cerrar da el arqueo correcto con datos de prueba |
| 5 | `CompraServicio` + `FrmCompras` + `FrmAjusteStock` | El stock sube y queda historial |
| 6 | `VentaServicio` + `FrmVenta` | Una venta completa descuenta stock y suma a la caja |
| 7 | Anulaciones + historial de ventas | Anular devuelve stock y corrige el arqueo |
| 8 | PDF | El cierre de caja coincide con lo calculado a mano |
| 9 | Fases 9-10 | Backup funcionando y sistema instalado en el negocio |

**Para la versión 2 (ya dejado fuera):** facturación electrónica (ARCA), clientes y cuenta corriente, anulación de compras, costo promedio, venta rápida de "ítem libre" sin producto, y login si se suma otro usuario.

---

¿Seguimos con la primera etapa? Puedo armarte el paso a paso de la **Etapa 3** con el formulario `FrmProductos` completo (diseño de controles, eventos y código), o la pantalla de **caja y arqueo**. Y si preferís tener esta guía como documento de trabajo, decime y te la dejo en un archivo.