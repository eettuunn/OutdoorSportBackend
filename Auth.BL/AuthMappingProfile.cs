using Auth.Common.Dtos;
using Auth.DAL.Entities;
using AutoMapper;

namespace Auth.BL;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AppUser, TokenUserDto>();
        CreateMap<RegisterDto, AppUser>();
    }
}