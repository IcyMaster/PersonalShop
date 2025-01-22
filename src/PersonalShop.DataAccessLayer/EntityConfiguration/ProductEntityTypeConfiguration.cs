using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalShop.Domain.Entities.Categorys;
using PersonalShop.Domain.Entities.Products;
using PersonalShop.Domain.Entities.Tags;

namespace PersonalShop.DataAccessLayer.EntityConfiguration;

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
            j => j.HasOne<Category>().WithMany()
            .OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.CategoryId),
            j => j.HasOne<Product>().WithMany()
            .OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.ProductId));

        //for tag many-to-many
        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Products)
            .UsingEntity<ProductTag>(
            j => j.HasOne<Tag>().WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.TagId),
            j => j.HasOne<Product>().WithMany().OnDelete(DeleteBehavior.Cascade).HasForeignKey(e => e.ProductId));
    }
}
