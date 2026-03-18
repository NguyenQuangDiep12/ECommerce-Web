using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public VoucherRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Voucher voucher, CancellationToken ct = default)
        {
            await _dbContext.Vouchers.AddAsync(voucher, ct);
        }

        public async Task<Voucher?> GetByCodeAsync(string code, CancellationToken ct = default)
        {
            return await _dbContext.Vouchers.FirstOrDefaultAsync(v => v.Code == code, ct);
        }

        public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Vouchers.FindAsync(new object[] {id }, ct);
        }

        public async Task<int> GetTotalUsageCountAsync(Guid voucherId, CancellationToken ct = default)
        {
            return await _dbContext.VoucherUsages.CountAsync(u => u.VoucherId == voucherId, ct);
        }

        public async Task<int> GetUserUsageCountAsync(Guid voucherId, Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.VoucherUsages.CountAsync(u => u.VoucherId == voucherId && u.UserId == userId, ct);
        }

        public void Remove(Voucher entity)
        {
            _dbContext.Vouchers.Remove(entity);
        }

        public void Update(Voucher entity)
        {
            _dbContext.Vouchers.Update(entity);
        }
    }
}
