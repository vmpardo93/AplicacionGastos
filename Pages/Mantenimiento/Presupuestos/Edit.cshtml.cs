using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;
using MovimientoGastos.Models.ViewModels;
using System.Security.Claims;

namespace MovimientoGastos.Pages.Mantenimiento.Presupuestos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PresupuestoViewModel PresupuestoVM { get; set; }
        public SelectList TiposGasto { get; set; }
        public SelectList Meses { get; set; }

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
            await CargarListas();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarListas();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            // Verificar si el presupuesto existe y pertenece al usuario
            var presupuesto = await _context.Presupuestos
                .FirstOrDefaultAsync(p => p.Id == PresupuestoVM.Id);

            if (presupuesto == null || presupuesto.UsuarioId != userId)
            {
                return NotFound();
            }

            // Verificar si ya existe otro presupuesto para el mismo tipo de gasto y mes
            var presupuestoExistente = await _context.Presupuestos
                .FirstOrDefaultAsync(p => p.UsuarioId == userId &&
                                        p.TipoGastoId == PresupuestoVM.TipoGastoId &&
                                        p.Mes == PresupuestoVM.Mes &&
                                        p.Id != PresupuestoVM.Id);

            if (presupuestoExistente != null)
            {
                ModelState.AddModelError("", "Ya existe un presupuesto para este tipo de gasto en el mes seleccionado.");
                return Page();
            }

            // Actualizar los campos del presupuesto
            presupuesto.TipoGastoId = PresupuestoVM.TipoGastoId;
            presupuesto.Mes = PresupuestoVM.Mes;
            presupuesto.Monto = PresupuestoVM.Monto;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PresupuestoExists(PresupuestoVM.Id))
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

        private async Task<bool> PresupuestoExists(int id)
        {
            return await _context.Presupuestos.AnyAsync(e => e.Id == id);
        }

        private async Task CargarListas()
        {
            // Cargar tipos de gasto
            var tiposGasto = await _context.TiposGasto
                .OrderBy(t => t.Nombre)
                .Select(t => new { t.Id, t.Nombre })
                .ToListAsync();

            TiposGasto = new SelectList(tiposGasto, "Id", "Nombre");
            
            // Cargar meses
            var mesesLista = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Numero = m,
                    Nombre = new DateTime(2024, m, 1).ToString("MMMM")
                })
                .ToList();
            
            Meses = new SelectList(mesesLista, "Numero", "Nombre");
        }
    }
} 