using FluentValidation;
using HelloKitty.Application.Features.Categories.DTOs;
using HelloKitty.Application.Features.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductName)
           .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
           .MaximumLength(255).WithMessage("Tên sản phẩm tối đa 255 ký tự");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Danh mục không được để trống");

            RuleFor(x => x.Description)
                .MaximumLength(4000).When(x => x.Description is not null);

            RuleFor(x => x.Variants)
                .NotEmpty().WithMessage("Sản phẩm phải có ít nhất 1 phiên bản");

            RuleForEach(x => x.Variants).ChildRules(v =>
            {
                v.RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Giá phải lớn hơn 0");

                v.RuleFor(x => x.Quantity)
                    .GreaterThanOrEqualTo(0).WithMessage("Số lượng không được âm");

                v.RuleFor(x => x.SKU)
                    .MaximumLength(100).When(x => x.SKU is not null);
            });
        }
    }
}
