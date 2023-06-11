using AutoMapper;
using Backend.Common.Dtos;
using Backend.DAL.Entities;

namespace Backend.BL;

public class BackMappingProfile : Profile
{
    public BackMappingProfile()
    {
        CreateMap<SportObjectEntity, ObjectListDto>();
        CreateMap<SportObjectEntity, ObjectDto>();
    }
}