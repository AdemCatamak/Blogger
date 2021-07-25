using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogger.Domain;
using Blogger.Domain.Exceptions;
using Blogger.Domain.Pagination;
using Blogger.Domain.Repositories;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Infra.Db.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BloggerDbContext _bloggerDbContext;

        public AuthorRepository(BloggerDbContext bloggerDbContext)
        {
            _bloggerDbContext = bloggerDbContext;
        }

        public async Task<Author> GetFirstAsync(IExpressionSpecification<Author> specification, CancellationToken cancellationToken)
        {
            PaginatedResponse<Author> paginatedResponse = await GetAsync(specification, 0, 1, cancellationToken);
            return paginatedResponse.Data.First();
        }

        public async Task<PaginatedResponse<Author>> GetAsync(IExpressionSpecification<Author> specification, int offset, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Author> authors = _bloggerDbContext.Authors
                                                          .Where(specification.Expression)
                                                          .OrderBy(author => author.CreatedOn);

            (int totalCount, List<Author> authorList) = await authors.PaginatedQueryAsync(offset, pageSize, cancellationToken);

            if (!authorList.Any())
            {
                throw new NotFoundException<Author>();
            }

            return new PaginatedResponse<Author>(authorList, totalCount);
        }
    }
}