using System.Security.Claims;
using Backend.Common.Dtos;
using Backend.Common.Enums;
using Backend.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/objects")]
public class ObjectsController : ControllerBase
{
    private readonly IObjectsService _objectsService;

    public ObjectsController(IObjectsService objectsService)
    {
        _objectsService = objectsService;
    }

    [HttpGet]
    public async Task<List<ObjectListDto>> GetObjects(SportType? type)
    {
        return await _objectsService.GetObjectsList(type);
    }
    
    [HttpGet("{id}")]
    public async Task<ObjectDto> GetObjectDetails(Guid id)
    {
        return await _objectsService.GetObjectDetails(id);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateObject([FromBody] CreateObjectDto createObjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _objectsService.CreateObject(createObjectDto, email);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditObject([FromBody] EditObjectDto editObjectDto, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _objectsService.EditObject(id, editObjectDto, email);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task DeleteObject(Guid id)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _objectsService.DeleteObject(id, email);
    }

    [HttpPut("{id}/photos")]
    [Authorize]
    public async Task AddObjectPhotos(Guid id, [FromForm] List<IFormFile> photos)
    {
        await _objectsService.AddObjectPhotos(id, photos);
    }
}