using System.Threading;
using System.Threading.Tasks;
using Blogger.Application.Commands;
using Blogger.Domain;
using Blogger.Domain.DomainSpecification.ArticleSpecifications;
using Blogger.Domain.Pagination;
using Blogger.Domain.Repositories;
using Blogger.Domain.Specification.ExpressionSpecificationSection.SpecificationOperations;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;
using MediatR;

namespace Blogger.Application.CommandHandlers
{
    public class QueryArticleCommandHandler : IRequestHandler<QueryArticleCommand, PaginatedResponse<Article>>
    {
        private readonly IArticleRepository _articleRepository;

        public QueryArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<PaginatedResponse<Article>> Handle(QueryArticleCommand request, CancellationToken cancellationToken)
        {
            IExpressionSpecification<Article> specification = ExpressionSpecification<Article>.Default;

            if (request.Id.HasValue)
            {
                specification = specification.And(new ArticleIdBe(request.Id.Value));
            }

            if (request.AuthorId.HasValue)
            {
                specification = specification.And(new AuthorIdBe(request.AuthorId.Value));
            }

            PaginatedResponse<Article> result = await _articleRepository.GetAsync(specification,
                                                                                  request.Offset(),
                                                                                  request.PageSize,
                                                                                  cancellationToken);

            return result;
        }
    }
}