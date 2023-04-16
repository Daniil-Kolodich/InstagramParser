using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class ParsingRequestConfiguration : EntityConfiguration<ParsingRequest>
{
    public new void Configure(EntityTypeBuilder<ParsingRequest> builder)
    {
        base.Configure(builder);

        builder.ToTable("ParsingRequests").HasKey(e => e.Id);
        builder.Property(e => e.ParseFrom).IsRequired();
        builder.Property(e => e.ParseTo).IsRequired();
        builder.Property(e => e.ParsingStatus).IsRequired();
        builder.Property(e => e.ParsingType).IsRequired();
        
        builder.Property(e => e.UserId).IsRequired();
        builder.HasOne<User>(e => e.User)
            .WithMany(u => u.ParsingRequests)
            .HasForeignKey(p => p.UserId)
            .IsRequired();
    }
}