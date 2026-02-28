using HelloKitty.API.Domain.Carts.Entities;
using HelloKitty.API.Domain.Catalog.Entities;
using HelloKitty.API.Domain.Inventory.Entities;
using HelloKitty.API.Domain.Logging.Entities;
using HelloKitty.API.Domain.Orders.Entities;
using HelloKitty.API.Domain.Promotions.Entities;
using HelloKitty.API.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Attribute = HelloKitty.API.Domain.Catalog.Entities.Attribute;

namespace HelloKitty.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        // User
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> userCredentials { get; set; }
        public DbSet<UserSensitve> userSensitves { get; set; }
        public DbSet<UserAddress> userAddresses { get; set; }
        public DbSet<UserPhone> userPhones { get; set; }
        public DbSet<UserWallet> userWallets { get; set; }
        public DbSet<OAuthAccount> oauthAccounts { get; set; }

        // Role
        public DbSet<Role> roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<RolePermission> rolePermissions { get; set; }

        // Catalog
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductVariant> productVariants { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Attribute> attributes { get; set; }
        public DbSet<AttributeValue> attributeValues { get; set; }
        public DbSet<VariantAttribute> variantAttributes { get; set; }

        // Cart
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        
        // Order
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<OrderAddress> orderAddress { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Refund> refunds { get; set; }
        public DbSet<Shipment> shipments { get; set; }

        // Promotion
        public DbSet<Voucher> vouchers { get; set; }
        public DbSet<VoucherUsage> voucherUsages { get; set; }

        // Inventory
        public DbSet<InventoryLog> inventoryLogs { get; set; }
        
        // Logging
        public DbSet<AuditLog> auditLogs { get; set; }
        public DbSet<SystemLog> systemLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
