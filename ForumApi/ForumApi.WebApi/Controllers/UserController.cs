using AuthorizationApi.Domain.Constants;
using ForumApi.BusinessLogic.ApiCommands.Users;
using ForumApi.BusinessLogic.ApiQueries.Users;
using ForumApi.WebApi.Filters;
using ForumApi.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ApiControllerBase
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        var getUserQuery = new GetUserQuery(
            UserId: userId);

        return await RequestAsync(getUserQuery, cancellationToken);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationRequest registrationRequest,
        CancellationToken cancellationToken)
    {
        var registrationCommand = new RegistrationCommand(
            Nickname: registrationRequest.Nickname,
            Password: registrationRequest.Password);

        return await RequestAsync(registrationCommand, cancellationToken);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var loginQuery = new LoginQuery(
            Nickname: loginRequest.Nickname,
            Password: loginRequest.Password);

        return await RequestAsync(loginQuery, cancellationToken);
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpDelete("deleteAccount")]
    public async Task<IActionResult> DeleteAccount(CancellationToken cancellationToken)
    {
        var requesterId = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);
        
        var deleteAccountCommand = new DeleteAccountCommand(
            RequesterId: requesterId);

        return await RequestAsync(deleteAccountCommand, cancellationToken);
    }
}