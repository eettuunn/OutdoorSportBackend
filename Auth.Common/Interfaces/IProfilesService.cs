using Auth.Common.Dtos;

namespace Auth.Common.Interfaces;

public interface IProfilesService
{
    public Task<ProfileDto> GetProfile(string email);

    public Task<ProfileDto> GetProfileByEmail(string email);
}