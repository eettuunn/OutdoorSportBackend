using Auth.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.DAL;

public static class AuthDALConfigurator
{
    public static void ConfigureAuthDAL(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresAuth");
        builder.Services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connection));
    }

    public static void ConfigureAuthDAL(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
            dbContext?.Database.Migrate();
            
            var initializer = scope.ServiceProvider.GetRequiredService<IAuthDbInitializer>();
            initializer.InitAuthDb();
        }
    }
}