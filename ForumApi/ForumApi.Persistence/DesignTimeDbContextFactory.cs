using ForumApi.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ForumApi.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private readonly IConfigurationRoot _configuration;

    public DesignTimeDbContextFactory()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "../../../../ForumApi.WebApi/appsettings.Development.json"))
            .Build();
    }
    
    public DatabaseContext CreateDbContext(string[] args)
    {
        var connectionString = _configuration[AppSettingsConstants.ForumDatabaseConnectionString];
        
        var option = new DbContextOptionsBuilder<DatabaseContext>();

        option.UseNpgsql(connectionString);

        return new DatabaseContext(option.Options);
    }
}