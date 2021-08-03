using System;
using System.Linq;
using Blogger.Application.Commands;
using Blogger.Domain;
using Blogger.Domain.Pagination;
using GraphQL;
using GraphQL.Types;
using MediatR;

namespace Blogger.GraphQLSection.Types
{
    public class BloggerQueryType : ObjectGraphType
    {
        public BloggerQueryType(IMediator mediator)
        {
            Field<AuthorType>
                (name: "author",
                 arguments: new QueryArguments(new QueryArgument<NonNullGraphType<GuidGraphType>> {Name = "id"}),
                 resolve: context =>
                 {
                     var authorId = context.GetArgument<Guid>("id");

                     var queryAuthorCommand = new QueryAuthorCommand();
                     queryAuthorCommand.AuthorIdList.Add(authorId);

                     PaginatedResponse<Author> paginatedResponse = mediator.Send(queryAuthorCommand).GetAwaiter().GetResult();
                     Author author = paginatedResponse.Data.First();

                     return author;
                 });

            Field<PaginatedAuthorType>
                (name: "authors",
                 arguments: new QueryArguments(new QueryArgument<IntGraphType> {Name = "pageNumber"},
                                               new QueryArgument<IntGraphType> {Name = "pageSize"}
                                              ),
                 resolve: context =>
                 {
                     int pageNumber = context.GetArgument<int?>("pageNumber") ?? 1;
                     int pageSize = context.GetArgument<int?>("pageSize") ?? 1;

                     var queryAuthorCommand = new QueryAuthorCommand(pageNumber, pageSize);

                     PaginatedResponse<Author> paginatedResponse = mediator.Send(queryAuthorCommand).GetAwaiter().GetResult();

                     return paginatedResponse;
                 });

            Field<ArticleType>
                (name: "article",
                 arguments:
                 new QueryArguments(new QueryArgument<NonNullGraphType<GuidGraphType>> {Name = "id"}
                                   ),
                 resolve: context =>
                 {
                     var articleId = context.GetArgument<Guid>("id");

                     var queryArticleCommand = new QueryArticleCommand
                                               {
                                                   Id = articleId
                                               };

                     PaginatedResponse<Article> paginatedResponse = mediator.Send(queryArticleCommand)
                                                                            .GetAwaiter().GetResult();
                     Article article = paginatedResponse.Data.First();

                     return article;
                 });

            Field<PaginatedArticleType>
                (name: "articles",
                 arguments:
                 new QueryArguments(new QueryArgument<GuidGraphType> {Name = "authorId"},
                                    new QueryArgument<IntGraphType> {Name = "pageNumber"},
                                    new QueryArgument<IntGraphType> {Name = "pageSize"}
                                   ),
                 resolve: context =>
                 {
                     var authorId = context.GetArgument<Guid?>("authorId");
                     int pageNumber = context.GetArgument<int?>("pageNumber") ?? 1;
                     int pageSize = context.GetArgument<int?>("pageSize") ?? 1;

                     var queryArticleCommand = new QueryArticleCommand(pageNumber, pageSize)
                                               {
                                                   AuthorId = authorId
                                               };
                     PaginatedResponse<Article> paginatedResponse = mediator.Send(queryArticleCommand)
                                                                            .GetAwaiter().GetResult();

                     return paginatedResponse;
                 });
        }
    }
}