using FluentValidation;
using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Auth.Services;
using HelloKitty.Application.Features.Auth.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // scan validators 
            services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);

            // validation service
            services.AddScoped<IValidationService, ValidationService>();

            // Application service
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
