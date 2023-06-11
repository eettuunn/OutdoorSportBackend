using AutoMapper;
using Backend.Common.Dtos;
using Backend.Common.Enums;
using Backend.Common.Interfaces;
using Backend.DAL;
using Microsoft.EntityFrameworkCore;

namespace Backend.BL.Services;

public class ObjectsService : IObjectsService
{
    private readonly BackendDbContext _context;
    private readonly IMapper _mapper;

    public ObjectsService(BackendDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ObjectListDto>> GetObjectsList(SportType? type)
    {
        var sportObjects = await _context
            .SportObjects
            .Where(so => type == null || so.Type == type)
            .ToListAsync();

        var objectDtos = _mapper.Map<List<ObjectListDto>>(sportObjects);
        return objectDtos;
    }
}