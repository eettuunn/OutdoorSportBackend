using Auth.Common.Dtos;
using Auth.Common.Interfaces;
using Auth.DAL;
using Auth.DAL.Entities;
using AutoMapper;
using delivery_backend_advanced.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.BL.Services;

public class ProfilesService : IProfilesService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly AuthDbContext _context;

    public ProfilesService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, AuthDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<ProfileDto> GetProfile(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null) throw new NotAuthorizedException("Not authorized");

        var profileDto = _mapper.Map<ProfileDto>(user);
        return profileDto;
    }

    public async Task<ProfileDto> GetProfileByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) throw new NotFoundException($"Can't find user with email {email}");

        var profileDto = _mapper.Map<ProfileDto>(user);
        profileDto.isBanned = user.IsBanned;
        return profileDto;
    }

    public async Task<TokenDto> EditProfile(string email, EditProfileDto editProfileDto)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new NotAuthorizedException("Not authorized");

        user.UserName = editProfileDto.userName ?? user.UserName;
        user.Email = editProfileDto.email ?? user.Email;
        user.PhoneNumber = editProfileDto.phoneNumber ?? user.PhoneNumber;
        user.Myself = editProfileDto.myself ?? user.Myself;
        user.Contacts = editProfileDto.contacts.Count == 0 ? user.Contacts : editProfileDto.contacts;
        user.Sports = editProfileDto.sports.Count == 0 ? user.Sports : editProfileDto.sports;

        await _userManager.UpdateAsync(user);
        
        var roles = await GetUserRoles(user.Id);
        var tokenUser = _mapper.Map<TokenUserDto>(user);
        var token = await _tokenService.CreateToken(tokenUser, roles);

        return new TokenDto
        {
            token = token
        };
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