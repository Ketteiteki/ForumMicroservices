using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Comments;

public record UpdateCommentCommand(
        Guid RequesterId,
        Guid CommentId,
        string Text)
    : IRequest<Result<CommentEntityDto>>;