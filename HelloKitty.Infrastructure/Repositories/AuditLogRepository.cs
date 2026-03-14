using HelloKitty.Domain.Logging.Entities;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public AuditLogRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task AddAsync(AuditLog entity, CancellationToken ct = default)
        {
            await _dbcontext.auditLogs.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<AuditLog> entities, CancellationToken ct = default)
        {
            await _dbcontext.auditLogs.AddRangeAsync(entities, ct);
        }

        public async Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName, string recordId, CancellationToken ct = default)
        {
            return await _dbcontext.auditLogs
                            .AsNoTracking()
                            .Where(t => t.TableName == tableName && t.RecordId == recordId)
                            .OrderByDescending(l => l.CreatedAt)
                            .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            return await _dbcontext.auditLogs
                            .AsNoTracking()
                            .Where(t => t.UserId == userId)
                            .OrderByDescending(l => l.CreatedAt)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync(ct);
        }
    }
}
