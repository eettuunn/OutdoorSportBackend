using Backend.Common.Interfaces;
using Backend.DAL;
using Microsoft.EntityFrameworkCore;

namespace OutdoorSportBackend.Configurators;

public static class BackendDALConfigurator
{
    public static void ConfigureBackendDAL(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresBackend");
        builder.Services.AddDbContext<BackendDbContext>(options => options.UseNpgsql(connection));
    }

    public static void ConfigureBackendDAL(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<BackendDbContext>();
            dbContext?.Database.Migrate();
            
            var initializer = scope.ServiceProvider.GetRequiredService<IBackDbInitializer>();
            initializer.InitBackDb();
        }
    }
}