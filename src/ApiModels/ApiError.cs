using System.Collections.Generic;

namespace ApiModels
{
    public class ApiError
    {
        public ApiError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }

        public string Message { get; set; }

        public string Target { get; set; }

        public IList<ApiError> Details { get; set; }

        public InnerError InnerError { get; set; }
    }
}
