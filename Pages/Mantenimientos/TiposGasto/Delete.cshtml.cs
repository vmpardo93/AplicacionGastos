using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Mantenimientos.TiposGasto
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TipoGasto TipoGasto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoGasto = await _context.TiposGasto.FirstOrDefaultAsync(m => m.Id == id);

            if (TipoGasto == null)
            {
                return NotFound();
            }

            // Verificar si el tipo de gasto está siendo utilizado
            var tieneGastos = await _context.GastosDetalle.AnyAsync(g => g.TipoGastoId == id);
            if (tieneGastos)
            {
                TempData["ErrorMessage"] = "No se puede eliminar el tipo de gasto porque está siendo utilizado en registros de gastos.";
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoGasto = await _context.TiposGasto.FindAsync(id);

            if (TipoGasto != null)
            {
                // Verificar nuevamente si el tipo de gasto está siendo utilizado
                var tieneGastos = await _context.GastosDetalle.AnyAsync(g => g.TipoGastoId == id);
                if (tieneGastos)
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el tipo de gasto porque está siendo utilizado en registros de gastos.";
                    return RedirectToPage("./Index");
                }

                _context.TiposGasto.Remove(TipoGasto);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
} 