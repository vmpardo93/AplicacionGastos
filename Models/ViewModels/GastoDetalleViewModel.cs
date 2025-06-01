using System.ComponentModel.DataAnnotations;

namespace MovimientoGastos.Models.ViewModels
{
    public class GastoDetalleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de gasto es requerido")]
        [Display(Name = "Tipo de Gasto")]
        public int TipoGastoId { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        public static GastoDetalleViewModel FromGastoDetalle(GastoDetalle detalle)
        {
            return new GastoDetalleViewModel
            {
                Id = detalle.Id,
                TipoGastoId = detalle.TipoGastoId,
                Monto = detalle.Monto,
            };
        }

        public GastoDetalle ToGastoDetalle()
        {
            return new GastoDetalle
            {
                Id = this.Id,
                TipoGastoId = this.TipoGastoId,
                Monto = this.Monto
            };
        }
    }
} 