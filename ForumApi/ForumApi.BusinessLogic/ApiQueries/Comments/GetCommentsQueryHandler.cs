using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiQueries.Comments;

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Result<List<CommentEntityDto>>>
{
    private readonly DatabaseContext _context;

    public GetCommentsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CommentEntityDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        if (request.DateOfStart == null)
        {
            var comments = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Owner)
                .OrderByDescending(c => c.DateOfCreate)
                .Where(c => c.PostId == request.PostId)
                .Take(request.Limit)
                .Select(c => new CommentEntityDto
                {
                    Id = c.Id,
                    OwnerId = c.OwnerId,
                    OwnerNickname = c.Owner.Nickname,
                    Text = c.Text,
                    DateOfCreate = c.DateOfCreate,
                    DateOfUpdate = c.DateOfUpdate
                })
                .ToListAsync(cancellationToken);

            return new Result<List<CommentEntityDto>>(comments);
        }
        
        var commentsWithStartDate = await _context.Comments
            .AsNoTracking()
            .Include(c => c.Owner)
            .OrderByDescending(c => c.DateOfCreate)
            .Where(c => c.PostId == request.PostId && 
                        c.DateOfCreate < request.DateOfStart)
            .Take(request.Limit)
            .Select(c => new CommentEntityDto
            {
                Id = c.Id,
                OwnerId = c.OwnerId,
                PostId = c.PostId,
                OwnerNickname = c.Owner.Nickname,
                Text = c.Text,
                DateOfCreate = c.DateOfCreate,
                DateOfUpdate = c.DateOfUpdate
            })
            .ToListAsync(cancellationToken);

        return new Result<List<CommentEntityDto>>(commentsWithStartDate);
    }
}