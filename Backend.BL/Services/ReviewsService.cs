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
    private readonly IMapper _mapper;

    public ReviewsService(BackendDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
}