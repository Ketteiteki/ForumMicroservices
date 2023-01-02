using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiQueries.Users;

public record GetUserQuery(
        Guid UserId)
    : IRequest<Result<UserEntityDto>>;