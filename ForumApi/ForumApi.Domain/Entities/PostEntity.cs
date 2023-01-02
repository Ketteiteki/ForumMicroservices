namespace ForumApi.Domain.Entities;

public class PostEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OwnerId { get; set; }
    
    public UserEntity Owner { get; set; }
    
    public string Text { get; set; }
    
    public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;
    
    public DateTime? DateOfUpdate { get; set; }
    
    public List<CommentEntity> Comments { get; set; }

    public PostEntity(Guid ownerId, string text)
    {
        OwnerId = ownerId;
        Text = text;
    }
}