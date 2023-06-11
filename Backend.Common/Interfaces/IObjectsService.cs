using Backend.Common.Dtos;
using Backend.Common.Enums;

namespace Backend.Common.Interfaces;

public interface IObjectsService
{
    public Task<List<ObjectListDto>> GetObjectsList(SportType? type);
}