using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiQueries.Posts;

public record GetPostsQuery(
        int Limit,
        DateTime? DateOfStart)
    : IRequest<Result<List<PostEntityDto>>>;