using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiCommands.Comments;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<CommentEntityDto>>
{
    private readonly DatabaseContext _context;

    public DeleteCommentCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<CommentEntityDto>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var requester = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequesterId, cancellationToken);

        if (requester == null)
        {
            return new Result<CommentEntityDto>(new DbEntityNotFoundError("Requester not found"));
        }

        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);
        
        if (comment == null)
        {
            return new Result<CommentEntityDto>(new DbEntityNotFoundError("Comment not found"));
        }

        if (requester.Id != comment.OwnerId)
        {
            return new Result<CommentEntityDto>(new ForbiddenError("It is forbidden to delete someone else's comment"));
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);

        var commentDto = new CommentEntityDto
        {
            Id = comment.Id,
            OwnerId = requester.Id,
            PostId = comment.PostId,
            OwnerNickname = requester.Nickname,
            Text = comment.Text,
            DateOfCreate = comment.DateOfCreate,
            DateOfUpdate = comment.DateOfUpdate 
        };

        return new Result<CommentEntityDto>(commentDto);
    }
}