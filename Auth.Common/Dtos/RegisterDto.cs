using System.ComponentModel.DataAnnotations;

namespace Auth.Common.Dtos;

public class RegisterDto
{
    [Required] public string email { get; set; }
    
    [Required] public string password { get; set; }
    
    [Required] public string userName { get; set; }
    
    public string? phoneNumber { get; set; }
}