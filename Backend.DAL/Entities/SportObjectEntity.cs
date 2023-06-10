using System.ComponentModel.DataAnnotations;
using Backend.Common.Enums;

namespace Backend.DAL.Entities;

public class SportObjectEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public float XCoordinate { get; set; }
    
    [Required]
    public float YCoordinate { get; set; }
    
    public SportType? Type { get; set; }

    public List<byte[]> Photos { get; set; } = new();

    public List<CommentEntity> Comments { get; set; } = new();

    public List<RatingEntity> Ratings { get; set; } = new();

    public List<ReportEntity> Reports { get; set; } = new();
}