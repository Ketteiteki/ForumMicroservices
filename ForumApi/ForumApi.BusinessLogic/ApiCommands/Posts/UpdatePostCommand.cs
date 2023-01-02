using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Posts;

public record UpdatePostCommand(
        Guid RequesterId,
        Guid PostId,
        string Text)
    : IRequest<Result<PostEntityDto>>;