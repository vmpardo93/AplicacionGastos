using System.ComponentModel.DataAnnotations;

namespace MovimientoGastos.Models
{
    public class FondoMonetario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El tipo es requerido")]
        [StringLength(50, ErrorMessage = "El tipo no puede tener más de 50 caracteres")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Gasto> Gastos { get; set; }
        public virtual ICollection<Deposito> Depositos { get; set; }
    }
} 