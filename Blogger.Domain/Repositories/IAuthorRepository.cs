using System.Threading;
using System.Threading.Tasks;
using Blogger.Domain.Pagination;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetFirstAsync(IExpressionSpecification<Author> specification, CancellationToken cancellationToken);
        Task<PaginatedResponse<Author>> GetAsync(IExpressionSpecification<Author> specification, int offset, int pageSize, CancellationToken cancellationToken);
    }
}