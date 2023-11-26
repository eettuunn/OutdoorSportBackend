using System.ComponentModel.DataAnnotations;
using Backend.Common.Enums;

namespace Backend.Common.Dtos;

public class UserSlotDto
{
    public Guid id { get; set; }
    
    public string? text { get; set; }
    
    public string address { get; set; }
    
    public SportType? type { get; set; }

    [Required]
    public DateTime time { get; set; }
    
    [Required]
    public string userEmail { get; set; }
}