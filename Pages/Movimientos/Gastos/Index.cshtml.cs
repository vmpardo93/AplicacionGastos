using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Movimientos.Gastos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Gasto> Gastos { get; set; }

        public async Task OnGetAsync()
        {
            Gastos = await _context.Gastos
                .Include(g => g.FondoMonetario)
                .Include(g => g.Detalles)
                .OrderByDescending(g => g.Fecha)
                .ToListAsync();
        }
    }
} 