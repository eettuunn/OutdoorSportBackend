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
        CreateMap<CommentEntity, CommentDto>();
        CreateMap<SlotEntity, SlotDto>();
        CreateMap<SlotEntity, UserSlotDto>();
        CreateMap<SignSlotDto, SlotEntity>();
        CreateMap<CreateObjectDto, SportObjectEntity>();
    }
}