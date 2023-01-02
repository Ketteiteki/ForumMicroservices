using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Domain.Entities;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiCommands.Posts;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<PostEntityDto>>
{
    private readonly DatabaseContext _context;

    public CreatePostCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<PostEntityDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var requester = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequesterId, cancellationToken);

        if (requester == null)
        {
            return new Result<PostEntityDto>(new DbEntityNotFoundError("Requester not found"));
        }

        var newPost = new PostEntity(
            ownerId: requester.Id,
            text: request.Text);

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync(cancellationToken);

        var postDto = new PostEntityDto
        {
            Id = newPost.Id,
            OwnerId = requester.Id,
            OwnerNickname = requester.Nickname,
            Text = newPost.Text,
            DateOfCreate = newPost.DateOfCreate,
            DateOfUpdate = newPost.DateOfUpdate
        };
        
        return new Result<PostEntityDto>(postDto);
    }
}