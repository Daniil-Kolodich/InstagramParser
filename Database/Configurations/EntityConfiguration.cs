using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(v => v.Id).IsRequired();
        builder.Property(v => v.CreatedWhen).IsRequired();
        builder.Property(v => v.UpdatedWhen).HasDefaultValue(null);
        builder.Property(v => v.IsDeleted).HasDefaultValue(false);
    }
}