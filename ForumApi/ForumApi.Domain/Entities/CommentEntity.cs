namespace ForumApi.Domain.Entities;

public class CommentEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid OwnerId { get; set; }
    
    public UserEntity Owner { get; set; }
    
    public Guid PostId { get; set; }
    
    public PostEntity Post { get; set; }
    
    public string Text { get; set; }
    
    public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;
    
    public DateTime? DateOfUpdate { get; set; }

    public CommentEntity(Guid ownerId, Guid postId, string text)
    {
        OwnerId = ownerId;
        PostId = postId;
        Text = text;
    }
}