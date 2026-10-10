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

