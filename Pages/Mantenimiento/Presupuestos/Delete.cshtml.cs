using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models.ViewModels;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Mantenimiento.Presupuestos
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
        public PresupuestoViewModel PresupuestoVM { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos
                .Include(p => p.TipoGasto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (presupuesto == null || presupuesto.UsuarioId != userId)
            {
                return NotFound();
            }

            PresupuestoVM = PresupuestoViewModel.FromPresupuesto(presupuesto);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var presupuesto = await _context.Presupuestos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (presupuesto == null || presupuesto.UsuarioId != userId)
            {
                return NotFound();
            }

            _context.Presupuestos.Remove(presupuesto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
} 