using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiQueries.Posts;

public record GetPostQuery(
        Guid PostId)
    : IRequest<Result<PostEntityDto>>;