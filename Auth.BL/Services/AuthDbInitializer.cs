using Auth.Common.Interfaces;
using Auth.DAL.Entities;
using Common.Configurators.ConfigClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auth.DAL;

public class AuthDbInitializer : IAuthDbInitializer
{
    private readonly AuthDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthDbInitializer(AuthDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public void InitAuthDb()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception)
        {
            throw new Exception("Something went wrong when initializing Auth DB");
        }

        if (_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult()) return;
        
        var adminConfig = _configuration.GetSection("AdminConfig").Get<AdminConfig>();

        _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();

        var admin = new AppUser()
        {
            UserName = adminConfig.UserName,
            Email = adminConfig.Email,
            EmailConfirmed = true
        };
        _userManager.CreateAsync(admin, adminConfig.Password).GetAwaiter().GetResult();

        var user =
            _context.Users.FirstOrDefault(u => u.Email == adminConfig.Email);
        if (user != null)
        {
            _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
            var adminEntity = new AdminEntity
            {
                Id = new Guid(),
                User = user
            };
            _context.Admins.Add(adminEntity);
            _context.SaveChanges();
        }
    }
}