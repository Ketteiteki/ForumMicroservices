using System.Security.Claims;
using AuthorizationApi.Domain.Constants;
using Core.Events;
using Core.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForumApi.WebApi.Filters;

public class AuthorizationAttribute : ActionFilterAttribute, IAsyncAuthorizationFilter
{
    private readonly IRequestClient<ValidateAccessTokenEvent> _validateAccessTokenClient;

    public AuthorizationAttribute(IRequestClient<ValidateAccessTokenEvent> validateAccessTokenClient)
    {
        _validateAccessTokenClient = validateAccessTokenClient;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token) == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var validateAccessTokenEvent = new ValidateAccessTokenEvent(
            Token: token);
        
        var validateAccessTokenResult = await _validateAccessTokenClient.GetResponse<Result<Guid>>(validateAccessTokenEvent);
        
        var requesterGuid = validateAccessTokenResult.Message.Value;

        var claims = new List<Claim> { new (ClaimConstants.Id, requesterGuid.ToString()) };
        
        // var validateAccessTokenResult = await _validateAccessTokenClient.GetResponse<Result<JwtSecurityToken>>(validateAccessTokenEvent);
        //
        // if (validateAccessTokenResult.Message.IsSuccess)
        // {
        //     context.Result = new UnauthorizedResult();
        //     return;
        // }
        //
        // var claims = validateAccessTokenResult.Message.Value.Claims;
        //
        
        var claimsIdentity = new ClaimsIdentity(claims);
        
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        context.HttpContext.User = claimsPrincipal;
    }
}
