namespace Auth.Common.Dtos;

public class TokenUserDto
{
    public Guid id { get; set; }
    
    public string email { get; set; }
    
    public string userName { get; set; }
    
    public bool isBanned { get; set; }
}