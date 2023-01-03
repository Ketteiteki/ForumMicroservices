using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Users;

public record DeleteAccountCommand(
    Guid RequesterId)
    : IRequest<Result<UserEntityDto>>;