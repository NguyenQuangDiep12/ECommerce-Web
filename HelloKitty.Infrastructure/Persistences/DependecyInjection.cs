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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HelloKitty.Infrastructure.Persistences
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ISystemLogRepository, SystemLogRepository>();
            services.AddScoped<IInventoryLogRepository, InventoryLogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infrastructure services
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IMediaStorageService, CloudinaryService>();

            // JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}