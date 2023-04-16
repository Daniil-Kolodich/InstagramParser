using Database.Constants;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users").HasKey(v => v.Id);
        builder.Property(v => v.UserName).IsRequired().HasMaxLength(50);
        builder.Property(v => v.Email).IsRequired().HasMaxLength(50);
        builder.Property(v => v.Password).IsRequired().HasMaxLength(1000);

        builder.HasMany<ParsingRequest>(u => u.ParsingRequests)
            .WithOne(p => p.User);
    }
}