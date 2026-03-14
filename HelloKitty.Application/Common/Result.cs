using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Common
{
    /// <summary>
    /// Result Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? Error { get; private set; }
        public Dictionary<string, string[]>? ValidationErrors { get; private set; }
        public bool IsValidationError
        {
            get
            {
                return ValidationErrors != null && ValidationErrors.Count > 0;
            }
        }

        private Result(bool isSuccess, T? value, string? error, Dictionary<string, string[]>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ValidationErrors = validationErrors;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        public static Result<T> Failure(string error) => new Result<T>(false, default, error);
        public static Result<T> ValidationFailure(Dictionary<string, string[]> errors) => new Result<T>(false, default, "Du lieu dau vao khong hop le", errors);
    }

    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string? Error { get; private set; }
        public Dictionary<string, string[]>? ValidationErrors { get; private set; }
        public bool IsValidationError
        {
            get
            {
                return ValidationErrors != null && ValidationErrors.Count > 0;
            }
        }

        private Result(bool isSuccess, string? error, Dictionary<string, string[]>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            ValidationErrors = validationErrors;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
        public static Result ValidationFailure(Dictionary<string, string[]> errors) => new Result(false, "Du lieu dau vao khong hop le", errors);
    }
}
