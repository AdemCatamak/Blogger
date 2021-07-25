using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.Infra
{
    public static class CompositionRootExtension
    {
        public static IServiceCollection AddBloggerDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            ICompositionRoot compositionRoot = new CompositionRoot();
            compositionRoot.Register(serviceCollection, configuration);
            return serviceCollection;
        }
    }
}