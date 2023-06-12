using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Backend.DAL;
using delivery_backend_advanced.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.BL.Services;

public class AdminService : IAdminService
{
    private readonly BackendDbContext _context;
    private readonly IMessageProducer _messageProducer;

    public AdminService(BackendDbContext context, IMessageProducer messageProducer)
    {
        _context = context;
        _messageProducer = messageProducer;
    }

    public async Task DeleteSportObject(Guid objectId)
    {
        var sportObj = await _context
            .SportObjects
            .FirstOrDefaultAsync(so => so.Id == objectId) ?? throw new CantFindByIdException("sport object", objectId);

        _context.SportObjects.Remove(sportObj);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSlot(Guid slotId)
    {
        var slot = await _context
            .Slots
            .FirstOrDefaultAsync(s => s.Id == slotId) ?? throw new CantFindByIdException("slot", slotId);

        _context.Slots.Remove(slot);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid commentId)
    {
        var comment = await _context
            .Comments
            .FirstOrDefaultAsync(c => c.Id == commentId) ?? throw new CantFindByIdException("comment", commentId);

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task BanUser(string email)
    {
        var ban = new BanUserModel
        {
            email = email,
            ban = true
        };

        _messageProducer.SendMessage(ban);
    }

    public async Task UnbanUser(string email)
    {
        var unban = new BanUserModel
        {
            email = email,
            ban = false
        };

        _messageProducer.SendMessage(unban);
    }
}