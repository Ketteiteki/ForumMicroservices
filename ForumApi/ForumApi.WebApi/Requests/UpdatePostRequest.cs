namespace ForumApi.WebApi.Requests;

public class UpdatePostRequest
{
    public Guid PostId { get; set; }
    
    public string Text { get; set; }
}