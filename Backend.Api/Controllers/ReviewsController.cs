using System.Security.Claims;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Common.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutdoorSportBackend.Controllers;

[Route("api/back/reviews")]
[Authorize]
[Authorize(Policy = PolicyNames.Ban)]
public class ReviewsController : ControllerBase
{
    private readonly IReviewsService _reviewsService;

    public ReviewsController(IReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpPost("{objectId}")]
    public async Task<IActionResult> LeaveComment(Guid objectId, [FromBody] CreateCommentDto createCommentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.LeaveComment(objectId, createCommentDto, email);
        return Ok();
    }
    
    [HttpDelete("{commentId}")]
    public async Task DeleteComment(Guid commentId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.DeleteComment(commentId, email);
    }
    
    [HttpPut("{commentId}")]
    public async Task<IActionResult> EditComment(Guid commentId, [FromBody] EditCommentDto editCommentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.EditComment(commentId, editCommentDto, email);
        return Ok();
    }

    [HttpPost("{objectId}/rate")]
    public async Task RateObject(Guid objectId, int value)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.RateSportObject(objectId, value, email);
    }

    [HttpGet("{objectId}/report")]
    public async Task<bool> CheckReportAbility(Guid objectId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        return await _reviewsService.CheckReportAbility(objectId, email);
    }
    
    [HttpPost("{objectId}/report")]
    public async Task ReportObject(Guid objectId)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        await _reviewsService.ReportObject(objectId, email);
    }
}