using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovimientoGastos.Models;

namespace MovimientoGastos.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // Aplicar todas las migraciones pendientes
                await context.Database.MigrateAsync();

                // Crear roles si no existen
                await CreateRoles(roleManager);

                // Crear usuario administrador
                await CreateAdminUser(userManager);

                // Crear datos semilla para fondos monetarios
                await CreateFondosMonetarios(context);
            }
            catch (Exception ex)
            {
                // Log del error si es necesario
                Console.WriteLine($"Error en DbInitializer: {ex.Message}");
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task CreateAdminUser(UserManager<IdentityUser> userManager)
        {
            // Verificar si el usuario admin ya existe
            var adminUser = await userManager.FindByNameAsync("admin");
            
            if (adminUser == null)
            {
                // Crear usuario admin
                adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "admin");
                
                if (result.Succeeded)
                {
                    // Asignar rol de Admin
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task CreateFondosMonetarios(ApplicationDbContext context)
        {
            // Verificar si ya existen fondos monetarios
            if (await context.FondosMonetarios.AnyAsync())
            {
                return; // La base de datos ya tiene datos
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
            await context.SaveChangesAsync();
        }
    }
} 