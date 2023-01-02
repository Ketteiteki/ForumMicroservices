using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiCommands.Posts;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<PostEntityDto>>
{
    private readonly DatabaseContext _context;

    public UpdatePostCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<PostEntityDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var requester = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequesterId, cancellationToken);

        if (requester == null)
        {
            return new Result<PostEntityDto>(new DbEntityNotFoundError("Requester not found"));
        }

        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);
        
        if (post == null)
        {
            return new Result<PostEntityDto>(new DbEntityNotFoundError("Post not found"));
        }

        if (requester.Id != post.OwnerId)
        {
            return new Result<PostEntityDto>(new ForbiddenError("It is forbidden to update someone else's post"));
        }

        post.Text = request.Text.Trim();
        post.DateOfUpdate = DateTime.UtcNow;

        _context.Posts.Update(post);
        await _context.SaveChangesAsync(cancellationToken);

        var postDto = new PostEntityDto
        {
            Id = post.Id,
            OwnerId = requester.Id,
            OwnerNickname = requester.Nickname,
            Text = post.Text,
            DateOfCreate = post.DateOfCreate,
            DateOfUpdate = post.DateOfUpdate
        };
            
        return new Result<PostEntityDto>(postDto);
    }
}