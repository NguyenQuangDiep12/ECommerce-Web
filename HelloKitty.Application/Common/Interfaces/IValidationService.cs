using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Common.Interfaces
{
    /// <summary>
    /// Validate Object, tra ve danh sach loi neu co
    /// Key = ten field, Value = danh sach loi cua field do
    /// </summary>
    public interface IValidationService
    {
        Task<Dictionary<string, string[]>> ValidateAsync<T>(T instance, CancellationToken ct = default);
    }
}
