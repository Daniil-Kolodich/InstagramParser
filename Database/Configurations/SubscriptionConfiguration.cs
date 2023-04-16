using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class SubscriptionConfiguration : EntityConfiguration<Subscription>
{
    public override void Configure(EntityTypeBuilder<Subscription> builder)
    {
        base.Configure(builder);

        builder.ToTable("Subscriptions").HasKey(e => e.Id);
        builder.Property(e => e.Source).IsRequired();
        builder.Property(e => e.Target).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        
        builder.Property(e => e.UserId).IsRequired();
        builder.HasOne<User>(e => e.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(p => p.UserId)
            .IsRequired();
        builder.HasMany<InstagramAccount>(e => e.InstagramAccounts)
            .WithOne(a => a.Subscription)
            .IsRequired();
    }
}