using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Posts;

public record DeletePostCommand(
        Guid RequesterId,
        Guid PostId)
    : IRequest<Result<PostEntityDto>>;