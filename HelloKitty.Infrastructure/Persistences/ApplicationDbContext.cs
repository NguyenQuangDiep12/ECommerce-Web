using HelloKitty.Domain.Carts.Entities;
using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Inventory.Entities;
using HelloKitty.Domain.Logging.Entities;
using HelloKitty.Domain.Orders.Entities;
using HelloKitty.Domain.Promotions.Entities;
using HelloKitty.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Attribute = HelloKitty.Domain.Catalog.Entities.Attribute;

namespace HelloKitty.Infrastructure.Persistences
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        // User
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<UserSensitve> UserSensitves { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<OAuthAccount> OAuthAccounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Role
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // Catalog
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<VariantAttribute> VariantAttributes { get; set; }

        // Cart
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        
        // Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderAddress> OrderAddresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        // Promotion
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherUsage> VoucherUsages { get; set; }

        // Inventory
        public DbSet<InventoryLog> InventoryLogs { get; set; }
        
        // Logging
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
