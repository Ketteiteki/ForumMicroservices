using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Posts;

public record CreatePostCommand(
        Guid RequesterId,
        string Text
    )
    : IRequest<Result<PostEntityDto>>;