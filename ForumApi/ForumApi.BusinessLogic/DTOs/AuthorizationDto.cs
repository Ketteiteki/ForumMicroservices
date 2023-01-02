using ForumApi.Domain.Entities;

namespace ForumApi.BusinessLogic.DTOs;

public class AuthorizationDto
{
    public string AccessToken { get; set; }
    
    public Guid Id { get; set; }
    
    public string Nickname { get; set; }

    public AuthorizationDto(string accessToken, UserEntity userEntity)
    {
        AccessToken = accessToken;
        Id = userEntity.Id;
        Nickname = userEntity.Nickname;
    }
}