using HelloKitty.Domain.Carts.Entities;
using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Domain.Users.Interfaces;
using HelloKitty.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
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

        public ICategoryRepository Categories { get; }

        public IProductRepository Products { get; }

        public IReviewRepository Reviews {  get; }

        public ICartRepository Carts { get; }

        public IOrderRepository Orders { get; }

        public IVoucherRepository Vouchers { get; }

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IUserCredentialRepository userCredentialRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAuditLogRepository auditLogRepository,
            ISystemLogRepository systemLogRepository,
            IInventoryLogRepository inventoryLogRepository,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IReviewRepository reviewRepository,
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IVoucherRepository voucherRepository)
        {
            _dbContext = dbContext;
             Users = userRepository;
            UserCredentials = userCredentialRepository;
            RefreshTokens = refreshTokenRepository;
            AuditLogs = auditLogRepository;
            SystemLogs = systemLogRepository;
            InventoriesLogs = inventoryLogRepository;
            Categories = categoryRepository;
            Products = productRepository;
            Reviews = reviewRepository;
            Vouchers = voucherRepository;
            Carts = cartRepository;
            Orders = orderRepository;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }

        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if(_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
