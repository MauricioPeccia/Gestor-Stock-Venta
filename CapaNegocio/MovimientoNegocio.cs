using System;
using KioscoLite.CapaDatos;
using KioscoLite.CapaNegocio.Modelos;

namespace KioscoLite.CapaNegocio
{
    /// <summary>
    /// Servicio de negocio para la validación y registro de Movimientos de Caja.
    /// </summary>
    public class MovimientoNegocio
    {
        private readonly MovimientoDatos _movimientoDatos;

        public MovimientoNegocio()
        {
            _movimientoDatos = new MovimientoDatos();
        }

        public bool RegistrarMovimiento(MovimientoCaja movimiento)
        {
            if (movimiento == null)
                throw new ArgumentNullException(nameof(movimiento));

            if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento))
                throw new InvalidOperationException("Debe seleccionar un tipo de movimiento.");

            if (string.IsNullOrWhiteSpace(movimiento.Concepto))
                throw new InvalidOperationException("El concepto o detalle del movimiento es obligatorio.");

            if (movimiento.Monto <= 0)
                throw new InvalidOperationException("El monto debe ser mayor a cero.");

            return _movimientoDatos.RegistrarMovimiento(movimiento);
        }
    }
}
