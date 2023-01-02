namespace ForumApi.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Nickname { get; set; }
    
    public List<PostEntity> Posts { get; set; }

    public List<CommentEntity> Comments { get; set; }

    public UserEntity(Guid id,string nickname)
    {
        Id = id;
        Nickname = nickname;
    }
}