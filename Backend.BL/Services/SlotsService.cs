using AutoMapper;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Backend.DAL;
using Backend.DAL.Entities;
using delivery_backend_advanced.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.BL.Services;

public class SlotsService : ISlotsService
{
    private readonly BackendDbContext _context;
    private readonly IMapper _mapper;

    public SlotsService(BackendDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task SignSlot(Guid objectId, SignSlotDto signSlotDto, string email)
    {
        if (DateTime.UtcNow >= signSlotDto.time)
            throw new BadRequestException("Time must be greater, than current date time");
        
        var sportObj = await _context
            .SportObjects
            .FirstOrDefaultAsync(so => so.Id == objectId) ?? throw new CantFindByIdException("sport object", objectId);
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email);

        var newSlot = _mapper.Map<SlotEntity>(signSlotDto);
        newSlot.User = user;
        newSlot.SportObject = sportObj;

        await _context.Slots.AddAsync(newSlot);
        await _context.SaveChangesAsync();
    }

    public async Task EditSlot(Guid slotId, EditSlotDto editSlotDto, string email)
    {
        if (DateTime.UtcNow >= editSlotDto.time)
            throw new BadRequestException("Time must be greater, than current date time");
        
        var slot = await _context
            .Slots
            .Include(s => s.User)
            .FirstOrDefaultAsync(so => so.Id == slotId) ?? throw new CantFindByIdException("slot", slotId);
        if (slot.User.Email != email) throw new ForbiddenException("You can't edit not your slot");

        slot.Text = editSlotDto.text ?? slot.Text;
        slot.Time = editSlotDto.time ?? slot.Time;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSlot(Guid slotId, string email)
    {
        var slot = await _context
            .Slots
            .Include(s => s.User)
            .FirstOrDefaultAsync(so => so.Id == slotId) ?? throw new CantFindByIdException("slot", slotId);
        if (slot.User.Email != email) throw new ForbiddenException("You can't delete not your slot");

        _context.Slots.Remove(slot);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SlotDto>> GetMySlots(string email)
    {
        var slots = await _context
            .Slots
            .Include(s => s.User)
            .Where(s => s.User.Email == email)
            .ToListAsync();

        var slotDtos = _mapper.Map<List<SlotDto>>(slots);
        return slotDtos;
    }
}