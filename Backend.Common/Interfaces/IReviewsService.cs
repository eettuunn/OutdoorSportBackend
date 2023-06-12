using Backend.Common.Dtos;

namespace Backend.Common.Interfaces;

public interface IReviewsService
{
    public Task LeaveComment(Guid objectId, CreateCommentDto createCommentDto, string email);
    public Task DeleteComment(Guid commentId, string email);
    public Task EditComment(Guid commentId, EditCommentDto editCommentDto, string email);
    public Task RateSportObject(Guid objectId, int value, string email);
    public Task<bool> CheckReportAbility(Guid objectId, string email);
    public Task ReportObject(Guid objectId, string email);
}