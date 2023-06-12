namespace Backend.Common.Interfaces;

public interface IAdminService
{
    public Task DeleteSportObject(Guid objectId);
    public Task DeleteSlot(Guid slotId);
    public Task DeleteComment(Guid commentId);
    public Task BanUser(string email);
    public Task UnbanUser(string email);
}