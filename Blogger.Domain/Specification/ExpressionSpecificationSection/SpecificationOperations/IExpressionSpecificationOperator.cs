using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.Specification.ExpressionSpecificationSection.SpecificationOperations
{
    public interface IExpressionSpecificationOperator
    {
        ExpressionSpecification<TModel> Apply<TModel>(ExpressionSpecification<TModel> specification);
    }
}