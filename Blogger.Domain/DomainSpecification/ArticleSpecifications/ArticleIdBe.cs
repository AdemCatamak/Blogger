using System;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.DomainSpecification.ArticleSpecifications
{
    public class ArticleIdBe : ExpressionSpecification<Article>
    {
        public ArticleIdBe(Guid articleId) : base(article => article.Id == articleId)
        {
        }
    }
}