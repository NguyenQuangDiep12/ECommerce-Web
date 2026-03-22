using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace HelloKitty.Infrastructure.Persistences
{
    public class UnitOfWork(
        ApplicationDbContext dbContext,
        IUserRepository userRepository,
        IUserCredentialRepository userCredentialRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IAuditLogRepository auditLogRepository,
        ISystemLogRepository systemLogRepository,
        IInventoryLogRepository inventoryLogRepository,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IReviewRepository reviewRepository,
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        IVoucherRepository voucherRepository) : IUnitOfWork
    {
        public IUserRepository Users { get; } = userRepository;
        public IUserCredentialRepository UserCredentials { get; } = userCredentialRepository;
        public IRefreshTokenRepository RefreshTokens { get; } = refreshTokenRepository;
        public IRoleRepository Roles { get; } = roleRepository;
        public IUserRoleRepository UserRoles { get; } = userRoleRepository;
        public IAuditLogRepository AuditLogs { get; } = auditLogRepository;
        public ISystemLogRepository SystemLogs { get; } = systemLogRepository;
        public IInventoryLogRepository InventoriesLogs { get; } = inventoryLogRepository;
        public ICategoryRepository Categories { get; } = categoryRepository;
        public IProductRepository Products { get; } = productRepository;
        public IReviewRepository Reviews { get; } = reviewRepository;
        public ICartRepository Carts { get; } = cartRepository;
        public IOrderRepository Orders { get; } = orderRepository;
        public IVoucherRepository Vouchers { get; } = voucherRepository;

        private IDbContextTransaction? _transaction;

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await dbContext.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync()
            => _transaction = await dbContext.Database.BeginTransactionAsync();

        public async Task CommitAsync()
        {
            if (_transaction is null) return;
            await dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction is null) return;
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose() => dbContext.Dispose();
    }
}