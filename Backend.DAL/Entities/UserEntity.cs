using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string UserName { get; set; }

    public List<CommentEntity> Comments { get; set; } = new();

    public List<RatingEntity> Ratings { get; set; } = new();

    public List<ReportEntity> Reports { get; set; } = new();

    public List<SportObjectEntity> SportObjects { get; set; } = new();
    
    public List<SlotEntity> Slots { get; set; } = new();
}