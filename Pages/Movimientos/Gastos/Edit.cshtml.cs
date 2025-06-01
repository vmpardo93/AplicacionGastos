using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using MovimientoGastos.Services;

namespace MovimientoGastos.Pages.Movimientos.Gastos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPresupuestoService _presupuestoService;

        public EditModel(ApplicationDbContext context, IPresupuestoService presupuestoService)
        {
            _context = context;
            _presupuestoService = presupuestoService;
        }

        [BindProperty]
        public Gasto Gasto { get; set; }
        public IList<FondoMonetario> FondosLista { get; set; }
        public IList<TipoGasto> TiposGastoLista { get; set; }

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

            await CargarListas();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListas();
                return Page();
            }

            // Validar presupuesto para cada detalle
            var userId = User.Identity.Name;
            foreach (var detalle in Gasto.Detalles)
            {
                var validacion = await _presupuestoService.ValidarPresupuestoAsync(
                    userId,
                    detalle.TipoGastoId,
                    detalle.Monto,
                    Gasto.Fecha
                );

                if (validacion.excedido)
                {
                    ModelState.AddModelError("", $"El gasto de tipo {detalle.TipoGasto?.Nombre} excede el presupuesto mensual. " +
                        $"Presupuestado: {validacion.montoPresupuestado:C}, Gastado: {validacion.montoGastado:C}");
                    await CargarListas();
                    return Page();
                }
            }

            _context.Attach(Gasto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GastoExists(Gasto.Id))
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

        private async Task CargarListas()
        {
            FondosLista = await _context.FondosMonetarios.ToListAsync();
            TiposGastoLista = await _context.TiposGasto.ToListAsync();
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }
    }
} 