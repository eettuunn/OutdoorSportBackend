using Backend.Common.Dtos;
using Backend.Common.Enums;
using Backend.Common.Interfaces;
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
}