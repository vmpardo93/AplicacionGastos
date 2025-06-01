using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using MovimientoGastos.Models.ViewModels;
using MovimientoGastos.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Movimientos.Gastos
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPresupuestoService _presupuestoService;

        public CreateModel(ApplicationDbContext context, IPresupuestoService presupuestoService)
        {
            _context = context;
            _presupuestoService = presupuestoService;
        }

        [BindProperty]
        public GastoViewModel GastoVM { get; set; }

        public IList<FondoMonetario> FondosLista { get; set; }
        public IList<TipoGasto> TiposGastoLista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await CargarListas();
            GastoVM = new GastoViewModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListas();
                return Page();
            }

            if (GastoVM.Detalles == null || !GastoVM.Detalles.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos un detalle al gasto.");
                await CargarListas();
                return Page();
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                foreach (var detalle in GastoVM.Detalles)
                {
                    if (detalle.TipoGastoId == 0)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un tipo de gasto para cada detalle.");
                        await CargarListas();
                        return Page();
                    }

                    var validacion = await _presupuestoService.ValidarPresupuestoAsync(
                        userId,
                        detalle.TipoGastoId,
                        detalle.Monto,
                        GastoVM.Fecha
                    );

                    if (validacion.excedido)
                    {
                        var tipoGasto = await _context.TiposGasto.FindAsync(detalle.TipoGastoId);
                        ModelState.AddModelError("", $"El gasto de tipo {tipoGasto?.Nombre} excede el presupuesto mensual. " +
                            $"Presupuestado: {validacion.montoPresupuestado:C}, Gastado: {validacion.montoGastado:C}");
                        await CargarListas();
                        return Page();
                    }
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var gasto = GastoVM.ToGasto();
                    gasto.UsuarioId = userId;

                    await _context.Gastos.AddAsync(gasto);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return RedirectToPage("./Index");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar el gasto: " + ex.Message);
                await CargarListas();
                return Page();
            }
        }

        private async Task CargarListas()
        {
            FondosLista = await _context.FondosMonetarios.ToListAsync();
            TiposGastoLista = await _context.TiposGasto.OrderBy(t => t.Nombre).ToListAsync();
        }
    }
} 