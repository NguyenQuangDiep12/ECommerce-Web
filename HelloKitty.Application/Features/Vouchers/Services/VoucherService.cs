using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Vouchers.DTOs;
using HelloKitty.Domain.Promotions.Entities;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Promotions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloKitty.Domain.Promotions;

namespace HelloKitty.Application.Features.Vouchers.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;
        public VoucherService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }
        public async Task<Result<VoucherResponse>> CreateAsync(
            CreateVoucherRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<VoucherResponse>.ValidationFailure(errors);

            var existing = await _unitOfWork.Vouchers.GetByCodeAsync(request.Code, ct);
            if (existing is not null)
                return Result<VoucherResponse>.Failure("Voucher code already exists");

            var voucher = new Voucher
            {
                Code = request.Code,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                MinOrderAmount = request.MinOrderAmount,
                MaxDiscountAmount = request.MaxDiscountAmount,
                MaxUsage = request.MaxUsage,
                MaxUsagePerUser = request.MaxUsagePerUser,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            await _unitOfWork.Vouchers.AddAsync(voucher, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<VoucherResponse>.Success(MapToResponse(voucher));
        }

        public async Task<Result<VoucherResponse>> ToggleActiveAsync(
            Guid id, CancellationToken ct = default)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdAsync(id, ct);

            if (voucher is null)
                return Result<VoucherResponse>.Failure("Voucher not found");

            voucher.IsActive = !voucher.IsActive;

            _unitOfWork.Vouchers.Update(voucher);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<VoucherResponse>.Success(MapToResponse(voucher));
        }

        public async Task<Result<VoucherValidationResponse>> ValidateAsync(Guid userId, ValidateVoucherRequest request, CancellationToken ct = default)
        {
            var voucher = await _unitOfWork.Vouchers.GetByCodeAsync(request.Code, ct);

            // Invalid or inactive
            if (voucher is null || !voucher.IsActive)
            {
                return Result<VoucherValidationResponse>.Success(
                    new VoucherValidationResponse
                    {
                        IsValid = false,
                        Message = "Invalid voucher",
                        DiscountAmount = 0
                    });
            }

            // Expired
            if (DateTime.UtcNow < voucher.StartDate || DateTime.UtcNow > voucher.EndDate)
            {
                return Result<VoucherValidationResponse>.Success(
                    new VoucherValidationResponse
                    {
                        IsValid = false,
                        Message = "Voucher expired",
                        DiscountAmount = 0
                    });
            }

            // Min order amount
            if (request.OrderAmount < voucher.MinOrderAmount)
            {
                return Result<VoucherValidationResponse>.Success(
                    new VoucherValidationResponse
                    {
                        IsValid = false,
                        Message = $"Minimum order amount is {voucher.MinOrderAmount:N0}",
                        DiscountAmount = 0
                    });
            }

            // Total usage
            var totalUsage = await _unitOfWork.Vouchers.GetTotalUsageCountAsync(voucher.VoucherId, ct);
            if (totalUsage >= voucher.MaxUsage)
            {
                return Result<VoucherValidationResponse>.Success(
                    new VoucherValidationResponse
                    {
                        IsValid = false,
                        Message = "Voucher usage limit reached",
                        DiscountAmount = 0
                    });
            }

            // User usage
            var userUsage = await _unitOfWork.Vouchers.GetUserUsageCountAsync(voucher.VoucherId, userId, ct);
            if (userUsage >= voucher.MaxUsagePerUser)
            {
                return Result<VoucherValidationResponse>.Success(
                    new VoucherValidationResponse
                    {
                        IsValid = false,
                        Message = "You have reached usage limit for this voucher",
                        DiscountAmount = 0
                    });
            }

            // Calculate discount
            decimal discount;

            if (voucher.DiscountType == DiscountType.Percentage)
            {
                var calculated = request.OrderAmount * voucher.DiscountValue / 100;
                discount = Math.Min(calculated, voucher.MaxDiscountAmount ?? decimal.MaxValue);
            }
            else
            {
                discount = voucher.DiscountValue;
            }

            return Result<VoucherValidationResponse>.Success(
                new VoucherValidationResponse
                {
                    IsValid = true,
                    Message = null,
                    DiscountAmount = discount
                });
        }
        private static VoucherResponse MapToResponse(Voucher v)
        {
            return new VoucherResponse
            {
                VoucherId = v.VoucherId,
                Code = v.Code,
                DiscountType = v.DiscountType,
                DiscountValue = v.DiscountValue,
                MinOrderAmount = v.MinOrderAmount,
                MaxUsage = v.MaxUsage,
                IsActive = v.IsActive,
                StartDate = v.StartDate,
                EndDate = v.EndDate
            };
        }
    }
}
