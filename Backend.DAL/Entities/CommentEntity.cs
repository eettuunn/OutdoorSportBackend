using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public string Text { get; set; }
    
    [Required]
    public DateTime Time { get; set; }
    
    [Required]
    public UserEntity User { get; set; }
    
    [Required]
    public SportObjectEntity SportObject { get; set; }
}