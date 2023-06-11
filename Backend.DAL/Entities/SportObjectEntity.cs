using System.ComponentModel.DataAnnotations;
using Backend.Common.Enums;

namespace Backend.DAL.Entities;

public class SportObjectEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public double XCoordinate { get; set; }
    
    [Required]
    public double YCoordinate { get; set; }
    
    public SportType? Type { get; set; }

    public List<string> Photos { get; set; } = new();
    
    public double? AverageRating { get; set; }
    
    [Required]
    public UserEntity User { get; set; }

    public List<CommentEntity> Comments { get; set; } = new();

    public List<RatingEntity> Ratings { get; set; } = new();

    public List<ReportEntity> Reports { get; set; } = new();
}