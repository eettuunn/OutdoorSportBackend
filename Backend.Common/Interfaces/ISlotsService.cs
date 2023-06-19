using Backend.Common.Dtos;

namespace Backend.Common.Interfaces;

public interface ISlotsService
{
    public Task SignSlot(Guid objectId, SignSlotDto signSlotDto, string email);
    public Task EditSlot(Guid slotId, EditSlotDto editSlotDto, string email);
    public Task DeleteSlot(Guid slotId, string email);
    public Task<List<SlotDto>> GetMySlots(string email);
}