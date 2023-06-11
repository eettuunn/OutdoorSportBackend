using Microsoft.AspNetCore.Builder;

namespace delivery_backend_advanced.Services.ExceptionHandler;

public static class BackendMiddlewareExtensions
{
    public static void UseCreatingUsersMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CreatingUsersMiddleware>();
    }
}