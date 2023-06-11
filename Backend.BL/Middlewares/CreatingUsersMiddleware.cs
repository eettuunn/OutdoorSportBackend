using System.Security.Claims;
using Backend.DAL;
using Backend.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace delivery_backend_advanced.Services.ExceptionHandler;

public class CreatingUsersMiddleware
{
    private readonly RequestDelegate _next;

    public CreatingUsersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, BackendDbContext _context)
    {
        await _next(context);

        var isAuth = context.User.Identity.IsAuthenticated;
        if (isAuth)
        {
            var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!await _context.Users.AnyAsync(u => u.Id == Guid.Parse(id)))
            {
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                var userName = context.User.FindFirst(ClaimTypes.Name)?.Value;
                var user = new UserEntity
                {
                    Id = Guid.Parse(id),
                    Email = email,
                    UserName = userName
                };
                
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
        }
        
    }
}