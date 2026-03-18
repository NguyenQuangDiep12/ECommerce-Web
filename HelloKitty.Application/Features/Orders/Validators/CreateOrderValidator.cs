using FluentValidation;
using HelloKitty.Application.Features.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.ReceiverName)
            .NotEmpty().WithMessage("Recipient name cannot be empty")
            .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("The phone number must not be left blank")
                .Matches(@"^(0|\+84)[3|5|7|8|9][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Province)
                .NotEmpty().WithMessage("Province/City cannot be left blank");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("District cannot be left blank");

            RuleFor(x => x.Ward)
                .NotEmpty().WithMessage("Ward must not be left blank");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("The specific address must not be left blank")
                .MaximumLength(255);
        }
    }
}
