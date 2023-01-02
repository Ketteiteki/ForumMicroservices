using ForumApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumApi.Persistence.Configurations;

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Text).IsRequired();
        builder.Property(x => x.OwnerId).IsRequired();
        builder.Property(x => x.DateOfCreate).IsRequired();
    }
}