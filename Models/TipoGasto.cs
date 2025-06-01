using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MovimientoGastos.Models
{
    public class TipoGasto
    {
        public TipoGasto()
        {
            Presupuestos = new List<Presupuesto>();
            GastoDetalles = new List<GastoDetalle>();
            Codigo = "0000"; // Valor por defecto temporal
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(10, ErrorMessage = "El código no puede tener más de 10 caracteres")]
        [Display(Name = "Código")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El código debe ser numérico")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessage = "La descripción no puede tener más de 250 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Presupuesto> Presupuestos { get; set; }
        public virtual ICollection<GastoDetalle> GastoDetalles { get; set; }
    }
} 