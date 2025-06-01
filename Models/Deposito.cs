using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimientoGastos.Models
{
    public class Deposito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El fondo es requerido")]
        [Display(Name = "Fondo")]
        public int FondoMonetarioId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        // Propiedad de navegación
        public virtual FondoMonetario FondoMonetario { get; set; }
    }
} 