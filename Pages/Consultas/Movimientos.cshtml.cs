using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Consultas
{
    [Authorize]
    public class MovimientosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MovimientosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaInicio { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaFin { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FondoId { get; set; }

        public IList<Gasto> Gastos { get; set; } = new List<Gasto>();
        public IList<Deposito> Depositos { get; set; } = new List<Deposito>();
        public IList<FondoMonetario> Fondos { get; set; } = new List<FondoMonetario>();
        public decimal TotalGastos { get; set; }
        public decimal TotalDepositos { get; set; }
        public decimal Balance => TotalDepositos - TotalGastos;

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

            // Cargar los fondos para el filtro
            Fondos = await _context.FondosMonetarios
                .OrderBy(f => f.Nombre)
                .ToListAsync();

            // Consulta base para gastos (INCLUIR DETALLES Y FILTRAR POR USUARIO)
            var gastosQuery = _context.Gastos
                .Include(g => g.FondoMonetario)
                .Include(g => g.Detalles) // ¡IMPORTANTE! Incluir detalles para calcular Total
                .Where(g => g.UsuarioId == userId) // ¡IMPORTANTE! Filtrar por usuario
                .Where(g => g.Fecha >= FechaInicio && g.Fecha <= FechaFin);

            // Consulta base para depósitos
            var depositosQuery = _context.Depositos
                .Include(d => d.FondoMonetario)
                .Where(d => d.Fecha >= FechaInicio && d.Fecha <= FechaFin);

            // Aplicar filtro por fondo si se seleccionó uno
            if (FondoId.HasValue)
            {
                gastosQuery = gastosQuery.Where(g => g.FondoMonetarioId == FondoId);
                depositosQuery = depositosQuery.Where(d => d.FondoMonetarioId == FondoId);
            }

            // Ejecutar las consultas
            Gastos = await gastosQuery
                .OrderByDescending(g => g.Fecha)
                .ThenByDescending(g => g.Id)
                .ToListAsync();

            Depositos = await depositosQuery
                .OrderByDescending(d => d.Fecha)
                .ThenByDescending(d => d.Id)
                .ToListAsync();

            // Calcular totales
            TotalGastos = Gastos.Sum(g => g.Total);
            TotalDepositos = Depositos.Sum(d => d.Monto);
        }
    }
} 