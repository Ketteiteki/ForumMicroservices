using AuthorizationApi.Application.Interfaces;
using AuthorizationApi.Domain.Constants;
using AuthorizationApi.Domain.Entities;
using AuthorizationApi.Persistence;
using Core.DTOs;
using Core.Events;
using Core.Models;
using Core.Models.Errors;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthorizationApi.BusinessLogic.EventHandlers;

public class RegistrationEventHandler : IConsumer<RegistrationEvent>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;

    public RegistrationEventHandler(DatabaseContext context, IJwtTokenService jwtTokenService, IHashService hashService,
        IConfiguration configuration)
    {
        _databaseContext = context;
        _jwtTokenService = jwtTokenService;
        _hashService = hashService;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<RegistrationEvent> context)
    {
        var requester = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Nickname == context.Message.Nickname);

        if (requester != null)
        {
            await context.RespondAsync(new Result<TokenAndId>(new AuthorizationError("User with this nickname already exists")));
            return;
        }

        var passwordHash = _hashService.HmacSha512CryptoHash(context.Message.Password, out var salt);
        
        var newUser = new UserEntity
        {
            Nickname = context.Message.Nickname,
            PasswordHash = passwordHash,
            PasswordSalt = salt
        };

        _databaseContext.Users.Add(newUser);
        await _databaseContext.SaveChangesAsync();

        var token = _jwtTokenService.CreateAccessToken(newUser, _configuration[AppSettingsConstants.AccessTokenSecretSignKey]);

        var tokenAndGuid = new TokenAndId(
            token: token,
            id: newUser.Id);
        
        await context.RespondAsync(new Result<TokenAndId>(tokenAndGuid));
    }
}