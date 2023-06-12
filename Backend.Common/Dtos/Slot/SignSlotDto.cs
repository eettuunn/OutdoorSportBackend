using System.ComponentModel.DataAnnotations;

namespace Backend.Common.Dtos;

public class SignSlotDto
{
    [Required]
    public string text { get; set; }
    
    [Required]
    public DateTime time { get; set; }
}