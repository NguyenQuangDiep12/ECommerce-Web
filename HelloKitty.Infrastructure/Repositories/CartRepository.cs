using HelloKitty.Domain.Carts.Entities;
using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace HelloKitty.Infrastructure.Repositories;

public class CartRepository(ApplicationDbContext dbContext) : ICartRepository
{
    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Carts
            .Include(c => c.CartItems)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(v => v.Product)
                        .ThenInclude(p => p.ProductImages)
            .Include(c => c.CartItems)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(v => v.VariantAttributes!)
                        .ThenInclude(va => va.AttributeValue)
                            .ThenInclude(av => av.Attribute)
            .AsSplitQuery() // tránh cartesian explosion
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
    }
    public async Task AddAsync(Cart cart, CancellationToken ct = default)
    {
        await dbContext.Carts.AddAsync(cart, ct);
    }

    public void Update(Cart cart)
    {
        dbContext.Carts.Update(cart);
    }
}
