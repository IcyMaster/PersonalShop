using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalShop.Data.EntityConfiguration;
using PersonalShop.Domain.Carts;
using PersonalShop.Domain.Categorys;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Roles;
using PersonalShop.Domain.Tags;
using PersonalShop.Domain.Users;

namespace PersonalShop.Data;

public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //cart
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartEntityTypeConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartItemEntityTypeConfiguration).Assembly);

        //order
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityTypeConfiguration).Assembly);

        //category
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryEntityTypeConfiguration).Assembly);

        //product
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

