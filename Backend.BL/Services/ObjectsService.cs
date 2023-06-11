using AutoMapper;
using Backend.Common.Dtos;
using Backend.Common.Enums;
using Backend.Common.Interfaces;
using Backend.DAL;
using Backend.DAL.Entities;
using delivery_backend_advanced.Exceptions;
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

    public async Task<ObjectDto> GetObjectDetails(Guid id)
    {
        var sportObj = await _context
            .SportObjects
            .Include(so => so.Comments)
            .Include(so => so.User)
            .FirstOrDefaultAsync(so => so.Id == id)
                       ?? throw new CantFindByIdException("sport object", id);

        var objectDto = _mapper.Map<ObjectDto>(sportObj);
        objectDto.userEmail = sportObj.User.Email;
        objectDto.comments = _mapper.Map<List<CommentDto>>(sportObj.Comments);

        return objectDto;
    }

    public async Task CreateObject(CreateObjectDto createObjectDto, string email)
    {
        var newObject = _mapper.Map<SportObjectEntity>(createObjectDto);
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email) 
                   ?? throw new NotFoundException($"Can't find user with email {email}");

        newObject.User = user;

        await _context.SportObjects.AddAsync(newObject);
        await _context.SaveChangesAsync();
    }
}