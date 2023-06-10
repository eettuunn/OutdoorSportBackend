using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Entities;

public class RatingEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public int Value { get; set; }
    
    [Required]
    public UserEntity User { get; set; }
    
    [Required]
    public SportObjectEntity SportObject { get; set; }
}