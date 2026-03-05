using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Logging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Logging.Interfaces
{
    public interface ISystemLogRepository : IAppendOnlyRepository<SystemLog>
    {
        Task<IEnumerable<SystemLog>> GetRecentErrorsAsync(int count = 50, CancellationToken ct = default);
    }
}
