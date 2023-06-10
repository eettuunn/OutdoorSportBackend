using Auth.Common.Dtos;
using Auth.Common.Interfaces;
using Auth.DAL.Entities;
using AutoMapper;
using delivery_backend_advanced.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Auth.BL.Services;

public class ProfilesService : IProfilesService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public ProfilesService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
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
        return profileDto;
    }
}