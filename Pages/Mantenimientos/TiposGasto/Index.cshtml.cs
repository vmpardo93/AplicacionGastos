using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models;

namespace MovimientoGastos.Pages.Mantenimientos.TiposGasto
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TipoGasto> TiposGasto { get; set; }

        public async Task OnGetAsync()
        {
            TiposGasto = await _context.TiposGasto
                .OrderBy(t => t.Codigo)
                .ToListAsync();
        }
    }
} 