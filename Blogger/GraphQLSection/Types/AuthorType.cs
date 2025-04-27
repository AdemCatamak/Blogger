using Blogger.Application.Commands;
using Blogger.Domain;
using Blogger.Domain.Pagination;
using GraphQL;
using GraphQL.Types;
using MediatR;

namespace Blogger.GraphQLSection.Types
{
    public sealed class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType(IMediator mediator)
        {
            Field(author => author.Id);
            Field(author => author.FirstName);
            Field(author => author.LastName);

            Field<PaginatedArticleType>
                (name: "articles",
                 arguments: new QueryArguments(
                                               new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "pageNumber"},
                                               new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "pageSize"}
                                              ),
                 resolve: context =>
                 {
                     var pageNumber = context.GetArgument<int>("pageNumber");
                     var pageSize = context.GetArgument<int>("pageSize");
                     QueryArticleCommand queryArticleCommand = new(pageNumber, pageSize)
                                                               {
                                                                   AuthorId = context.Source.Id
                                                               };
                     PaginatedResponse<Article> paginatedResponse = mediator.Send(queryArticleCommand)
                                                                            .GetAwaiter().GetResult();

                     return paginatedResponse;
                 });
        }
    }

    public sealed class PaginatedAuthorType : ObjectGraphType<PaginatedResponse<Author>>
    {
        public PaginatedAuthorType()
        {
            Field(response => response.TotalCount);
            Field<ListGraphType<AuthorType>>(nameof(PaginatedResponse<Author>.Data));
        }
    }
}