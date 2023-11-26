using System.Security.Claims;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Common.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/slots")]
public class SlotsController : ControllerBase
{
    private readonly ISlotsService _slotsService;

    public SlotsController(ISlotsService slotsService)
    {
        _slotsService = slotsService;
    }

    [HttpPost("{objectId}")]
    [Authorize]
    [Authorize(Policy = PolicyNames.Ban)]
    public async Task<IActionResult> SignSlot(Guid objectId, [FromBody] SignSlotDto signSlotDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.SignSlot(objectId, signSlotDto, email);
        return Ok();
    }
    
    [HttpPut("{slotId}")]
    [Authorize]
    [Authorize(Policy = PolicyNames.Ban)]
    public async Task<IActionResult> EditSlot(Guid slotId, [FromBody] EditSlotDto editSlotDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.EditSlot(slotId, editSlotDto, email);
        return Ok();
    }
    
    [HttpDelete("{slotId}")]
    [Authorize]
    [Authorize(Policy = PolicyNames.Ban)]
    public async Task DeleteSlot(Guid slotId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.DeleteSlot(slotId, email);
    }

    [HttpGet]
    [Authorize]
    [Authorize(Policy = PolicyNames.Ban)]
    public async Task<List<UserSlotDto>> GetMySlots()
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        return await _slotsService.GetSlotList(email);
    }
    
    [HttpGet("{email}")]
    public async Task<List<UserSlotDto>> GetUsersSlots(string email)
    {
        return await _slotsService.GetSlotList(email);
    }
}