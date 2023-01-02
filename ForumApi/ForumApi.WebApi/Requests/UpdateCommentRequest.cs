namespace ForumApi.WebApi.Requests;

public class UpdateCommentRequest
{
    public Guid CommentId { get; set; }
    
    public string Text { get; set; }
}