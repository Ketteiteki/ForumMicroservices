using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiQueries.Users;

public record LoginQuery(
        string Nickname,
        string Password)
    : IRequest<Result<AuthorizationDto>>;