using Auth.Common.Dtos;
using Auth.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<TokenDto>> RegisterUser([FromBody] RegisterDto registerDto)
    {
        if (ModelState.IsValid)
        {
            var token = await _authService.Register(registerDto);
            return Ok(token);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
    
    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenDto>> LoginUser([FromBody] LoginCredsDto loginCredsDto)
    {
        if (ModelState.IsValid)
        {
            var token = await _authService.Login(loginCredsDto);
            return Ok(token);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}