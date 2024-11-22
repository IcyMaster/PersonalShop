using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Categorys;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Tags;

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
            .WithMany(e => e.Products)
            .UsingEntity<ProductCategory>(
            j => j.HasOne<Category>().WithMany().HasForeignKey(e => e.CategoryId),
            j => j.HasOne<Product>().WithMany().HasForeignKey(e => e.ProductId));

        //for tag many-to-many
        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Products)
            .UsingEntity<ProductTag>(
            j => j.HasOne<Tag>().WithMany().HasForeignKey(e => e.TagId),
            j => j.HasOne<Product>().WithMany().HasForeignKey(e => e.ProductId));
    }
}
