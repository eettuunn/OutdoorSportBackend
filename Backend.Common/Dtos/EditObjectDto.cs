using Backend.Common.Enums;

namespace Backend.Common.Dtos;

public class EditObjectDto
{
    public string? address { get; set; }
    
    public double? xCoordinate { get; set; }
    
    public double? yCoordinate { get; set; }
    
    public SportType? type { get; set; }

    public List<byte[]> photos { get; set; } = new();
}