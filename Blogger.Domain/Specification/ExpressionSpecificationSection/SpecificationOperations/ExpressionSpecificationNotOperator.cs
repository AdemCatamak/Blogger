using System;
using System.Linq.Expressions;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.Specification.ExpressionSpecificationSection.SpecificationOperations
{
    internal class ExpressionSpecificationCombineNotOperator : IExpressionSpecificationOperator
    {
        public ExpressionSpecification<TModel> Apply<TModel>(ExpressionSpecification<TModel> specification)
        {
            var candidateExpr = specification.Expression.Parameters[0];
            var body = Expression.Not(specification.Expression.Body);

            var resultExpression = Expression.Lambda<Func<TModel, bool>>(body, candidateExpr);
            var combinedSpecification = new DynamicExpressionSpecification<TModel>(resultExpression);
            return combinedSpecification;
        }
    }

    public static class ExpressionSpecificationNotOperatorExtension
    {
        public static ExpressionSpecification<T> Not<T>(this ExpressionSpecification<T> specification)
        {
            var expressionSpecificationNotOperator = new ExpressionSpecificationCombineNotOperator();
            var expressionSpecification = expressionSpecificationNotOperator.Apply(specification);
            return expressionSpecification;
        }
    }
}