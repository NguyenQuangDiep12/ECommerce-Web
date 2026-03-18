using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Promotions.Interfaces
{
    public interface IVoucherRepository : IWriteRepository<Voucher>
    {
        Task AddAsync(Voucher voucher, CancellationToken ct = default);
        Task<Voucher?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Voucher?> GetByCodeAsync(string code, CancellationToken ct = default);
        Task<int> GetTotalUsageCountAsync(Guid voucherId, CancellationToken ct = default);
        Task<int> GetUserUsageCountAsync(
            Guid voucherId, Guid userId, CancellationToken ct = default);
    }
}
