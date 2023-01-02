using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiQueries.Posts;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Result<List<PostEntityDto>>>
{
    private readonly DatabaseContext _context;

    public GetPostsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<List<PostEntityDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        if (request.DateOfStart == null)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Include(c => c.Owner)
                .OrderByDescending(c => c.DateOfCreate)
                .Take(request.Limit)
                .Select(c => new PostEntityDto
                {
                    Id = c.Id,
                    OwnerId = c.OwnerId,
                    OwnerNickname = c.Owner.Nickname,
                    Text = c.Text,
                    DateOfCreate = c.DateOfCreate,
                    DateOfUpdate = c.DateOfUpdate
                })
                .ToListAsync(cancellationToken);

            return new Result<List<PostEntityDto>>(posts);
        }
        
        var postsWithStartDate = await _context.Comments
            .AsNoTracking()
            .Include(c => c.Owner)
            .OrderByDescending(c => c.DateOfCreate)
            .Where(c => c.DateOfCreate < request.DateOfStart)
            .Take(request.Limit)
            .Select(c => new PostEntityDto
            {
                Id = c.Id,
                OwnerId = c.OwnerId,
                OwnerNickname = c.Owner.Nickname,
                Text = c.Text,
                DateOfCreate = c.DateOfCreate,
                DateOfUpdate = c.DateOfUpdate
            })
            .ToListAsync(cancellationToken);

        return new Result<List<PostEntityDto>>(postsWithStartDate);
    }
}