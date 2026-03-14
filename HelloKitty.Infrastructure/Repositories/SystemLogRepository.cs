using HelloKitty.Domain.Logging.Entities;
using HelloKitty.Domain.Logging.Enums;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using LogLevel = HelloKitty.Domain.Logging.Enums.LogLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class SystemLogRepository : ISystemLogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SystemLogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(SystemLog entity, CancellationToken ct = default)
        {
            await _dbContext.systemLogs.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<SystemLog> entities, CancellationToken ct = default)
        {
            await _dbContext.systemLogs.AddRangeAsync(entities, ct);
        }

        public async Task<IEnumerable<SystemLog>> GetRecentErrorsAsync(int count = 50, CancellationToken ct = default)
        {
            return await _dbContext.systemLogs
                            .AsNoTracking()
                            .Where(l => l.LogLevel >= LogLevel.Error)
                            .OrderByDescending(l => l.CreatedAt)
                            .Take(count)
                            .ToListAsync(ct);
        }
    }
}
