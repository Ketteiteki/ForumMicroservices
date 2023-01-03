namespace Core.DTOs;

public class TokenAndId
{
    public string Token { get; set; }

    public Guid Id { get; set; }

    public TokenAndId(string token, Guid id)
    {
        Token = token;
        Id = id;
    }
}