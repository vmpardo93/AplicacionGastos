using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimientoGastos.Models
{
    public class GastoDetalle
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "El gasto es requerido")]
        public int GastoId { get; set; }

        [ForeignKey("GastoId")]
        public virtual Gasto Gasto { get; set; }

        //[Required(ErrorMessage = "El tipo de gasto es requerido")]
        [Display(Name = "Tipo de Gasto")]
        public int TipoGastoId { get; set; }

        [ForeignKey("TipoGastoId")]
        public virtual TipoGasto TipoGasto { get; set; }

        //[Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
} 