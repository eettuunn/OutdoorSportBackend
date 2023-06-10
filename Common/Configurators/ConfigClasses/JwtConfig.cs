namespace Common.Configurators.ConfigClasses;

public class JwtConfig
{
    public required string Issuer { get; set; }
    
    public required string Audience { get; set; }
    
    public required string Key { get; set; }
    
    public required int AccessMinutesLifeTime { get; set; }
}