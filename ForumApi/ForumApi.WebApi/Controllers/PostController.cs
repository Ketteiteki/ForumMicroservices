using AuthorizationApi.Domain.Constants;
using ForumApi.BusinessLogic.ApiCommands.Posts;
using ForumApi.WebApi.Filters;
using ForumApi.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class PostController : ApiControllerBase
{
    public PostController(IMediator mediator) : base(mediator)
    {
    }

    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpPost("createPost")]
    public async Task<IActionResult> CreatePost(CreatePostRequest createPostRequest, CancellationToken cancellationToken)
    {
        var requesterGuid = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);

        var createPostCommand = new CreatePostCommand(
            RequesterId: requesterGuid,
            Text: createPostRequest.Text);

        return await RequestAsync(createPostCommand, cancellationToken);
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpPut("updatePost")]
    public async Task<IActionResult> UpdatePost(UpdatePostRequest updatePostRequest, CancellationToken cancellationToken)
    {
        var requesterGuid = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);

        var updatePostCommand = new UpdatePostCommand(
            RequesterId: requesterGuid,
            PostId: updatePostRequest.PostId,
            Text: updatePostRequest.Text);

        return await RequestAsync(updatePostCommand, cancellationToken);
    }
    
    [TypeFilter(typeof(AuthorizationAttribute))]
    [HttpDelete("deletePost")]
    public async Task<IActionResult> DeletePost([FromQuery] Guid postId, CancellationToken cancellationToken)
    {
        var requesterGuid = new Guid(HttpContext.User.Claims.First(c => c.Type == ClaimConstants.Id).Value);

        var deletePostCommand = new DeletePostCommand(
            RequesterId: requesterGuid,
            PostId: postId);

        return await RequestAsync(deletePostCommand, cancellationToken);
    }
}