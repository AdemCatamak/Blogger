using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.Specification.ExpressionSpecificationSection.SpecificationOperations
{
    public interface IExpressionSpecificationCombineOperator
    {
        IExpressionSpecification<TModel> Combine<TModel>(IExpressionSpecification<TModel> left, IExpressionSpecification<TModel> right);
    }
}