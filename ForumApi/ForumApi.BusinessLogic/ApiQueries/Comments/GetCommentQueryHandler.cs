using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiQueries.Comments;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Result<CommentEntityDto>>
{
    private readonly DatabaseContext _context;

    public GetCommentQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<CommentEntityDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .AsNoTracking()
            .Include(c => c.Owner)
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

        if (comment == null)
        {
            return new Result<CommentEntityDto>(new DbEntityNotFoundError("Comment not found"));
        }

        var commentDto = new CommentEntityDto
        {
            Id = comment.Id,
            OwnerId = comment.OwnerId,
            PostId = comment.PostId,
            OwnerNickname = comment.Owner.Nickname,
            Text = comment.Text,
            DateOfCreate = comment.DateOfCreate,
            DateOfUpdate = comment.DateOfUpdate
        };
        
        return new Result<CommentEntityDto>(commentDto);
    }
}