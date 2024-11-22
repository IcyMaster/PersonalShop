using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Products;

namespace PersonalShop.Data.EntityConfiguration;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId);

        //for category many-to-many
        builder
            .HasMany(e => e.Categories)
            .WithMany(e => e.Products);
    }
}
