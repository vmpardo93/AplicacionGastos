using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Data;
using MovimientoGastos.Models.ViewModels;

namespace MovimientoGastos.Pages.Movimientos.Depositos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DepositoViewModel> Depositos { get; set; } = new List<DepositoViewModel>();

        public async Task OnGetAsync()
        {
            var depositos = await _context.Depositos
                .Include(d => d.FondoMonetario)
                .OrderByDescending(d => d.Fecha)
                .ThenByDescending(d => d.Id)
                .ToListAsync();

            Depositos = depositos.Select(d => DepositoViewModel.FromDeposito(d)).ToList();
        }
    }
} 