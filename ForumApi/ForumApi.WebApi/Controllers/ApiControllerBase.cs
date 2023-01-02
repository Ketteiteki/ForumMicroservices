using Core.Models;
using Core.Models.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.WebApi.Controllers;

[Controller]
public class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    public ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    [NonAction]
    protected async Task<IActionResult> RequestAsync<TValue>(
        IRequest<Result<TValue>> request, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.Error switch
        {
            AuthorizationError authorizationError =>
                new ObjectResult(new { authorizationError.Message }) { StatusCode = 401 },
            DbEntityNotFoundError dbEntityNotFoundError =>
                new ObjectResult(new { dbEntityNotFoundError.Message }) { StatusCode = 404 },
            DbEntityExistsError dbEntityExistsError =>
                new ObjectResult(new { dbEntityExistsError.Message }) { StatusCode = 409 },
            ForbiddenError forbiddenError =>
                new ObjectResult(new { forbiddenError.Message }) { StatusCode = 403 },

            _ => new ObjectResult(result.Value) { StatusCode = 200 }
        };
    }
}