using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Users;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Card;
using PersonalShop.Data.EntityConfiguration;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Roles;

namespace PersonalShop.Data;

public class ApplicationDbContext : IdentityDbContext<User,UserRole,string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
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

        base.OnModelCreating(modelBuilder);
    }
}

