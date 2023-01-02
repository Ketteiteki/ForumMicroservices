using System.IdentityModel.Tokens.Jwt;
using AuthorizationApi.Application.Interfaces;
using AuthorizationApi.Domain.Constants;
using Core.Events;
using Core.Models;
using Core.Models.Errors;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace AuthorizationApi.BusinessLogic.EventHandlers;

public class ValidateAccessTokenEventHandler : IConsumer<ValidateAccessTokenEvent>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;

    public ValidateAccessTokenEventHandler(IJwtTokenService jwtTokenService, IConfiguration configuration)
    {
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<ValidateAccessTokenEvent> context)
    {
        var signKey = _configuration[AppSettingsConstants.AccessTokenSecretSignKey];
        
        if (_jwtTokenService.TryValidateAccessToken(context.Message.Token, signKey, out var validatedJwtToken))
        {
            var requesterGuid = new Guid(validatedJwtToken.Claims.First(c => c.Type == ClaimConstants.Id).Value);
            
            await context.RespondAsync(new Result<Guid>(requesterGuid));
            return;
        }
        
        await context.RespondAsync(new Result<JwtSecurityToken>(new AuthorizationError("Invalid token")));
    }
}