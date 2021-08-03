using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogger.Application.Commands;
using Blogger.Domain;
using Blogger.Domain.Pagination;
using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;

namespace Blogger.GraphQLSection.Types
{
    public sealed class ArticleType : ObjectGraphType<Article>
    {
        public ArticleType(IMediator mediator,
                           IDataLoaderContextAccessor dataLoaderContextAccessor)
        {
            Field(article => article.Id);
            Field(article => article.Title);
            Field(article => article.AuthorId);
            Field(article => article.PublishedOn);
            Field(article => article.CreatedOn);

            Field<AuthorType>(name: "author",
                              resolve: context =>
                              {
                                  IDataLoader<Guid, Author> loader
                                      = dataLoaderContextAccessor.Context.GetOrAddBatchLoader<Guid, Author>
                                          ("AuthorById",
                                           (authorIdCollection, cancellationToken) => FetchFunc(authorIdCollection, cancellationToken, mediator));

                                  return loader.LoadAsync(context.Source.AuthorId);
                              });
        }

        private async Task<IDictionary<Guid, Author>> FetchFunc(IEnumerable<Guid> authorIdCollection, CancellationToken cancellationToken, IMediator mediator)
        {
            authorIdCollection = authorIdCollection.ToList();
            QueryAuthorCommand queryAuthorCommand = new(1, authorIdCollection.Count());
            queryAuthorCommand.AuthorIdList.AddRange(authorIdCollection);

            PaginatedResponse<Author> paginatedResponse = await mediator.Send(queryAuthorCommand, cancellationToken);

            Dictionary<Guid, Author> authors = paginatedResponse.Data.ToDictionary(author => author.Id);
            return authors;
        }
    }

    public sealed class PaginatedArticleType : ObjectGraphType<PaginatedResponse<Article>>
    {
        public PaginatedArticleType()
        {
            Field(response => response.TotalCount);
            Field<ListGraphType<ArticleType>>(nameof(PaginatedResponse<Article>.Data));
        }
    }
}