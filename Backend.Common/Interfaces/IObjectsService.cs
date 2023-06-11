using Backend.Common.Dtos;
using Backend.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Backend.Common.Interfaces;

public interface IObjectsService
{
    public Task<List<ObjectListDto>> GetObjectsList(SportType? type);
    public Task<ObjectDto> GetObjectDetails(Guid id);
    public Task CreateObject(CreateObjectDto createObjectDto, string email);
    public Task EditObject(Guid id, EditObjectDto editObjectDto, string email);
    public Task DeleteObject(Guid id, string email);
    public Task AddObjectPhotos(Guid id, List<IFormFile> photos);
}