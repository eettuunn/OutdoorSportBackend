using Backend.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.DAL.Entities;

public class AppUser : IdentityUser
{
    public List<SportsType>? Sports { get; set; } = new();

    public List<string>? Contacts { get; set; } = new();
    
    public string? Myself { get; set; }
    
    public bool IsBanned { get; set; } = false;
    
    public AdminEntity? Admin { get; set; }
}