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
        if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var bearerToken) == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (bearerToken.ToString().Split(" ").FirstOrDefault("Bearer") == null ||
            bearerToken.ToString().Split(" ").Length != 2)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var token = bearerToken.ToString().Split(" ")[1];
        
        var validateAccessTokenEvent = new ValidateAccessTokenEvent(
            Token: token);
        
        var validateAccessTokenResult = await _validateAccessTokenClient.GetResponse<Result<Guid>>(validateAccessTokenEvent);

        if (validateAccessTokenResult.Message.IsSuccess == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var requesterGuid = validateAccessTokenResult.Message.Value;

        var claims = new List<Claim> { new (ClaimConstants.Id, requesterGuid.ToString()) };
        
        var claimsIdentity = new ClaimsIdentity(claims);
        
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        context.HttpContext.User = claimsPrincipal;
    }
}
