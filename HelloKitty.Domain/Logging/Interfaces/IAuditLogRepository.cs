using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Logging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Logging.Interfaces
{
    public interface IAuditLogRepository : IAppendOnlyRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 10, CancellationToken ct = default);
        Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName, string recordId, CancellationToken ct = default);
    }
}
