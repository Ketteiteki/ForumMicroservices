using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Domain.Entities;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiCommands.Comments;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentEntityDto>>
{
    private readonly DatabaseContext _context;

    public CreateCommentCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<CommentEntityDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var requester = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequesterId, cancellationToken);

        if (requester == null)
        {
            return new Result<CommentEntityDto>(new DbEntityNotFoundError("Requester not found"));
        }
        
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return new Result<CommentEntityDto>(new DbEntityNotFoundError("Post not found"));
        }

        var newComment = new CommentEntity(
            ownerId: requester.Id,
            postId: post.Id,
            text: request.Text);

        _context.Comments.Add(newComment);
        await _context.SaveChangesAsync(cancellationToken);

        var commentDto = new CommentEntityDto
        {
            Id = newComment.Id,
            OwnerId = requester.Id,
            PostId = newComment.PostId,
            OwnerNickname = requester.Nickname,
            Text = newComment.Text,
            DateOfCreate = newComment.DateOfCreate,
            DateOfUpdate = newComment.DateOfUpdate 
        };

        return new Result<CommentEntityDto>(commentDto);
    }
}