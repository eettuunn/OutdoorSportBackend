using System.ComponentModel.DataAnnotations;

namespace Auth.DAL.Entities;

public class AdminEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public AppUser User { get; set; }
}