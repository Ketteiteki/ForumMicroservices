using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Comments;

public record CreateCommentCommand(
        Guid RequesterId,
        Guid PostId,
        string Text)
    : IRequest<Result<CommentEntityDto>>;