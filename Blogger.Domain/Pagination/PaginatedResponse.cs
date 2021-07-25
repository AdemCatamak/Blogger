using System.Collections.Generic;

namespace Blogger.Domain.Pagination
{
    public class PaginatedResponse<TResponse>
    {
        public List<TResponse> Data { get; private set; }
        public int TotalCount { get; private set; }

        public PaginatedResponse(List<TResponse> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public static PaginatedResponse<TResponse> Empty
            => new(new List<TResponse>(), 0);
    }
}