using System;
using System.Collections.Generic;
using Blogger.Domain.Specification.ExpressionSpecificationSection.Specifications;

namespace Blogger.Domain.DomainSpecification.AuthorSpecifications
{
    public class AuthorIdBeIn : ExpressionSpecification<Author>
    {
        public AuthorIdBeIn(ICollection<Guid> authorIdCollection) : base(author => authorIdCollection.Contains(author.Id))
        {
        }
    }
}