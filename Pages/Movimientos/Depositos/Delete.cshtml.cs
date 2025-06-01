using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Movimientos.Depositos
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Deposito Deposito { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposito = await _context.Depositos
                .Include(d => d.FondoMonetario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (deposito == null)
            {
                return NotFound();
            }

            Deposito = deposito;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposito = await _context.Depositos.FindAsync(id);
            if (deposito != null)
            {
                _context.Depositos.Remove(deposito);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
} 