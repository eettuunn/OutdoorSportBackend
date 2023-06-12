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

    [HttpDelete("{objectId}")]
    public async Task DeleteObject(Guid objectId)
    {
        await _adminService.DeleteSportObject(objectId);
    }
}