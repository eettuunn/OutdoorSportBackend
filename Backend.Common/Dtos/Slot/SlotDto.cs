using System.ComponentModel.DataAnnotations;

namespace Backend.Common.Dtos;

public class SlotDto
{
    public Guid id { get; set; }
    
    public string? text { get; set; }
    
    [Required]
    public DateTime time { get; set; }
    
    [Required]
    public string userEmail { get; set; }
}