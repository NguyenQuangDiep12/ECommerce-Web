using FluentValidation;
using HelloKitty.Application.Features.Categories.DTOs;
using HelloKitty.Application.Features.Reviews.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Reviews.Validators
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Sản phẩm không hợp lệ");

            RuleFor(x => x.Ratings)
                .InclusiveBetween(1, 5)
                .WithMessage("Đánh giá phải từ 1 đến 5 sao");

            RuleFor(x => x.Comments)
                .MaximumLength(1000).When(x => x.Comments is not null)
                .WithMessage("Bình luận tối đa 1000 ký tự");
        }
    }
}
