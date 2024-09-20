using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Orders;
using System.Reflection.Emit;

namespace PersonalShop.Data.EntityConfiguration;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasMany(e => e.OrderItems)
            .WithOne()
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(o => o.UserId)
            .IsUnique(false);

        builder
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<Order>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
