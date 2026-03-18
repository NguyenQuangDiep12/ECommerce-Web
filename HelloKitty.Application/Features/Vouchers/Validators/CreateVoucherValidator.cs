using FluentValidation;
using HelloKitty.Application.Features.Vouchers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Vouchers.Validators
{
    public class CreateVoucherValidator : AbstractValidator<CreateVoucherRequest>
    {
        public CreateVoucherValidator()
        {
            RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Voucher code is required")
            .MaximumLength(50).WithMessage("Voucher code must not exceed 50 characters")
            .Matches("^[A-Z0-9_-]+$")
            .WithMessage("Voucher code can only contain uppercase letters, numbers, '_' and '-'");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Discount value must be greater than 0");

            RuleFor(x => x.MinOrderAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum order amount cannot be negative");

            RuleFor(x => x.MaxUsage)
                .GreaterThan(0).WithMessage("Max usage must be greater than 0");

            RuleFor(x => x.MaxUsagePerUser)
                .GreaterThan(0).WithMessage("Max usage per user must be greater than 0");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be greater than start date");

            RuleFor(x => x.StartDate)
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Start date cannot be in the past");
        }
    }
}
