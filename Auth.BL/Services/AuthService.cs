using Auth.Common.Dtos;
using Auth.Common.Enums;
using Auth.Common.Interfaces;
using Auth.DAL;
using Auth.DAL.Entities;
using AutoMapper;
using Common.Configurators.ConfigClasses;
using delivery_backend_advanced.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auth.BL.Services;

public class AuthService : IAuthService
{
    private readonly AuthDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public AuthService(AuthDbContext context, UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> Login(LoginCredsDto loginCredsDto)
    {
        var findUser = await _userManager.FindByEmailAsync(loginCredsDto.email) ??
                       throw new BadRequestException("Invalid credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(findUser, loginCredsDto.password);
        if (!isPasswordValid) throw new BadRequestException("Invalid credentials");

        var roles = await GetUserRoles(findUser.Id);

        var tokenUser = _mapper.Map<TokenUserDto>(findUser);
        var token = await _tokenService.CreateToken(tokenUser, roles);

        return new TokenDto
        {
            token = token
        };
    }

    public async Task<TokenDto> Register(RegisterDto registerDto)
    {
        var newUser = _mapper.Map<AppUser>(registerDto);
        var result = await _userManager.CreateAsync(newUser, registerDto.password);

        if (!result.Succeeded)
        {
            var errorsStrings = result.Errors.Select(e => e.Description.ToString()).ToList();
            throw new AuthErrorsException(errorsStrings);
        }

        var findUser = await _context
                           .Users
                           .FirstOrDefaultAsync(user => user.Email == registerDto.email) ??
                       throw new NotFoundException($"User with email {registerDto.email} not found");
        await _userManager.AddToRoleAsync(findUser, UserRole.User.ToString());

        var loginCreds = new LoginCredsDto
        {
            email = registerDto.email,
            password = registerDto.password
        };
        return await Login(loginCreds);
    }
    
    
    
    private async Task<List<IdentityRole>> GetUserRoles(string id)
    {
        var rolesIds = await _context
            .UserRoles
            .Where(role => role.UserId == id)
            .Select(role => role.RoleId)
            .ToListAsync();
        var roles = await _context
            .Roles
            .Where(role => rolesIds.Contains(role.Id))
            .ToListAsync();

        return roles;
    }
}