using Microsoft.AspNetCore.Identity;

namespace Auth.DAL.Entities;

public class AppUser : IdentityUser
{
    public bool IsBanned { get; set; } = false;
    
    public AdminEntity? Admin { get; set; }
}