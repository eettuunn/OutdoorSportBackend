using System.Security.Claims;
using Auth.Common.Dtos;
using Auth.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/profile")]
public class ProfilesController : ControllerBase
{
    private readonly IProfilesService _profilesService;

    public ProfilesController(IProfilesService profilesService)
    {
        _profilesService = profilesService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ProfileDto> GetProfile()
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        return await _profilesService.GetProfile(email);
    }

    [HttpGet("{email}")]
    public async Task<ProfileDto> GetProfileById(string email)
    {
        return await _profilesService.GetProfileByEmail(email);
    }
}