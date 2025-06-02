using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MovimientoGastos.Data;
using MovimientoGastos.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar puerto para Heroku
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Configuración de base de datos
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var databaseUri = new Uri(databaseUrl);
var userInfo = databaseUri.UserInfo.Split(':');
var connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.Substring(1)};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

// ✅ CONFIGURAR NPGSQL PARA MANEJAR FECHAS AUTOMÁTICAMENTE
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ✅ CONFIGURAR IDENTITY CON ROLES CORRECTAMENTE
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = false;  // ✅ Simplificar para testing
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;    // ✅ Mínimo para testing
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()  // ✅ CRÍTICO: Agregar soporte para roles
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();

var app = builder.Build();

// ✅ INICIALIZACIÓN SEGURA CON TRY-CATCH
using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Solo aplicar migraciones, no seeds por ahora
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migraciones aplicadas correctamente");
        
        // Intentar seeds solo si Identity está funcionando
        try
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await DbInitializer.Initialize(context, userManager, roleManager);
            Console.WriteLine("✅ Seeds ejecutados correctamente");
        }
        catch (Exception seedEx)
        {
            Console.WriteLine($"⚠️ Error en seeds (ignorando): {seedEx.Message}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error crítico en inicialización: {ex.Message}");
        // No hacer throw para evitar crash total
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();