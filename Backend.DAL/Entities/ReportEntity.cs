using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Entities;

public class ReportEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public UserEntity User { get; set; }
    
    [Required]
    public SportObjectEntity SportObject { get; set; }
}