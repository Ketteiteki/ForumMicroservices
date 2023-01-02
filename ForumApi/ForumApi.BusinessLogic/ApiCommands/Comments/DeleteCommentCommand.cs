using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Comments;

public record DeleteCommentCommand(
        Guid RequesterId,
        Guid CommentId)
    : IRequest<Result<CommentEntityDto>>;