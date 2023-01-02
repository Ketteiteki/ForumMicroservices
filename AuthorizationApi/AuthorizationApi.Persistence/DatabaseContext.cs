using AuthorizationApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationApi.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }
}