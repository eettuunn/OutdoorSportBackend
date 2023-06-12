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

    [HttpPost("{objectId}/rate")]
    [Authorize]
    public async Task RateObject(Guid objectId, int value)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.RateSportObject(objectId, value, email);
    }

    [HttpGet("{objectId}/report")]
    [Authorize]
    public async Task<bool> CheckReportAbility(Guid objectId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        return await _reviewsService.CheckReportAbility(objectId, email);
    }
    
    [HttpPost("{objectId}/report")]
    [Authorize]
    public async Task ReportObject(Guid objectId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.ReportObject(objectId, email);
    }
}