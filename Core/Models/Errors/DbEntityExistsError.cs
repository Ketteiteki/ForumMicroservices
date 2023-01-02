namespace Core.Models.Errors;

public class DbEntityExistsError : Error
{
    public DbEntityExistsError(string message) : base(message)
    {
    }
}