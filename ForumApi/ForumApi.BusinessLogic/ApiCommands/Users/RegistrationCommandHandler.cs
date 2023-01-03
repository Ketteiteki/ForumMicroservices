using Core.DTOs;
using Core.Events;
using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Domain.Entities;
using ForumApi.Persistence;
using MassTransit;
using MediatR;

namespace ForumApi.BusinessLogic.ApiCommands.Users;

public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, Result<AuthorizationDto>>
{
    private readonly DatabaseContext _context;
    private readonly IRequestClient<RegistrationEvent> _registrationClient;
    private readonly IRequestClient<ValidateAccessTokenEvent> _validateAccessTokenClient;

    public RegistrationCommandHandler(IRequestClient<RegistrationEvent> registrationClient, DatabaseContext context,
        IRequestClient<ValidateAccessTokenEvent> validateAccessTokenClient)
    {
        _registrationClient = registrationClient;
        _context = context;
        _validateAccessTokenClient = validateAccessTokenClient;
    }

    public async Task<Result<AuthorizationDto>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var registrationEvent = new RegistrationEvent(
            Nickname: request.Nickname,
            Password: request.Password);
        
        var registrationResult = await _registrationClient.GetResponse<Result<TokenAndId>>(registrationEvent, cancellationToken);

        if (registrationResult.Message.IsSuccess == false)
        {
            return new Result<AuthorizationDto>(new AuthorizationError(registrationResult.Message.Error.Message));
        }

        var accessToken = registrationResult.Message.Value.Token;
        var newUserId = registrationResult.Message.Value.Id;

        var newUser = new UserEntity(
            id: newUserId,
            nickname: request.Nickname);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync(cancellationToken);

        var authorizationDto = new AuthorizationDto(
            accessToken: accessToken,
            userEntity: newUser);

        return new Result<AuthorizationDto>(authorizationDto);
    }
}