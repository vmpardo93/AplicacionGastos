using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Consultas
{
    [Authorize]
    public class GraficosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GraficosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaInicio { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaFin { get; set; }

        public List<string> TiposGasto { get; set; } = new List<string>();
        public List<decimal> MontosPresupuestados { get; set; } = new List<decimal>();
        public List<decimal> MontosEjecutados { get; set; } = new List<decimal>();

        public async Task OnGetAsync()
        {
            // Obtener el ID del usuario actual
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return; // Si no hay usuario, no mostrar nada
            }

            // Si no se proporcionan fechas, usar el mes actual
            if (!FechaInicio.HasValue)
            {
                FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (!FechaFin.HasValue)
            {
                FechaFin = FechaInicio.Value.AddMonths(1).AddDays(-1);
            }

            // Obtener todos los tipos de gasto que tienen presupuesto o gastos para el usuario
            var tiposGastoConDatos = await _context.TiposGasto
                .Where(t => _context.Presupuestos.Any(p => p.TipoGastoId == t.Id && p.UsuarioId == userId) ||
                           _context.GastosDetalle.Any(gd => gd.TipoGastoId == t.Id && 
                                                          gd.Gasto.UsuarioId == userId &&
                                                          gd.Gasto.Fecha >= FechaInicio && 
                                                          gd.Gasto.Fecha <= FechaFin))
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            foreach (var tipo in tiposGastoConDatos)
            {
                TiposGasto.Add(tipo.Nombre);

                // Calcular el presupuesto total para el rango de fechas
                decimal presupuestoTotal = 0;
                
                // Obtener todos los meses en el rango de fechas
                var fechaActual = new DateTime(FechaInicio.Value.Year, FechaInicio.Value.Month, 1);
                var fechaFinMes = new DateTime(FechaFin.Value.Year, FechaFin.Value.Month, 1);

                while (fechaActual <= fechaFinMes)
                {
                    var presupuestoMes = await _context.Presupuestos
                        .Where(p => p.UsuarioId == userId &&
                               p.TipoGastoId == tipo.Id &&
                               p.Mes == fechaActual.Month)
                        .FirstOrDefaultAsync();

                    if (presupuestoMes != null)
                    {
                        // Si el rango no cubre todo el mes, calcular proporcionalmente
                        var diasEnMes = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                        var fechaInicioMes = new DateTime(fechaActual.Year, fechaActual.Month, 1);
                        var fechaFinMes2 = fechaInicioMes.AddMonths(1).AddDays(-1);

                        var fechaInicioEfectiva = fechaActual == fechaInicioMes ? 
                            (FechaInicio.Value > fechaInicioMes ? FechaInicio.Value : fechaInicioMes) : fechaInicioMes;
                        var fechaFinEfectiva = fechaActual == new DateTime(FechaFin.Value.Year, FechaFin.Value.Month, 1) ? 
                            (FechaFin.Value < fechaFinMes2 ? FechaFin.Value : fechaFinMes2) : fechaFinMes2;

                        var diasEfectivos = (fechaFinEfectiva - fechaInicioEfectiva).Days + 1;
                        var proporcion = (decimal)diasEfectivos / diasEnMes;

                        presupuestoTotal += presupuestoMes.Monto * proporcion;
                    }

                    fechaActual = fechaActual.AddMonths(1);
                }

                // Obtener el total ejecutado para el tipo de gasto en el rango de fechas
                var totalEjecutado = await _context.GastosDetalle
                    .Include(gd => gd.Gasto)
                    .Where(gd => gd.TipoGastoId == tipo.Id &&
                           gd.Gasto.UsuarioId == userId && // ¡IMPORTANTE! Filtrar por usuario
                           gd.Gasto.Fecha >= FechaInicio &&
                           gd.Gasto.Fecha <= FechaFin)
                    .SumAsync(gd => gd.Monto);

                MontosPresupuestados.Add(presupuestoTotal);
                MontosEjecutados.Add(totalEjecutado);
            }

            // Si no hay datos, mostrar un mensaje apropiado
            if (!TiposGasto.Any())
            {
                TiposGasto.Add("Sin datos");
                MontosPresupuestados.Add(0);
                MontosEjecutados.Add(0);
            }
        }
    }
} 