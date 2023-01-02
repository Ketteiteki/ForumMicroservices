namespace Core.Models.Errors;

public class AuthorizationError : Error
{
    public AuthorizationError(string message) : base(message)
    {
    }
}