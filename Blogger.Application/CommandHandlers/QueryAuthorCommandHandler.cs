using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogger.Application.Commands;
using Blogger.Domain;
using Blogger.Domain.DomainSpecification.AuthorSpecifications;
using Blogger.Domain.Pagination;
using Blogger.Domain.Repositories;
using Blogger.Domain.Specification.ExpressionSpecificationSection.SpecificationOperations;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;
using MediatR;

namespace Blogger.Application.CommandHandlers
{
    public class QueryAuthorCommandHandler : IRequestHandler<QueryAuthorCommand, PaginatedResponse<Author>>
    {
        private readonly IAuthorRepository _authorRepository;

        public QueryAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<PaginatedResponse<Author>> Handle(QueryAuthorCommand request, CancellationToken cancellationToken)
        {
            IExpressionSpecification<Author> specification = ExpressionSpecification<Author>.Default;

            if (request.AuthorIdList.Any())
            {
                specification = specification.And(new AuthorIdBeIn(request.AuthorIdList));
            }

            PaginatedResponse<Author> result = await _authorRepository.GetAsync(specification, request.Offset(), request.PageSize, cancellationToken);

            return result;
        }
    }
}