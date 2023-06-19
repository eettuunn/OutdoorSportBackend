using System.ComponentModel.DataAnnotations;
using Backend.Common.Enums;

namespace Auth.Common.Dtos;

public class EditProfileDto
{
    [RegularExpression(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+", ErrorMessage = "Invalid email address")]
    public string? email { get; set; }
    
    public string? userName { get; set; }
    
    public string? phoneNumber { get; set; }
    
    public List<SportsType>? sports { get; set; } = new();

    public List<string>? contacts { get; set; } = new();
    
    public string? myself { get; set; }
}