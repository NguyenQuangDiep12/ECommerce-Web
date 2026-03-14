using FluentValidation;
using HelloKitty.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Auth.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email khong duoc de trong")
                .MaximumLength(150)
                .WithMessage("Email qua dai")
                .Matches(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
                .WithMessage("Email khong hop le");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mat khau khong duoc de trong");
        }
    }
}
