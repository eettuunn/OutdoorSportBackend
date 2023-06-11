namespace Backend.Common.Dtos;

public class CommentDto
{
    public Guid id { get; set; }
    
    public DateTime time { get; set; }
    
    public string text { get; set; }
    
    public string userEmail { get; set; }
}