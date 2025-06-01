using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Mantenimientos.TiposGasto
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
            TipoGasto = new TipoGasto();
        }

        [BindProperty]
        public TipoGasto TipoGasto { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TipoGasto.Nombre))
                {
                    ModelState.AddModelError("TipoGasto.Nombre", "El nombre es requerido");
                    return Page();
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Verificar si ya existe un tipo de gasto con el mismo nombre
                var existeNombre = await _context.TiposGasto
                    .AnyAsync(t => t.Nombre.ToLower() == TipoGasto.Nombre.ToLower());

                if (existeNombre)
                {
                    ModelState.AddModelError("TipoGasto.Nombre", "Ya existe un tipo de gasto con este nombre");
                    return Page();
                }

                // Generar código automáticamente como número secuencial
                var ultimoCodigo = await _context.TiposGasto
                    .OrderByDescending(t => t.Codigo)
                    .Select(t => t.Codigo)
                    .FirstOrDefaultAsync();

                int nuevoNumero = 1;
                if (ultimoCodigo != null && int.TryParse(ultimoCodigo, out int ultimoNumero))
                {
                    nuevoNumero = ultimoNumero + 1;
                }

                TipoGasto.Codigo = nuevoNumero.ToString("D4"); // Formato con 4 dígitos

                _context.TiposGasto.Add(TipoGasto);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tipo de gasto creado exitosamente.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido un error al crear el tipo de gasto. Por favor, inténtelo nuevamente.");
                return Page();
            }
        }
    }
} 