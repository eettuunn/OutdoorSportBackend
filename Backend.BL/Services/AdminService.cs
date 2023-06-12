using Backend.Common.Interfaces;
using Backend.DAL;
using delivery_backend_advanced.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.BL.Services;

public class AdminService : IAdminService
{
    private readonly BackendDbContext _context;

    public AdminService(BackendDbContext context)
    {
        _context = context;
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
        throw new NotImplementedException();
    }

    public async Task DeleteComment(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public async Task BanUser(string email)
    {
        throw new NotImplementedException();
    }

    public async Task UnbanUser(string email)
    {
        throw new NotImplementedException();
    }
}