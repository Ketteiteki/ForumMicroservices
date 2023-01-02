using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiQueries.Comments;

public record GetCommentQuery(
        Guid CommentId)
    : IRequest<Result<CommentEntityDto>>;