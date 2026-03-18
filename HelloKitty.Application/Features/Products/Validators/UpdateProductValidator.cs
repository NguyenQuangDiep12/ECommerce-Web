using FluentValidation;
using HelloKitty.Application.Features.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductName)
            .NotEmpty()
            .MaximumLength(255);

            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}
