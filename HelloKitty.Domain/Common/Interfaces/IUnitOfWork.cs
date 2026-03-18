using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IUserCredentialRepository UserCredentials { get; }
        IRefreshTokenRepository RefreshTokens { get; }

        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IReviewRepository Reviews { get; }

        ICartRepository Carts { get; }

        IOrderRepository Orders { get; }

        IVoucherRepository Vouchers { get; }

        IAuditLogRepository AuditLogs { get; }
        ISystemLogRepository SystemLogs { get; }
        IInventoryLogRepository InventoriesLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
