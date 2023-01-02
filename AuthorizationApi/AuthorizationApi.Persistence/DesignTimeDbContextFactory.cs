using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthorizationApi.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var option = new DbContextOptionsBuilder<DatabaseContext>();

        option.UseNpgsql("Server=localhost;User Id=postgres;Password=postgres;Database=AuthorizationDev;");

        return new DatabaseContext(option.Options);
    }
}