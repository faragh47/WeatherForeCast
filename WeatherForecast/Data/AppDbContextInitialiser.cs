using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApplication2.Data;

public static class AppDbContextInitialiser
{
    public static IServiceCollection AppDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
        });
        
        return services;
    } 
    public static async Task InitialiseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}