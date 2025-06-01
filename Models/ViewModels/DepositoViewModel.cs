using System.ComponentModel.DataAnnotations;

namespace MovimientoGastos.Models.ViewModels
{
    public class DepositoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El fondo es requerido")]
        [Display(Name = "Fondo")]
        public int FondoMonetarioId { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        // Propiedad para mostrar el nombre del fondo monetario
        [Display(Name = "Fondo")]
        public string? FondoMonetarioNombre { get; set; }

        // Constructor
        public DepositoViewModel()
        {
            Fecha = DateTime.Today;
        }

        // Métodos de conversión
        public static DepositoViewModel FromDeposito(Deposito deposito)
        {
            return new DepositoViewModel
            {
                Id = deposito.Id,
                Fecha = deposito.Fecha,
                FondoMonetarioId = deposito.FondoMonetarioId,
                Monto = deposito.Monto,
                FondoMonetarioNombre = deposito.FondoMonetario?.Nombre
            };
        }

        public Deposito ToDeposito()
        {
            return new Deposito
            {
                Id = this.Id,
                Fecha = this.Fecha,
                FondoMonetarioId = this.FondoMonetarioId,
                Monto = this.Monto
            };
        }
    }
}