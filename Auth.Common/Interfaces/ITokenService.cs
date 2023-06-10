using System.Security.Claims;
using Auth.Common.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Auth.Common.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(TokenUserDto tokenUserDto, List<IdentityRole> roles);
    ClaimsPrincipal? GetExpiredTokenInfo(string? accessToken);
}