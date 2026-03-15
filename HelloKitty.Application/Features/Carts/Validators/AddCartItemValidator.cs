using FluentValidation;
using HelloKitty.Application.Features.Carts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Carts.Validators
{
    public class AddCartItemValidator : AbstractValidator<AddCartItemRequest>
    {
        public AddCartItemValidator()
        {
            RuleFor(x => x.VarianId)
                .NotEmpty().WithMessage("product is not valid");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The number of product must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Total number does not exceed 100");
        }
    }

    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemRequest>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The number of product must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Total number does not exceed 100");
        }
    }
}
