using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Vouchers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Vouchers.Services
{
    public interface IVoucherService
    {
        Task<Result<VoucherValidationResponse>> ValidateAsync(
        Guid userId, ValidateVoucherRequest request, CancellationToken ct = default);
        Task<Result<VoucherResponse>> CreateAsync(
            CreateVoucherRequest request, CancellationToken ct = default);
        Task<Result<VoucherResponse>> ToggleActiveAsync(
            Guid id, CancellationToken ct = default);
    }
}
