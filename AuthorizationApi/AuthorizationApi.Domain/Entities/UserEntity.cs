namespace AuthorizationApi.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nickname { get; set; }
    
    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }
}