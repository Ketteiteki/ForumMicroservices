using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiQueries.Comments;

public record GetCommentsQuery(
        Guid PostId,
        int Limit,
        DateTime? DateOfStart)
    : IRequest<Result<List<CommentEntityDto>>>;