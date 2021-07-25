using System;
using System.Collections.Generic;
using Blogger.Domain;
using Blogger.Domain.Pagination;
using MediatR;

namespace Blogger.Application.Commands
{
    public class QueryAuthorCommand : PaginatedRequest,
                                      IRequest<PaginatedResponse<Author>>
    {
        public List<Guid> AuthorIdList { get; } = new();

        public QueryAuthorCommand(int pageNumber = 1, int pageSize = 1)
            : base(pageNumber, pageSize)
        {
        }
    }
}