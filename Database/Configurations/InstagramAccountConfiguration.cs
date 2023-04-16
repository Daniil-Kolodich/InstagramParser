using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class InstagramAccountConfiguration : EntityConfiguration<InstagramAccount>
{
    public override void Configure(EntityTypeBuilder<InstagramAccount> builder)
    {
        base.Configure(builder);

        builder.ToTable("InstagramAccounts").HasKey(e => e.Id);
        builder.Property(e => e.InstagramId).IsRequired();
        builder.Property(e => e.DeclinedReason).HasDefaultValue(null);
        builder.Property(e => e.InstagramAccountType).IsRequired();

        builder.HasOne<Subscription>(e => e.Subscription)
            .WithMany(p => p.InstagramAccounts)
            .HasForeignKey(e => e.SubscriptionId)
            .IsRequired();
    }
}