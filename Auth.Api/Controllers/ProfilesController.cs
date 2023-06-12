using System.Security.Claims;
using Auth.Common.Dtos;
using Auth.Common.Interfaces;
using Common.Policies;
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
    [Authorize(Policy = PolicyNames.Ban)]
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

    [HttpPut]
    [Authorize]
    [Authorize(Policy = PolicyNames.Ban)]
    public async Task<ActionResult<TokenDto>> EditProfile([FromBody] EditProfileDto editProfileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        var token = await _profilesService.EditProfile(email, editProfileDto);
        return Ok(token);
    }
}