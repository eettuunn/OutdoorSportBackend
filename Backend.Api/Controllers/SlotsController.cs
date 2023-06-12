using System.Security.Claims;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Common.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/slots")]
[Authorize]
[Authorize(Policy = PolicyNames.Ban)]
public class SlotsController : ControllerBase
{
    private readonly ISlotsService _slotsService;

    public SlotsController(ISlotsService slotsService)
    {
        _slotsService = slotsService;
    }

    [HttpPost("{objectId}")]
    public async Task<IActionResult> SignSlot(Guid objectId, [FromBody] SignSlotDto signSlotDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.SignSlot(objectId, signSlotDto, email);
        return Ok();
    }
    
    [HttpPut("{slotId}")]
    public async Task<IActionResult> EditSlot(Guid slotId, [FromBody] EditSlotDto editSlotDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.EditSlot(slotId, editSlotDto, email);
        return Ok();
    }
    
    [HttpDelete("{slotId}")]
    public async Task DeleteSlot(Guid slotId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.DeleteSlot(slotId, email);
    }
}