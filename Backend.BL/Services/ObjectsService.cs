using AutoMapper;
using Backend.Common.Dtos;
using Backend.Common.Enums;
using Backend.Common.Interfaces;
using Backend.DAL;
using Backend.DAL.Entities;
using delivery_backend_advanced.Exceptions;
using Microsoft.AspNetCore.Http;
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
            .ThenInclude(c => c.User)
            .Include(so => so.User)
            .Include(so => so.Slots)
            .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(so => so.Id == id)
                       ?? throw new CantFindByIdException("sport object", id);

        var objectDto = _mapper.Map<ObjectDto>(sportObj);
        objectDto.userEmail = sportObj.User.Email;
        objectDto.comments = _mapper.Map<List<CommentDto>>(sportObj.Comments);
        for (int i = 0; i < objectDto.comments.Count; i++)
        {
            objectDto.comments[i].email = sportObj.Comments[i].User.Email;
        }
        objectDto.slots = objectDto.slots.OrderBy(s => s.time).ToList();
        for (int i = 0; i < objectDto.slots.Count; i++)
        {
            objectDto.slots[i].userEmail = sportObj.Slots[i].User.Email;
        }
        //todo: background job for deleting past slots
        
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

    public async Task EditObject(Guid id, EditObjectDto editObjectDto, string email)
    {
        var sportObj = await _context
            .SportObjects
            .Include(so => so.User)
            .FirstOrDefaultAsync(so => so.Id == id) 
                       ?? throw new CantFindByIdException("sport object", id);
        if (sportObj.User.Email != email) throw new ForbiddenException("You can't edit not your object");

        sportObj.Type = editObjectDto.type ?? sportObj.Type;
        sportObj.Address = editObjectDto.address ?? sportObj.Address;
        sportObj.XCoordinate = editObjectDto.xCoordinate ?? sportObj.XCoordinate;
        sportObj.YCoordinate = editObjectDto.yCoordinate ?? sportObj.YCoordinate;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteObject(Guid id, string email)
    {
        var sportObject = await _context
            .SportObjects
            .Include(so => so.User)
            .FirstOrDefaultAsync(so => so.Id == id) ?? throw new CantFindByIdException("sport object", id);
        if (sportObject.User.Email != email) throw new ForbiddenException("You can't delete not your sport object");

        _context.Remove(sportObject);
        await _context.SaveChangesAsync();
    }

    public async Task AddObjectPhotos(Guid id, List<IFormFile> photos)
    {
        //todo: maybe only for your object
        var sportObject = await _context
            .SportObjects
            .FirstOrDefaultAsync(so => so.Id == id) ?? throw new CantFindByIdException("sport object", id);

        foreach (var photo in photos)
        {
            using (var ms = new MemoryStream())
            {
                await photo.CopyToAsync(ms);
                var bytes = ms.ToArray();
                var bytesString = Convert.ToBase64String(bytes);
                sportObject.Photos.Add(bytesString);
                await _context.SaveChangesAsync();
            }   
        }
    }
}