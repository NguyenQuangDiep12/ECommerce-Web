using HelloKitty.Domain.Carts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Carts.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task AddAsync(Cart cart, CancellationToken ct = default);
        void Update(Cart cart);
    }
}
