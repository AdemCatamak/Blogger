using System;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.DomainSpecification.ArticleSpecifications
{
    public class AuthorIdBe : ExpressionSpecification<Article>
    {
        public AuthorIdBe(Guid authorId) : base(article => article.AuthorId == authorId)
        {
        }
    }
}