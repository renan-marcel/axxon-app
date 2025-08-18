
using System;

namespace ProdAbs.SharedKernel
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException("A successful result cannot have an error message.");
            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("A failed result must have an error message.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok() => new Result(true, string.Empty);
        public static Result Fail(string message) => new Result(false, message);

        public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Fail<T>(string message) => new Result<T>(default, false, message);
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Cannot access the value of a failed result.");
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
