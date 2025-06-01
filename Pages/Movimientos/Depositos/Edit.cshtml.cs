using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Movimientos.Depositos
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Deposito Deposito { get; set; } = default!;
        public IList<FondoMonetario> FondosMonetarios { get; set; } = default!;

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
            FondosMonetarios = await _context.FondosMonetarios.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                FondosMonetarios = await _context.FondosMonetarios.ToListAsync();
                return Page();
            }

            _context.Attach(Deposito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DepositoExists(Deposito.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> DepositoExists(int id)
        {
            return await _context.Depositos.AnyAsync(e => e.Id == id);
        }
    }
} 