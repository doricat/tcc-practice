using System.Collections.Generic;

namespace ApiModels
{
    public class PagedResult<T> : ApiResult<T>
    {
        public PagedResult(T value) : base(value)
        {
        }

        public string NextLink { get; set; }

        public IList<string> Sorts { get; set; }

        public int? Count { get; set; }

        public int? MaxPageSize { get; set; }
    }
}