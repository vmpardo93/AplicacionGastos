using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using MovimientoGastos.Models.ViewModels;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Mantenimiento.Presupuestos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PresupuestoViewModel> Presupuestos { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // Si no hay usuario identificado, retornar lista vacía
                Presupuestos = new List<PresupuestoViewModel>();
                return;
            }

            var presupuestos = await _context.Presupuestos
                .Include(p => p.TipoGasto)
                .Where(p => p.UsuarioId == userId)
                .OrderBy(p => p.Mes)
                .ThenBy(p => p.TipoGasto.Nombre)
                .ToListAsync();

            Presupuestos = presupuestos.Select(p => new PresupuestoViewModel
            {
                Id = p.Id,
                TipoGastoId = p.TipoGastoId,
                Mes = p.Mes,
                Monto = p.Monto,
                TipoGastoNombre = p.TipoGasto.Nombre
            }).ToList();
        }
    }
} 