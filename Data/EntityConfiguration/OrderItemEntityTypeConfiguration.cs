using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Card;
using PersonalShop.Domain.Orders;

namespace PersonalShop.Data.EntityConfiguration
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(e => e.Product)
                .WithOne()
                .HasForeignKey<OrderItem>(e => e.ProductId).IsRequired();
        }
    }
}
