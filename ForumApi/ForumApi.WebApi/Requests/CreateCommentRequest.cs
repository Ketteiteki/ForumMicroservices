namespace ForumApi.WebApi.Requests;

public class CreateCommentRequest
{
    public Guid PostId { get; set; }
    
    public string Text { get; set; }
}