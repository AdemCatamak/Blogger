namespace Blogger.Domain.Pagination
{
    public abstract class PaginatedRequest
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        protected PaginatedRequest(int pageNumber = 1, int pageSize = 1)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int Offset() => (PageNumber - 1) * PageSize;
    }
}