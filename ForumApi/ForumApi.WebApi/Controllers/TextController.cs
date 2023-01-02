using AuthorizationApi.Domain.Constants;
using Core.Events;
using ForumApi.WebApi.Filters;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.WebApi.Controllers;

[Route("[controller]")]
[Controller]
public class TextController : ControllerBase
{
    private readonly IRequestClient<ValidateAccessTokenEvent> _validateAccessTokenClient;

    public TextController(IRequestClient<ValidateAccessTokenEvent> validateAccessTokenClient)
    {
        _validateAccessTokenClient = validateAccessTokenClient;
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpGet]
    public async Task<IActionResult> Invoke()
    {
        // var validateAccessTokenEvent = new ValidateAccessTokenEvent(
        //     Token: "ffsdfsd");
        //
        // var validateAccessTokenResult = await _validateAccessTokenClient.GetResponse<Result<JwtSecurityToken>>(validateAccessTokenEvent);
        //
        // Console.Write(validateAccessTokenResult.Message.IsSuccess);
        
        return Ok(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);
    }
}