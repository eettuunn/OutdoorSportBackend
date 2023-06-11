namespace Backend.Common.Dtos;

public class CommentDto
{
    public Guid id { get; set; }
    
    public string text { get; set; }
    
    public string email { get; set; }
}