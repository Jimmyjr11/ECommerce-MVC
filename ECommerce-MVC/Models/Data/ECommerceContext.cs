using ECommerce_MVC.Models.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce_MVC.Models.Data
{
    public class ECommerceContext : IdentityDbContext<AppUser>
    {
        public ECommerceContext() { }
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationships
            ConfigureRelationships(builder);

            // Column Types & Indexes
            ConfigureConstraints(builder);

            // Data Seeding
            SeedData(builder);
        }

        private void ConfigureRelationships(ModelBuilder builder)
        {
            builder.Entity<Category>().HasOne(c => c.ParentCategory).WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>().HasOne(o => o.ShippingAddress).WithMany(a => a.Orders)
                .HasForeignKey(o => o.ShippingAddressId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>().HasOne(oi => oi.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Address>().HasOne(a => a.User).WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureConstraints(ModelBuilder builder)
        {
            builder.Entity<Product>().HasIndex(p => p.SKU).IsUnique();
            builder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            builder.Entity<Order>().HasIndex(o => o.OrderNumber).IsUnique();

            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Entity<OrderItem>().Property(oi => oi.LineTotal).HasColumnType("decimal(18,2)");
        }

        private void SeedData(ModelBuilder builder)
        {
            const string adminRoleId = "1";
            const string customerRoleId = "2";
            const string adminUserId = "100";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "STAMP_1" },
                new IdentityRole { Id = customerRoleId, Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = "STAMP_2" }
            );

            builder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminUserId,
                FullName = "Aly Gamal",
                UserName = "alygamal5@gmail.com",
                NormalizedUserName = "ALYGAMAL5@GMAIL.COM",
                Email = "alygamal5@gmail.com",
                NormalizedEmail = "ALYGAMAL5@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEI9OshVz8yYlB2p8GzC4I7T/S1Zq/Jm5uXfS5z6B7N3w==", // Admin@123
                SecurityStamp = "STATIC_STAMP",
                ConcurrencyStamp = "USER_STAMP"
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }
    }
}