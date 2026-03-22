using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Domain.Users.Interfaces;

namespace HelloKitty.Domain.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Users
        IUserRepository Users { get; }
        IUserCredentialRepository UserCredentials { get; }
        IRefreshTokenRepository RefreshTokens { get; }

        // RBAC
        IRoleRepository Roles { get; }
        IUserRoleRepository UserRoles { get; }

        // Catalog
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IReviewRepository Reviews { get; }

        // Cart
        ICartRepository Carts { get; }

        // Orders
        IOrderRepository Orders { get; }

        // Promotions
        IVoucherRepository Vouchers { get; }

        // Logging
        IAuditLogRepository AuditLogs { get; }
        ISystemLogRepository SystemLogs { get; }

        // Inventory
        IInventoryLogRepository InventoriesLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}