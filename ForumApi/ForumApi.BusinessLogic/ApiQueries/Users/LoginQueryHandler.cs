using Core.Events;
using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiQueries.Users;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthorizationDto>>
{
    private readonly DatabaseContext _context;
    private readonly IRequestClient<LoginEvent> _loginClient;
    private readonly IRequestClient<ValidateAccessTokenEvent> _validateAccessTokenClient;

    public LoginQueryHandler(DatabaseContext context, IRequestClient<LoginEvent> loginClient, 
        IRequestClient<ValidateAccessTokenEvent> validateAccessTokenClient)
    {
        _context = context;
        _loginClient = loginClient;
        _validateAccessTokenClient = validateAccessTokenClient;
    }

    public async Task<Result<AuthorizationDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var loginEvent = new LoginEvent(
            Nickname: request.Nickname,
            Password: request.Password);

        var loginResult = await _loginClient.GetResponse<Result<string>>(loginEvent, cancellationToken);

        if (loginResult.Message.IsSuccess == false)
        {
            return new Result<AuthorizationDto>(new AuthorizationError(loginResult.Message.Error.Message));
        }

        var accessToken = loginResult.Message.Value;
        
        var validateAccessTokenEvent = new ValidateAccessTokenEvent(
            Token: accessToken);
        
        var validateAccessTokenResult = _validateAccessTokenClient
            .GetResponse<Result<Guid>>(validateAccessTokenEvent, cancellationToken);

        var userId = validateAccessTokenResult.Result.Message.Value;
        
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            return new Result<AuthorizationDto>(new AuthorizationError("User not found"));
        }
        
        var authorizationDto = new AuthorizationDto(
            accessToken: accessToken, 
            userEntity: user);

        return new Result<AuthorizationDto>(authorizationDto);
    }
}