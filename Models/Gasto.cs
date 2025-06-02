using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimientoGastos.Models
{
    public class Gasto
    {
        public Gasto()
        {
            Detalles = new List<GastoDetalle>();
        }

        public int Id { get; set; }

        //[Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        //[Required(ErrorMessage = "El comercio es requerido")]
        [StringLength(100)]
        [Display(Name = "Comercio")]
        public string Comercio { get; set; }

        //[Required(ErrorMessage = "El tipo de documento es requerido")]
        [StringLength(50)]
        [Display(Name = "Tipo de Documento")]
        public string TipoDoc { get; set; }

        //[Required(ErrorMessage = "El número de documento es requerido")]
        [StringLength(50)]
        [Display(Name = "Número de Documento")]
        public string NumeroDoc { get; set; }

        [Display(Name = "Fondo")]
        public int FondoMonetarioId { get; set; }


        //[Required(ErrorMessage = "El usuario es requerido")]
        [StringLength(450)]
        public string UsuarioId { get; set; }

        [ForeignKey("FondoMonetarioId")]
        public virtual FondoMonetario? FondoMonetario { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public virtual ICollection<GastoDetalle> Detalles { get; set; }

        [NotMapped]
        public decimal Total => Detalles?.Sum(d => d.Monto) ?? 0;
    }
} 

