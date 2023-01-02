using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Users;

public record RegistrationCommand(
        string Nickname,
        string Password)
    : IRequest<Result<AuthorizationDto>>;