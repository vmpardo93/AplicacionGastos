using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Models;

namespace MovimientoGastos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoGasto> TiposGasto { get; set; }
        public DbSet<FondoMonetario> FondosMonetarios { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<GastoDetalle> GastosDetalle { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Fondo> Fondos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales de las entidades
            modelBuilder.Entity<TipoGasto>()
                .HasIndex(t => t.Codigo)
                .IsUnique();

            modelBuilder.Entity<Presupuesto>()
                .HasIndex(p => new { p.UsuarioId, p.TipoGastoId, p.Mes })
                .IsUnique();

            // Configuración de la relación entre Gasto y GastoDetalle
            modelBuilder.Entity<GastoDetalle>()
                .HasOne(d => d.Gasto)
                .WithMany(g => g.Detalles)
                .HasForeignKey(d => d.GastoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la relación entre GastoDetalle y TipoGasto
            modelBuilder.Entity<GastoDetalle>()
                .HasOne(d => d.TipoGasto)
                .WithMany(t => t.GastoDetalles)
                .HasForeignKey(d => d.TipoGastoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la relación entre Gasto y FondoMonetario
            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.FondoMonetario)
                .WithMany(f => f.Gastos)
                .HasForeignKey(g => g.FondoMonetarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la relación entre Deposito y FondoMonetario
            modelBuilder.Entity<Deposito>()
                .HasOne(d => d.FondoMonetario)
                .WithMany(f => f.Depositos)
                .HasForeignKey(d => d.FondoMonetarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}