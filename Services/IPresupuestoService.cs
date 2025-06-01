using MovimientoGastos.Models;

namespace MovimientoGastos.Services
{
    public interface IPresupuestoService
    {
        Task<(bool excedido, decimal montoPresupuestado, decimal montoGastado)> ValidarPresupuestoAsync(
            string usuarioId,
            int tipoGastoId,
            decimal montoNuevo,
            DateTime fecha);
    }
} 