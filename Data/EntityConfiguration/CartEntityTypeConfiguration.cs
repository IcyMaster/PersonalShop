using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Card;

namespace PersonalShop.Data.EntityConfiguration;

public class CartEntityTypeConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasMany(e => e.CartItems)
            .WithOne(e => e.Cart)
            .HasForeignKey(e => e.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
