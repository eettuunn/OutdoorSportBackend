using System.ComponentModel.DataAnnotations;

namespace Auth.Common.Dtos;

public class LoginCredsDto
{
    [Required] public string email { get; set; }

    [Required] public string password { get; set; }
}