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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
            PresupuestoVM = new PresupuestoViewModel();
        }

        [BindProperty]
        public PresupuestoViewModel PresupuestoVM { get; set; }
        public SelectList TiposGasto { get; set; }
        public SelectList Meses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await CargarListas();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Recargar las listas para mantener los datos del formulario
            await CargarListas();

            // Validar que se haya seleccionado un tipo de gasto
            if (PresupuestoVM.TipoGastoId <= 0)
            {
                ModelState.AddModelError("PresupuestoVM.TipoGastoId", "Debe seleccionar un tipo de gasto.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "No se pudo identificar al usuario.");
                return Page();
            }

            // Verificar si el TipoGastoId es válido
            var tipoGastoExists = await _context.TiposGasto.AnyAsync(t => t.Id == PresupuestoVM.TipoGastoId);
            if (!tipoGastoExists)
            {
                ModelState.AddModelError("PresupuestoVM.TipoGastoId", "El tipo de gasto seleccionado no es válido.");
                return Page();
            }

            // Verificar si ya existe un presupuesto para el mismo tipo de gasto y mes
            var presupuestoExistente = await _context.Presupuestos
                .FirstOrDefaultAsync(p => p.UsuarioId == userId &&
                                        p.TipoGastoId == PresupuestoVM.TipoGastoId &&
                                        p.Mes == PresupuestoVM.Mes);

            if (presupuestoExistente != null)
            {
                ModelState.AddModelError("", "Ya existe un presupuesto para este tipo de gasto en el mes seleccionado.");
                return Page();
            }

            var presupuesto = new Presupuesto
            {
                TipoGastoId = PresupuestoVM.TipoGastoId,
                Mes = PresupuestoVM.Mes,
                Monto = PresupuestoVM.Monto,
                UsuarioId = userId
            };

            _context.Presupuestos.Add(presupuesto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
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