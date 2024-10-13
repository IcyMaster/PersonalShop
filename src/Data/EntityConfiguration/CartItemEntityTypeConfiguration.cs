using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Card;

namespace PersonalShop.Data.EntityConfiguration
{
    public class CartItemEntityTypeConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(e => e.Product)
                .WithOne()
                .HasForeignKey<CartItem>(e => e.ProductId).IsRequired();
        }
    }
}
