using Backend.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/admin")]
[Authorize]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpDelete("object/delete/{objectId}")]
    public async Task DeleteObject(Guid objectId)
    {
        await _adminService.DeleteSportObject(objectId);
    }
    
    [HttpDelete("slot/delete/{slotId}")]
    public async Task DeleteSlot(Guid slotId)
    {
        await _adminService.DeleteSlot(slotId);
    }
    
    [HttpDelete("comment/delete/{commentId}")]
    public async Task DeleteComment(Guid commentId)
    {
        await _adminService.DeleteComment(commentId);
    }

    [HttpPut("ban/{email}")]
    public async Task BanUser(string email)
    {
        await _adminService.BanUser(email);
    }
    
    [HttpPut("unban/{email}")]
    public async Task UnbanUser(string email)
    {
        await _adminService.UnbanUser(email);
    }
}