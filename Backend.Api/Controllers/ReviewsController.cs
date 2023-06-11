using System.Security.Claims;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewsService _reviewsService;

    public ReviewsController(IReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpPost("{objectId}")]
    [Authorize]
    public async Task LeaveComment(Guid objectId, [FromBody] CreateCommentDto createCommentDto)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.LeaveComment(objectId, createCommentDto, email);
    }
    
    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task DeleteComment(Guid commentId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.DeleteComment(commentId, email);
    }
    
    [HttpPut("{commentId}")]
    [Authorize]
    public async Task EditComment(Guid commentId, [FromBody] EditCommentDto editCommentDto)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.EditComment(commentId, editCommentDto, email);
    }
}