using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Models;

namespace MovimientoGastos.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                // Aplicar todas las migraciones pendientes
                context.Database.Migrate();

                // Verificar si ya existen fondos monetarios
                if (context.FondosMonetarios.Any())
                {
                    return;   // La base de datos ya tiene datos
                }

                var fondosMonetarios = new FondoMonetario[]
                {
                    new FondoMonetario
                    {
                        Nombre = "Cuentas Bancarias",
                        Tipo = "1"
                    },
                    new FondoMonetario
                    {
                        Nombre = "Fondos de Caja Menuda",
                        Tipo = "2"
                    }
                };

                context.FondosMonetarios.AddRange(fondosMonetarios);
                context.SaveChanges();
            }
            catch
            {
                // Simplemente continuar si hay algún error
                return;
            }
        }
    }
} 