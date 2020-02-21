namespace ApiModels
{
    public class ApiResult<T>
    {
        public ApiResult()
        {
        }

        public ApiResult(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}