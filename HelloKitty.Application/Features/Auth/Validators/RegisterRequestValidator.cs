using FluentValidation;
using HelloKitty.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Auth.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email khong duoc de trong")
                .MaximumLength(150)
                .WithMessage("Email qua dai")
                .Matches(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
                .WithMessage("Email khong hop le");
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Ho va ten khong duoc de trong")
                .MaximumLength(150);
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mat khau khong duoc de trong")
                .MinimumLength(8)
                .WithMessage("Mat khau phai co it nhat 8 ky tu")
                .MaximumLength(15)
                .WithMessage("Mat khau khong duoc vuot qua 15 ky tu")
                .Matches("[A-Z]")
                .WithMessage("Mat khau phai chua it nhat 1 chu cai viet hoa")
                .Matches("[0-9]")
                .WithMessage("Mat khau phai chua it nhat 1 chu so");
            RuleFor(x => x.Gender)
                .NotEmpty()
                .Must(g => g == "Male" || g == "Female" || g == "other")
                .WithMessage("Gioi tinh khong hop le");
        }
    }
}
