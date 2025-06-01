using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Services
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly ApplicationDbContext _context;

        public PresupuestoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool excedido, decimal montoPresupuestado, decimal montoGastado)> ValidarPresupuestoAsync(
            string usuarioId,
            int tipoGastoId,
            decimal montoNuevo,
            DateTime fecha)
        {
            // Obtener el presupuesto del mes para el tipo de gasto
            var presupuesto = await _context.Presupuestos
                .Where(p => p.UsuarioId == usuarioId &&
                           p.TipoGastoId == tipoGastoId &&
                           p.Mes == fecha.Month)
                .FirstOrDefaultAsync();

            if (presupuesto == null)
            {
                // Si no hay presupuesto definido, consideramos que no hay límite
                return (false, 0, 0);
            }

            // Calcular el total gastado en el mes para ese tipo de gasto
            var totalGastado = await _context.GastosDetalle
                .Include(gd => gd.Gasto)
                .Where(gd => gd.TipoGastoId == tipoGastoId &&
                            gd.Gasto.Fecha.Month == fecha.Month &&
                            gd.Gasto.Fecha.Year == fecha.Year)
                .SumAsync(gd => gd.Monto);

            // Sumar el nuevo monto al total gastado
            var totalConNuevoGasto = totalGastado + montoNuevo;

            // Verificar si excede el presupuesto
            return (totalConNuevoGasto > presupuesto.Monto, presupuesto.Monto, totalGastado);
        }
    }
} 