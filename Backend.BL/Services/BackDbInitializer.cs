using Backend.Common.Enums;
using Backend.Common.Interfaces;
using Backend.DAL;
using Backend.DAL.Entities;
using Common.Configurators.ConfigClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend.BL.Services;

public class BackDbInitializer : IBackDbInitializer
{
    private readonly BackendDbContext _context;
    private readonly IConfiguration _configuration;

    public BackDbInitializer(BackendDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public void InitBackDb()
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
            throw new Exception("Something went wrong when initializing Backend DB");
        }

        var adminConfig = _configuration.GetSection("AdminConfig").Get<AdminConfig>();

        if (_context.Users.Any(u => u.Email == adminConfig.Email)) return;

        var admin = new UserEntity
        {
            UserName = adminConfig.UserName,
            Email = adminConfig.Email,
            Id = Guid.Parse(adminConfig.Id)
        };
        _context.Users.Add(admin);

        var sportObj1 = new SportObjectEntity
        {
            Id = new Guid(),
            Address = "Улица Максима Горьково, 55",
            XCoordinate = 56.472333,
            YCoordinate = 84.941154,
            Type = SportType.Basketball,
            User = admin
        };
        var sportObj2 = new SportObjectEntity
        {
            Id = new Guid(),
            Address = "Улица Лебедева, 11",
            XCoordinate = 56.478422,
            YCoordinate = 84.970969,
            Type = SportType.Football,
            User = admin
        };

        _context.SportObjects.Add(sportObj1);
        _context.SportObjects.Add(sportObj2);
        
        _context.SaveChanges();
    }
}