namespace BluePrint.shared.services.Responses
{
    public class GenericResult<T> : Result
    {
        internal GenericResult(bool succeeded, T result) : base(succeeded)
        {
            Result = result;
            Errors = Array.Empty<Error>();
        }

        internal GenericResult(bool succeeded, params Error[] errors) : base(succeeded, errors)
        {
        }

        public static GenericResult<T> Success(T result)
        {
            return new GenericResult<T>(true, result);
        }

        public new static GenericResult<T> Error(params Error[] errors)
        {
            return new GenericResult<T>(false, errors);
        }

        public T Result { get; set; } = default!;

    }

    public class Result
    {
        internal Result(bool succeeded)
        {
            Succeeded = succeeded;
            Errors = Array.Empty<Error>();
        }

        internal Result(bool succeeded, params Error[] errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Error(params Error[] errors)
        {
            return new Result(false, errors);
        }

        public bool Succeeded { get; set; }

        public Error[] Errors { get; set; }

    }
}
