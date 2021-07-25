using System;
using Blogger.Domain;
using Blogger.Domain.Pagination;
using MediatR;

namespace Blogger.Application.Commands
{
    public class QueryArticleCommand : PaginatedRequest,
                                       IRequest<PaginatedResponse<Article>>
    {
        public Guid? Id { get; set; }
        public Guid? AuthorId { get; set; }

        public QueryArticleCommand(int pageNumber = 1, int pageSize = 1)
            : base(pageNumber, pageSize)
        {
        }
    }
}