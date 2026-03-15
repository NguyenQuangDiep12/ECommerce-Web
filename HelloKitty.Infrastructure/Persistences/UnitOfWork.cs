using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Domain.Users.Interfaces;
using HelloKitty.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IUserRepository Users { get; }

        public IUserCredentialRepository UserCredentials { get; }

        public IRefreshTokenRepository RefreshTokens { get; }

        public IAuditLogRepository AuditLogs { get; }

        public ISystemLogRepository SystemLogs { get; }

        public IInventoryLogRepository InventoriesLogs { get; }

        public ICategoryRepository Categories => throw new NotImplementedException();

        public IProductRepository Products => throw new NotImplementedException();

        public IReviewRepository Reviews => throw new NotImplementedException();

        public ICartRepository Carts => throw new NotImplementedException();

        public IOrderRepository Orders => throw new NotImplementedException();

        public IVoucherRepository Vouchers => throw new NotImplementedException();

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IUserCredentialRepository userCredentialRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAuditLogRepository auditLogRepository,
            ISystemLogRepository systemLogRepository,
            IInventoryLogRepository inventoryLogRepository)
        {
            _dbContext = dbContext;
             Users = userRepository;
            UserCredentials = userCredentialRepository;
            RefreshTokens = refreshTokenRepository;
            AuditLogs = auditLogRepository;
            SystemLogs = systemLogRepository;
            InventoriesLogs = inventoryLogRepository;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
