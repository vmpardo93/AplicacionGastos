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

            // ✅ CONFIGURACIÓN ESPECÍFICA PARA POSTGRESQL
            // Configurar todas las propiedades DateTime para usar timestamp sin timezone
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp without time zone");
                    }
                }
            }

            // Configuraciones adicionales de las entidades
            modelBuilder.Entity<TipoGasto>()
                .HasIndex(t => t.Codigo)
                .IsUnique();

            modelBuilder.Entity<Presupuesto>()
                .HasIndex(p => new { p.UsuarioId, p.TipoGastoId, p.Mes })
                .IsUnique();

            // ✅ CONFIGURACIÓN ESPECÍFICA DE GASTO
            modelBuilder.Entity<Gasto>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .HasColumnType("timestamp without time zone");
                
                // Asegurar que el mapeo de FondoMonetario sea correcto
                entity.Property(e => e.FondoMonetarioId)
                    .HasColumnName("FondoMonetarioId");
            });

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

        // ✅ CONFIGURAR CONVERSIÓN DE FECHAS A UTC
        public override int SaveChanges()
        {
            ConvertDatesToUtc();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDatesToUtc();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDatesToUtc()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.CurrentValue is DateTime dateTime && dateTime.Kind == DateTimeKind.Unspecified)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
    }
}
