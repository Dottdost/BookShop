using BookShop.ADMIN.ModelsAdmin;
using BookShop.Auth.ModelsAuth;
using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> ops) : base(ops)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PreOrder> PreOrders { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Admin> Admins { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}