using AutoMapper;
using Backend.Common.Dtos;
using Backend.Common.Interfaces;
using Backend.DAL;
using Backend.DAL.Entities;
using delivery_backend_advanced.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.BL.Services;

public class ReviewsService : IReviewsService
{
    private readonly BackendDbContext _context;
    private readonly IMessageProducer _messageProducer;

    public ReviewsService(BackendDbContext context, IMessageProducer messageProducer)
    {
        _context = context;
        _messageProducer = messageProducer;
    }
    
    public async Task LeaveComment(Guid objectId, CreateCommentDto createCommentDto, string email)
    {
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email);
        var sportObject = await _context
            .SportObjects
            .FirstOrDefaultAsync(so => so.Id == objectId) ?? throw new CantFindByIdException("sport object", objectId);
        
        var newComment = new CommentEntity
        {
            Id = new Guid(),
            Text = createCommentDto.text,
            User = user,
            SportObject = sportObject
        };

        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid commentId, string email)
    {
        var comment = await _context
            .Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId) ?? throw new CantFindByIdException("comment", commentId);
        if (comment.User.Email != email) throw new ForbiddenException("You can't delete not your comment");
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task EditComment(Guid commentId, EditCommentDto editCommentDto, string email)
    {
        var comment = await _context
            .Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId) ?? throw new CantFindByIdException("comment", commentId);
        if (comment.User.Email != email) throw new ForbiddenException("You can't edit not your comment");

        comment.Text = editCommentDto.text ?? comment.Text;

        await _context.SaveChangesAsync();
    }

    public async Task RateSportObject(Guid objectId, int value, string email)
    {
        if (value is > 10 or < 1) throw new BadRequestException("Value must be in range from 1 to 10");
        var sportObj = await _context
            .SportObjects
            .Include(so => so.Ratings)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(so => so.Id == objectId) ?? throw new CantFindByIdException("sport object", objectId);
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email);
        var userRating = sportObj.Ratings.FirstOrDefault(r => r.User == user);
        
        if (userRating != null)
        {
            sportObj.AverageRating = (double) (sportObj.Ratings.Sum(r => r.Value) - userRating.Value + value) / sportObj.Ratings.Count;
            userRating.Value = value;
        }
        else
        {
            if (sportObj.Ratings.Count == 0)
            {
                sportObj.AverageRating = value;
            }
            else
            {
                var sum = sportObj.Ratings.Sum(r => r.Value) + value;
                var av = (double) sum / (sportObj.Ratings.Count + 1);
                sportObj.AverageRating = av;
            }
            var newRating = new RatingEntity
            {
                Id = new Guid(),
                User = user,
                SportObject = sportObj,
                Value = value
            };
            await _context.Ratings.AddAsync(newRating);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckReportAbility(Guid objectId, string email)
    {
        var sportObj = await _context
            .SportObjects
            .Include(so => so.User)
            .FirstOrDefaultAsync(so => so.Id == objectId) ?? throw new CantFindByIdException("sport object", objectId);
        var report = await _context
            .Reports
            .FirstOrDefaultAsync(r => r.User == sportObj.User && r.SportObject == sportObj);
        
        return report == null;
    }

    public async Task ReportObject(Guid objectId, string email)
    {
        if (await CheckReportAbility(objectId, email))
        {
            var sportObj = await _context
                .SportObjects
                .Include(so => so.User)
                .FirstOrDefaultAsync(so => so.Id == objectId) ?? throw new CantFindByIdException("sport object", objectId);
            var newReport = new ReportEntity
            {
                Id = new Guid(),
                SportObject = sportObj,
                User = sportObj.User
            };
            
            _messageProducer.SendMessage(sportObj.User.Email);

            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ConflictException("You can't report this sport object");
        }
    }
}