using System;
using System.Collections.Generic;
using KioscoLite.CapaDatos;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaNegocio
{
    /// <summary>
    /// Servicio de negocio encargado de ejecutar las Pruebas de Integración extremo a extremo (E2E)
    /// del ciclo de vida de un producto en Kiosco Lite (Inventario -> Movimientos/Compras -> Caja/Ventas -> Reportes).
    /// </summary>
    public class PruebaIntegracionNegocio
    {
        private readonly ProductoDatos _productoDatos;
        private readonly MovimientoDatos _movimientoDatos;
        private readonly VentaDatos _ventaDatos;

        public PruebaIntegracionNegocio()
        {
            _productoDatos = new ProductoDatos();
            _movimientoDatos = new MovimientoDatos();
            _ventaDatos = new VentaDatos();
        }

        /// <summary>
        /// Ejecuta secuencialmente el ciclo de vida completo de un producto de prueba interactuando con los DAOs
        /// y validando la integridad transaccional de stock y saldos de caja en kiosco_db.
        /// </summary>
        /// <returns>Lista de cadenas con la bitácora (log) detallada de resultados de cada paso.</returns>
        public List<string> EjecutarCicloCompletoProducto()
        {
            var log = new List<string>();
            string marcaTiempo = DateTime.Now.ToString("HH:mm:ss");

            log.Add($"[{marcaTiempo}] === INICIO DE PRUEBA DE INTEGRACIÓN: CICLO DE VIDA DE PRODUCTO ===");

            try
            {
                // =========================================================================
                // PASO A: Insertar 'Producto Test' en el inventario con Stock Inicial = 0
                // =========================================================================
                string sufijoUnico = DateTime.Now.ToString("ddHHmmssfff");
                const decimal costoUnitarioTest = 100.00m;
                const decimal precioVentaTest = 150.00m;

                var productoTest = new Producto
                {
                    CodigoBarras = $"779{sufijoUnico}",
                    CodigoInterno = $"TST-{sufijoUnico.Substring(Math.Max(0, sufijoUnico.Length - 8))}",
                    Nombre = $"Producto Test ({DateTime.Now:HH:mm:ss})",
                    PrecioCosto = costoUnitarioTest,
                    PrecioVenta = precioVentaTest,
                    StockActual = 0m,
                    StockMinimo = 5m,
                    EsFraccionado = false,
                    Activo = true
                };

                int idProductoCreado = _productoDatos.InsertarProducto(productoTest);
                Producto? prodVerificadoA = _productoDatos.ObtenerProductoPorId(idProductoCreado);

                if (idProductoCreado <= 0 || prodVerificadoA == null)
                {
                    log.Add("ERROR [Paso A]: No se pudo insertar el producto de prueba en la base de datos.");
                    return log;
                }

                if (prodVerificadoA.StockActual != 0m)
                {
                    log.Add($"ERROR [Paso A]: El stock inicial esperado era 0, pero se registró {prodVerificadoA.StockActual:0.###}.");
                    return log;
                }

                log.Add($"OK: Producto creado -> ID #{idProductoCreado} | '{prodVerificadoA.Nombre}' | Stock Inicial: {prodVerificadoA.StockActual:0.##} u. | Costo: $ {costoUnitarioTest:N2} | Venta: $ {precioVentaTest:N2}");

                // =========================================================================
                // PASO B: Registrar 'Compra de Mercadería' de 50 unidades (Transaccional)
                //         Validar aumento de stock a 50 y descuento exacto en caja diaria
                // =========================================================================
                const decimal cantidadCompra = 50m;
                decimal montoCompraEsperado = Math.Round(cantidadCompra * costoUnitarioTest, 2); // $ 5,000.00
                decimal egresosCajaAntes = _movimientoDatos.ObtenerTotalEgresosCajaHoy();

                var movimientoCompra = new MovimientoCaja
                {
                    TipoMovimiento = "Compra Mercaderia",
                    Concepto = $"Compra Test Integración - 50 u. de {prodVerificadoA.Nombre}",
                    Monto = montoCompraEsperado,
                    Fecha = DateTime.Now,
                    IdProductoCompra = idProductoCreado,
                    CantidadCompra = cantidadCompra,
                    PrecioCostoUnitario = costoUnitarioTest,
                    Proveedor = "Proveedor Test Integración"
                };

                bool compraExitosa = _movimientoDatos.RegistrarMovimiento(movimientoCompra);
                Producto? prodVerificadoB = _productoDatos.ObtenerProductoPorId(idProductoCreado);
                decimal egresosCajaDespues = _movimientoDatos.ObtenerTotalEgresosCajaHoy();
                decimal diferenciaEgresoCaja = egresosCajaDespues - egresosCajaAntes;

                if (!compraExitosa || prodVerificadoB == null)
                {
                    log.Add("ERROR [Paso B]: Falló la transacción de Compra de Mercadería (se ejecutó Rollback).");
                    return log;
                }

                if (prodVerificadoB.StockActual != 50m)
                {
                    log.Add($"ERROR [Paso B]: Inconsistencia de Stock tras compra. Esperado: 50 | Actual: {prodVerificadoB.StockActual:0.###}.");
                    return log;
                }

                if (diferenciaEgresoCaja != montoCompraEsperado)
                {
                    log.Add($"ERROR [Paso B]: El egreso en caja diaria ($ {diferenciaEgresoCaja:N2}) no coincide con el monto de compra ($ {montoCompraEsperado:N2}).");
                    return log;
                }

                log.Add($"OK: Compra de Mercadería registrada (+{cantidadCompra:0.##} u.) -> Stock oficial actualizado a {prodVerificadoB.StockActual:0.##} u.");
                log.Add($"OK: Egreso de Caja validado -> Se restaron exactamente $ {diferenciaEgresoCaja:N2} de la caja diaria (Egresos hoy: $ {egresosCajaDespues:N2}).");

                // =========================================================================
                // PASO C: Simular 'Venta en Caja' de 5 unidades (Transaccional)
                //         Validar reducción de stock a 45 y suma del ingreso a subtotales diarios
                // =========================================================================
                const decimal cantidadVenta = 5m;
                decimal subtotalVentaEsperado = Math.Round(cantidadVenta * precioVentaTest, 2); // $ 750.00
                decimal subtotalDiarioAntes = _ventaDatos.ObtenerSubtotalVentasHoy();

                var ventaSimulada = new Venta
                {
                    Fecha = DateTime.Now,
                    MedioPago = "EFECTIVO",
                    Total = subtotalVentaEsperado,
                    RequiereTicket = false,
                    Observaciones = "Venta automática generada por Prueba de Integración",
                    Detalles = new List<DetalleVenta>
                    {
                        new DetalleVenta
                        {
                            IdProducto = idProductoCreado,
                            Codigo = prodVerificadoB.CodigoInterno,
                            Descripcion = prodVerificadoB.Nombre,
                            Cantidad = cantidadVenta,
                            PrecioCostoHistorico = prodVerificadoB.PrecioCosto,
                            PrecioUnitario = prodVerificadoB.PrecioVenta,
                            Subtotal = subtotalVentaEsperado
                        }
                    }
                };

                int idVentaGenerada = _ventaDatos.InsertarVenta(ventaSimulada);
                Producto? prodVerificadoC = _productoDatos.ObtenerProductoPorId(idProductoCreado);
                decimal subtotalDiarioDespues = _ventaDatos.ObtenerSubtotalVentasHoy();
                decimal incrementoSubtotalDiario = subtotalDiarioDespues - subtotalDiarioAntes;

                if (idVentaGenerada <= 0 || prodVerificadoC == null)
                {
                    log.Add("ERROR [Paso C]: Falló la transacción de Venta en Caja (se ejecutó Rollback).");
                    return log;
                }

                if (prodVerificadoC.StockActual != 45m)
                {
                    log.Add($"ERROR [Paso C]: Inconsistencia de Stock tras venta. Esperado: 45 | Actual: {prodVerificadoC.StockActual:0.###}.");
                    return log;
                }

                if (incrementoSubtotalDiario != subtotalVentaEsperado)
                {
                    log.Add($"ERROR [Paso C]: El incremento en el subtotal diario ($ {incrementoSubtotalDiario:N2}) no coincide con la venta ($ {subtotalVentaEsperado:N2}).");
                    return log;
                }

                log.Add($"OK: Venta registrada (Ticket #{idVentaGenerada} por {cantidadVenta:0.##} u.) -> Stock oficial reducido a {prodVerificadoC.StockActual:0.##} u.");
                log.Add($"OK: Ingreso de Caja validado -> +$ {incrementoSubtotalDiario:N2} sumados al subtotal diario de ventas (Subtotal hoy: $ {subtotalDiarioDespues:N2}).");

                // Limpieza lógica del producto de prueba para no contaminar el catálogo operativo del cajero
                _productoDatos.EliminarProducto(idProductoCreado);
                log.Add($"OK: Limpieza post-prueba -> 'Producto Test' (ID #{idProductoCreado}) desactivado del catálogo operativo preservando historial contable.");

                log.Add($"[{DateTime.Now:HH:mm:ss}] === RESULTADO FINAL: PRUEBA DE INTEGRACIÓN SUPERADA CON ÉXITO (100% OK) ===");
            }
            catch (Exception ex)
            {
                log.Add($"EXCEPCIÓN CRÍTICA (Transacción revertida con Rollback): {ex.Message}");
                log.Add($"[{DateTime.Now:HH:mm:ss}] === RESULTADO FINAL: PRUEBA INTERRUMPIDA POR ERROR ===");
            }

            return log;
        }
    }
}
