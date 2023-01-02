using AuthorizationApi.Application.Interfaces;
using AuthorizationApi.Domain.Constants;
using AuthorizationApi.Persistence;
using Core.Events;
using Core.Models;
using Core.Models.Errors;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthorizationApi.BusinessLogic.EventHandlers;

public class LoginEventHandler : IConsumer<LoginEvent>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;
    
    public LoginEventHandler(DatabaseContext databaseContext, IJwtTokenService jwtTokenService, IHashService hashService, IConfiguration configuration)
    {
        _databaseContext = databaseContext;
        _jwtTokenService = jwtTokenService;
        _hashService = hashService;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<LoginEvent> context)
    {
        var requester = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Nickname == context.Message.Nickname);

        if (requester == null)
        {
            await context.RespondAsync(new Result<string>(new AuthorizationError("User don't exists")));
            return;
        }

        var passwordHash = _hashService.HmacSha512CryptoHashWithSalt(context.Message.Password, requester.PasswordSalt);

        if (requester.PasswordHash != passwordHash)
        {
            await context.RespondAsync(new Result<string>(new AuthorizationError("Password is invalid")));
            return;
        }

        var signKey = _configuration[AppSettingsConstants.AccessTokenSecretSignKey];
        
        var token = _jwtTokenService.CreateAccessToken(requester, signKey);

        await context.RespondAsync(new Result<string>(token));
    }
}