using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Carts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Carts.Services
{
    public interface ICartService
    {
        Task<Result<CartResponse>> GetCartAsync(Guid userId, CancellationToken ct = default);
        Task<Result<CartResponse>> AddItemAsync(Guid userId, AddCartItemRequest request, CancellationToken ct = default);
        Task<Result<CartResponse>> UpdateItemAsync(Guid userId, Guid cartItemId, UpdateCartItemRequest request, CancellationToken ct = default);
        Task<Result<CartResponse>> RemoveItemAsync(Guid userId, Guid cartItemId, CancellationToken ct = default);
        Task<Result> ClearCartAsync(Guid userId, CancellationToken ct = default);
    }
}
