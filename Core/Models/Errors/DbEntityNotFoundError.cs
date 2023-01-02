namespace Core.Models.Errors;

public class DbEntityNotFoundError : Error
{
    public DbEntityNotFoundError(string message) : base(message)
    {
    }
}