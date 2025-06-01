using System.ComponentModel.DataAnnotations;

namespace MovimientoGastos.Models.ViewModels
{
    public class GastoViewModel
    {
        public GastoViewModel()
        {
            Detalles = new List<GastoDetalleViewModel>();
            Fecha = DateTime.Today;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El comercio es requerido")]
        [StringLength(100)]
        [Display(Name = "Comercio")]
        public string Comercio { get; set; }

        [Required(ErrorMessage = "El tipo de documento es requerido")]
        [StringLength(50)]
        [Display(Name = "Tipo de Documento")]
        public string TipoDoc { get; set; }

        [Required(ErrorMessage = "El número de documento es requerido")]
        [StringLength(50)]
        [Display(Name = "Número de Documento")]
        public string NumeroDoc { get; set; }

        [Required(ErrorMessage = "El fondo es requerido")]
        [Display(Name = "Fondo")]
        public int FondoMonetarioId { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public List<GastoDetalleViewModel> Detalles { get; set; }

        [Display(Name = "Total")]
        public decimal Total => Detalles?.Sum(d => d.Monto) ?? 0;

        public static GastoViewModel FromGasto(Gasto gasto)
        {
            return new GastoViewModel
            {
                Id = gasto.Id,
                Fecha = gasto.Fecha,
                Comercio = gasto.Comercio,
                TipoDoc = gasto.TipoDoc,
                NumeroDoc = gasto.NumeroDoc,
                FondoMonetarioId = gasto.FondoMonetarioId,
                Observaciones = gasto.Observaciones,
                Detalles = gasto.Detalles?.Select(d => GastoDetalleViewModel.FromGastoDetalle(d)).ToList() ?? new List<GastoDetalleViewModel>()
            };
        }

        public Gasto ToGasto()
        {
            return new Gasto
            {
                Id = this.Id,
                Fecha = this.Fecha,
                Comercio = this.Comercio,
                TipoDoc = this.TipoDoc,
                NumeroDoc = this.NumeroDoc,
                FondoMonetarioId = this.FondoMonetarioId,
                Observaciones = this.Observaciones,
                Detalles = this.Detalles?.Select(d => d.ToGastoDetalle()).ToList() ?? new List<GastoDetalle>()
            };
        }
    }
}