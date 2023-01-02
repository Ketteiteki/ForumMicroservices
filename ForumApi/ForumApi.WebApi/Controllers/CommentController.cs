using AuthorizationApi.Domain.Constants;
using ForumApi.BusinessLogic.ApiCommands.Comments;
using ForumApi.WebApi.Filters;
using ForumApi.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CommentController : ApiControllerBase
{
    public CommentController(IMediator mediator) : base(mediator)
    {
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpPost("createComment")]
    public async Task<IActionResult> CreateComment(CreateCommentRequest createCommentRequest, CancellationToken cancellationToken)
    {
        var requesterGuid = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);

        var createCommentCommand = new CreateCommentCommand(
            RequesterId: requesterGuid,
            PostId: createCommentRequest.PostId,
            Text: createCommentRequest.Text);

        return await RequestAsync(createCommentCommand, cancellationToken);
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpPut("updateComment")]
    public async Task<IActionResult> UpdateComment(UpdateCommentRequest updateCommentRequest, CancellationToken cancellationToken)
    {
        var requesterGuid = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);

        var updateCommentCommand = new UpdateCommentCommand(
            RequesterId: requesterGuid,
            CommentId: updateCommentRequest.CommentId,
            Text: updateCommentRequest.Text);

        return await RequestAsync(updateCommentCommand, cancellationToken);
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpDelete("deleteComment")]
    public async Task<IActionResult> DeleteComment([FromQuery] Guid commentId, CancellationToken cancellationToken)
    {
        var requesterGuid = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);

        var deletePostCommand = new DeleteCommentCommand(
            RequesterId: requesterGuid,
            CommentId: commentId);

        return await RequestAsync(deletePostCommand, cancellationToken);
    }
}