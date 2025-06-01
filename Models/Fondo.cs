using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimientoGastos.Models
{
    public class Fondo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El saldo inicial es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Saldo Inicial")]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor o igual a 0")]
        public decimal SaldoInicial { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [NotMapped]
        public decimal SaldoActual => SaldoInicial - (Gastos?.Sum(g => g.Detalles.Sum(d => d.Monto)) ?? 0);

        // Navegación
        public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
} 