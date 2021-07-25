using System.Linq;
using System.Reflection;
using Blogger.Application.Commands;
using Blogger.Domain.Repositories;
using Blogger.Infra.Db;
using Blogger.Infra.Db.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blogger.Infra
{
    public interface ICompositionRoot
    {
        void Register(IServiceCollection serviceCollection, IConfiguration configuration);
    }

    public class CompositionRoot : ICompositionRoot
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(QueryAuthorCommand).Assembly);

            services.AddDbContext<BloggerDbContext>((provider, builder) =>
            {
                builder.UseSqlServer(provider.GetRequiredService<IConfiguration>().GetConnectionString("SqlServer"));
                builder.EnableSensitiveDataLogging();
                builder.UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>());
            });

            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
        }
    }
}