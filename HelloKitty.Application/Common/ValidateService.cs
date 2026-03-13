using FluentValidation;
using HelloKitty.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HelloKitty.Application.Common
{
    public class ValidationService(IServiceProvider serviceProvider) : IValidationService
    {
        public async Task<Dictionary<string, string[]>> ValidateAsync<T>(
            T instance,
            CancellationToken ct = default)
        {
            // Tim validator tuong ung voi type T trong DI container
            var validator = serviceProvider.GetService<IValidator<T>>();

            // Khong co validator => bo qua, khong loi
            if (validator is null)
                return new Dictionary<string, string[]>();

            var result = await validator.ValidateAsync(instance, ct);

            if (result.IsValid)
                return new Dictionary<string, string[]>();

            // Group loi theo field name
            /** Ex: 
             * {
             *      "Email": ["Email khong dung dinh dang"],
             *      "Password": ["Toi thieu phai co 8 ky tu", "phai co chu cai hoa"]
             * }
             */
            return result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}