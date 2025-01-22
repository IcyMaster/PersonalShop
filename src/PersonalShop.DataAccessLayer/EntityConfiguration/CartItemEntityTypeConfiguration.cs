using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Entities.Carts;

namespace PersonalShop.DataAccessLayer.EntityConfiguration
{
    public class CartItemEntityTypeConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.CartId);

            builder.HasOne(x => x.Product)
                .WithOne()
                .HasForeignKey<CartItem>(x => x.ProductId).IsRequired();
        }
    }
}
