using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Movimientos.Gastos
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Gasto Gasto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Gasto = await _context.Gastos
                .Include(g => g.FondoMonetario)
                .Include(g => g.Detalles)
                .ThenInclude(d => d.TipoGasto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Gasto == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 