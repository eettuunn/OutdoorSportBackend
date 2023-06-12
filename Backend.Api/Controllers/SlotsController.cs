using System.Security.Claims;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/slots")]
[Authorize]
public class SlotsController : ControllerBase
{
    private readonly ISlotsService _slotsService;

    public SlotsController(ISlotsService slotsService)
    {
        _slotsService = slotsService;
    }

    [HttpPost("{objectId}")]
    public async Task SignSlot(Guid objectId, [FromBody] SignSlotDto signSlotDto)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _slotsService.SignSlot(objectId, signSlotDto, email);
    }
}