using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Entities;

public class SlotEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public DateTime Time { get; set; }
    
    public string? Text { get; set; }
    
    [Required]
    public UserEntity User { get; set; }
}