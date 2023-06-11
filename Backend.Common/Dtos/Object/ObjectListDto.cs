using Backend.Common.Enums;

namespace Backend.Common.Dtos;

public class ObjectListDto
{
    public Guid id { get; set; }
    
    public double xCoordinate { get; set; }
    
    public double yCoordinate { get; set; }
    
    public SportType? type { get; set; }
}