using Auth.Common.Dtos;

namespace Auth.Common.Interfaces;

public interface IAuthService
{
    public Task<TokenDto> Login(LoginCredsDto loginCredsDto);
    public Task<TokenDto> Register(RegisterDto registerDto);
}