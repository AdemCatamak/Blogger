using System;
using System.Collections.Generic;
using Blogger.GraphQLSection.Types;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.GraphQLSection
{
    public class BloggerSchema : Schema
    {
        public BloggerSchema(IServiceProvider serviceProvider, IEnumerable<IFieldMiddleware> middleware)
            : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<BloggerQueryType>();
            foreach (IFieldMiddleware fieldMiddleware in middleware)
            {
                FieldMiddleware.Use(fieldMiddleware);
            }
        }
    }
}