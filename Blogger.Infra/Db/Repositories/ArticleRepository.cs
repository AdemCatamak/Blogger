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
    public class ArticleRepository : IArticleRepository
    {
        private readonly BloggerDbContext _bloggerDbContext;

        public ArticleRepository(BloggerDbContext bloggerDbContext)
        {
            _bloggerDbContext = bloggerDbContext;
        }

        public async Task<PaginatedResponse<Article>> GetAsync(IExpressionSpecification<Article> specification, int offset, int limit, CancellationToken cancellationToken)
        {
            IQueryable<Article> articles = _bloggerDbContext.Articles
                                                            .Where(specification.Expression)
                                                            .OrderBy(article => article.CreatedOn);

            (int totalCount, var articleList) = await articles.PaginatedQueryAsync(offset, limit, cancellationToken);

            if (!articleList.Any())
            {
                throw new NotFoundException<Article>();
            }

            return new PaginatedResponse<Article>(articleList, totalCount);
        }
    }
}