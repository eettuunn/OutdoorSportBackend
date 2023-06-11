using Backend.Common.Dtos;

namespace Backend.Common.Interfaces;

public interface IReviewsService
{
    public Task LeaveComment(Guid objectId, CreateCommentDto createCommentDto, string email);
    public Task DeleteComment(Guid commentId, string email);
    public Task EditComment(Guid commentId, EditCommentDto editCommentDto, string email);
}