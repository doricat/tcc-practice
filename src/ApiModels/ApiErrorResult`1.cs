namespace ApiModels
{
    public class ApiErrorResult<T>
    {
        public ApiErrorResult()
        {
        }

        public ApiErrorResult(T error)
        {
            Error = error;
        }

        public T Error { get; set; }
    }
}