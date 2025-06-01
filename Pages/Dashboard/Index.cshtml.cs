using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public decimal TotalGastosDelMes { get; set; }
        public decimal TotalDepositosDelMes { get; set; }
        public decimal Balance { get; set; }
        public int TotalPresupuestos { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            NombreUsuario = User.Identity?.Name ?? "Usuario";

            if (!string.IsNullOrEmpty(userId))
            {
                var fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                try
                {
                    // Calcular total de gastos del mes actual
                    TotalGastosDelMes = await _context.GastosDetalle
                        .Include(gd => gd.Gasto)
                        .Where(gd => gd.Gasto.UsuarioId == userId &&
                               gd.Gasto.Fecha >= fechaInicio &&
                               gd.Gasto.Fecha <= fechaFin)
                        .SumAsync(gd => gd.Monto);

                    // Calcular total de depósitos del mes actual
                    TotalDepositosDelMes = await _context.Depositos
                        .Where(d => d.Fecha >= fechaInicio && d.Fecha <= fechaFin)
                        .SumAsync(d => d.Monto);

                    // Calcular balance
                    Balance = TotalDepositosDelMes - TotalGastosDelMes;

                    // Contar presupuestos del mes actual
                    TotalPresupuestos = await _context.Presupuestos
                        .Where(p => p.UsuarioId == userId && p.Mes == DateTime.Now.Month)
                        .CountAsync();
                }
                catch (Exception)
                {
                    // En caso de error, inicializar con valores por defecto
                    TotalGastosDelMes = 0;
                    TotalDepositosDelMes = 0;
                    Balance = 0;
                    TotalPresupuestos = 0;
                }
            }
        }
    }
} 