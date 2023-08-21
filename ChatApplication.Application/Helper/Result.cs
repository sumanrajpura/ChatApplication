namespace ChatApplication.Application.Helper
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
        public static Result<T> Failure(string error = "Error while performing operation.") => new() { IsSuccess = false, Error = error };
    }
}
