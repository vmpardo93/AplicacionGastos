using System.ComponentModel.DataAnnotations;

namespace MovimientoGastos.Models.ViewModels
{
    public class PresupuestoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de gasto es requerido")]
        [Display(Name = "Tipo de Gasto")]
        public int TipoGastoId { get; set; }

        [Required(ErrorMessage = "El mes es requerido")]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12")]
        [Display(Name = "Mes")]
        public int Mes { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        // Propiedad para mostrar el nombre del tipo de gasto
        [Display(Name = "Tipo de Gasto")]
        public string? TipoGastoNombre { get; set; }

        // Métodos de conversión
        public static PresupuestoViewModel FromPresupuesto(Presupuesto presupuesto)
        {
            return new PresupuestoViewModel
            {
                Id = presupuesto.Id,
                TipoGastoId = presupuesto.TipoGastoId,
                Mes = presupuesto.Mes,
                Monto = presupuesto.Monto,
                TipoGastoNombre = presupuesto.TipoGasto?.Nombre
            };
        }

        public Presupuesto ToPresupuesto(string usuarioId)
        {
            return new Presupuesto
            {
                Id = this.Id,
                TipoGastoId = this.TipoGastoId,
                Mes = this.Mes,
                Monto = this.Monto,
                UsuarioId = usuarioId
            };
        }

        // Propiedad para mostrar el nombre del mes con validación
        public string NombreMes 
        {
            get
            {
                if (Mes >= 1 && Mes <= 12)
                {
                    return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes);
                }
                return "Mes inválido";
            }
        }
    }
} 