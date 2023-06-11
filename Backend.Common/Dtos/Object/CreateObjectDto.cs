using System.ComponentModel.DataAnnotations;
using Backend.Common.Enums;

namespace Backend.Common.Dtos;

public class CreateObjectDto
{
    [Required]
    public string address { get; set; }
    
    [Required]
    public double xCoordinate { get; set; }
    
    [Required]
    public double yCoordinate { get; set; }
    
    public SportType? type { get; set; }
}