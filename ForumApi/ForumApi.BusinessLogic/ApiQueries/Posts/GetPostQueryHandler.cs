using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiQueries.Posts;

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostEntityDto>>
{
    private readonly DatabaseContext _context;

    public GetPostQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<PostEntityDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

        if (post == null)
        {
            return new Result<PostEntityDto>(new DbEntityNotFoundError("Post not found"));
        }

        var postDto = new PostEntityDto
        {
            Id = post.Id,
            OwnerId = post.OwnerId,
            OwnerNickname = post.Owner.Nickname,
            Text = post.Text,
            DateOfCreate = post.DateOfCreate,
            DateOfUpdate = post.DateOfUpdate
        };

        return new Result<PostEntityDto>(postDto);
    }
}