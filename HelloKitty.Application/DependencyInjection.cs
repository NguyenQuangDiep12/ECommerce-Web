using FluentValidation;
using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Auth.Services;
using HelloKitty.Application.Features.Auth.Validators;
using HelloKitty.Application.Features.Carts.Services;
using HelloKitty.Application.Features.Categories.Services;
using HelloKitty.Application.Features.Inventories.Services;
using HelloKitty.Application.Features.Orders.Services;
using HelloKitty.Application.Features.Products.Services;
using HelloKitty.Application.Features.Reports.Services;
using HelloKitty.Application.Features.Reviews.Services;
using HelloKitty.Application.Features.Roles.Services;
using HelloKitty.Application.Features.Users.Services;
using HelloKitty.Application.Features.Vouchers.Services;
using HelloKitty.Domain.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HelloKitty.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // FluentValidation — scan toàn bộ assembly
            services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);

            // Validation service
            services.AddScoped<IValidationService, ValidationService>();

            // Auth
            services.AddScoped<IAuthService, AuthService>();

            // Users
            services.AddScoped<IUserService, UserService>();

            // Catalog
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReviewService, ReviewService>();

            // Cart
            services.AddScoped<ICartService, CartService>();

            // Orders
            services.AddScoped<IOrderService, OrderService>();

            // Promotions
            services.AddScoped<IVoucherService, VoucherService>();

            // Inventory
            services.AddScoped<IInventoryService, InventoryService>();

            // Admin
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}