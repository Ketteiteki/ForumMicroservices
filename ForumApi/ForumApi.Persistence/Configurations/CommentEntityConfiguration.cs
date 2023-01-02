using ForumApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumApi.Persistence.Configurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Text).IsRequired();
        builder.Property(x => x.PostId).IsRequired();
        builder.Property(x => x.DateOfCreate).IsRequired();
    }
}