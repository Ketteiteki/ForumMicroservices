namespace Core.Models.Errors;

public class ForbiddenError : Error
{
    public ForbiddenError(string message) : base(message)
    {
    }
}