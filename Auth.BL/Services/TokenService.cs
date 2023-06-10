using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Common.Dtos;
using Auth.Common.Enums;
using Auth.Common.Interfaces;
using Common.Configurators.ConfigClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.BL.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> CreateToken(TokenUserDto tokenUserDto, List<IdentityRole> roles)
    {
        var newToken = CreateJwtToken(await CreateClaims(tokenUserDto, roles));
        var tokenHandler = new JwtSecurityTokenHandler();
        
        return tokenHandler.WriteToken(newToken);
    }

    private async Task<List<Claim>> CreateClaims(TokenUserDto user, List<IdentityRole> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.id.ToString()),
            new(ClaimTypes.Name, user.userName),
            new(ClaimTypes.Email, user.email),
            new("ban", user.isBanned.ToString())
        };
        
        foreach (var r in roles)
        {
            claims.Add(new(ClaimTypes.Role, r.Name));
        }
        return claims;
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
    {
        var jwtConfig = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
        return new JwtSecurityToken(
            jwtConfig.Issuer,
            jwtConfig.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfig.AccessMinutesLifeTime),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key)),
                SecurityAlgorithms.HmacSha256)
        );
    }
    
    public ClaimsPrincipal? GetExpiredTokenInfo(string? accessToken)
    {
        var jwtConfig = _configuration.GetSection("JwtConfig").Get<JwtConfig>();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}