using System.Threading;
using System.Threading.Tasks;
using Blogger.Domain.Pagination;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<PaginatedResponse<Article>> GetAsync(IExpressionSpecification<Article> specification, int offset, int limit, CancellationToken cancellationToken);
    }
}