using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KioscoLite.CapaDatos;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaNegocio
{
    /// <summary>
    /// Servicio de negocio para la terminal de Caja (Ventas).
    /// </summary>
    public class VentaNegocio
    {
        private readonly VentaDatos _ventaDatos;

        public VentaNegocio()
        {
            _ventaDatos = new VentaDatos();
        }

        public List<Producto> ObtenerCatalogoEnMemoria()
        {
            return _ventaDatos.ObtenerTodosLosProductos();
        }

        public int RegistrarVentaCompleta(Venta venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta), "La venta no puede ser nula.");

            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new InvalidOperationException("No se pueden registrar ventas sin productos.");

            foreach (var det in venta.Detalles)
            {
                det.Subtotal = Math.Round(det.Cantidad * det.PrecioUnitario, 2);
            }

            venta.Total = venta.Detalles.Sum(d => d.Subtotal);

            if (venta.Total <= 0)
                throw new InvalidOperationException("El monto total de la venta debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(venta.MedioPago))
                venta.MedioPago = "EFECTIVO";

            return _ventaDatos.InsertarVenta(venta);
        }

        public decimal CalcularCantidadPorMonto(decimal precioKilo, decimal montoDinero)
        {
            if (precioKilo <= 0) return 0;
            return Math.Round(montoDinero / precioKilo, 3);
        }

        public void GenerarTicketPDF(Venta venta, int idVenta)
        {
            try
            {
                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tickets");
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string archivoTicket = Path.Combine(carpeta, $"Ticket_{idVenta}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

                using (var writer = new StreamWriter(archivoTicket))
                {
                    writer.WriteLine("========================================");
                    writer.WriteLine("              KIOSCO LITE               ");
                    writer.WriteLine("       TERMINAL DE VENTAS - CAJA        ");
                    writer.WriteLine("========================================");
                    writer.WriteLine($"Ticket Nro: #{idVenta}");
                    writer.WriteLine($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm:ss}");
                    writer.WriteLine($"Medio de Pago: {venta.MedioPago}");
                    writer.WriteLine("----------------------------------------");
                    writer.WriteLine("CANT   DESCRIPCION            SUBTOTAL  ");
                    writer.WriteLine("----------------------------------------");

                    foreach (var item in venta.Detalles)
                    {
                        string cant = item.EsFraccionado ? item.Cantidad.ToString("0.000") : item.Cantidad.ToString("0.##");
                        string desc = item.Descripcion.Length > 20
                            ? item.Descripcion.Substring(0, 17) + "..."
                            : item.Descripcion.PadRight(20);
                        writer.WriteLine($"{cant.PadRight(6)} {desc} $ {item.Subtotal:N2}");
                    }

                    writer.WriteLine("----------------------------------------");
                    writer.WriteLine($"TOTAL A PAGAR: $ {venta.Total:N2}");
                    writer.WriteLine("========================================");
                    writer.WriteLine("         ¡GRACIAS POR SU COMPRA!        ");
                    writer.WriteLine("========================================");
                }
            }
            catch
            {
                // No interrumpir el flujo de caja si el sistema de archivos bloquea la escritura del ticket
            }
        }
    }
}
