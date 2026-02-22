namespace HomeHub.Application.Common
{
    //TODO: Definir tipos de respuestas en vexz de codigos?
    public sealed record Error(string Code, string Message);

    //TODO: Falta un Error?
    public sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public Error? Error { get; }

        private Result(bool ok, T? value, Error? error) => (IsSuccess, Value, Error) = (ok, value, error);

        public static Result<T> Ok(T value) => new(true, value, null);
        public static Result<T> Fail(string code, string message) => new(false, default, new Error(code, message));
    }
    public sealed class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        private Result(bool ok, Error? error) => (IsSuccess, Error) = (ok, error);

        public static Result Ok() => new(true, null);
        public static Result Fail(string code, string message) => new(false, new Error(code, message));
    }
}
