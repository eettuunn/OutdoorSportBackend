using Backend.Common.Dtos;

namespace Backend.Common.Interfaces;

public interface ISlotsService
{
    public Task SignSlot(Guid objectId, SignSlotDto signSlotDto, string email);
}