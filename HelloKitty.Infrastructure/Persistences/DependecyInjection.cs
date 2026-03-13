using HelloKitty.Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HelloKitty.Infrastructure.Repositories;
using HelloKitty.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloKitty.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelloKitty.Infrastructure.Persistences
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configurations
            )
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                        configurations.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            // repositories 
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // infrastructure services
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

            // Jwt Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configurations["Issuer"],
                        ValidAudience = configurations["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configurations["SecretKey"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly
        }
    }
}
