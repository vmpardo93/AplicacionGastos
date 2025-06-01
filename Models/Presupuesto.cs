using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MovimientoGastos.Models
{
    public class Presupuesto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        [Display(Name = "Tipo de Gasto")]
        public int TipoGastoId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12")]
        [Display(Name = "Mes")]
        public int Mes { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        // Propiedades de navegación
        [ForeignKey("UsuarioId")]
        public virtual IdentityUser Usuario { get; set; }

        [ForeignKey("TipoGastoId")]
        public virtual TipoGasto TipoGasto { get; set; }
    }
} 