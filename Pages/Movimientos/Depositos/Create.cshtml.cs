using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using MovimientoGastos.Models.ViewModels;

namespace MovimientoGastos.Pages.Movimientos.Depositos
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
            DepositoVM = new DepositoViewModel();
        }

        [BindProperty]
        public DepositoViewModel DepositoVM { get; set; }
        public SelectList FondosMonetarios { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await CargarFondos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarFondos();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validar que el FondoMonetarioId sea válido
            var fondoExists = await _context.FondosMonetarios.AnyAsync(f => f.Id == DepositoVM.FondoMonetarioId);
            if (!fondoExists)
            {
                ModelState.AddModelError("DepositoVM.FondoMonetarioId", "El fondo seleccionado no es válido.");
                return Page();
            }

            var deposito = DepositoVM.ToDeposito();
            _context.Depositos.Add(deposito);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task CargarFondos()
        {
            var fondos = await _context.FondosMonetarios
                .OrderBy(f => f.Nombre)
                .Select(f => new { f.Id, f.Nombre })
                .ToListAsync();

            FondosMonetarios = new SelectList(fondos, "Id", "Nombre");
        }
    }
} 