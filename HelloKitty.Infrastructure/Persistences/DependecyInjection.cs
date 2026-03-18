using HelloKitty.Domain.Carts.Interfaces;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Domain.Logging.Interfaces;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Domain.Promotions.Interfaces;
using HelloKitty.Domain.Users.Interfaces;
using HelloKitty.Infrastructure.Repositories;
using HelloKitty.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        configurations.GetConnectionString("TMDT"), // use user-secrets key connectionstrings
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            // repositories 
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ISystemLogRepository, SystemLogRepository>();
            services.AddScoped<IInventoryLogRepository, InventoryLogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();

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
                        ValidIssuer = configurations["Jwt:Issuer"],
                        ValidAudience = configurations["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configurations["Jwt:SecretKey"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
