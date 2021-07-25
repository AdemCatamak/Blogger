using System;
using System.Linq;
using Blogger.Domain;
using Blogger.Infra;
using Blogger.Infra.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Blogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Blogger", Version = "v1"}); });

            services.AddBloggerDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BloggerDbContext bloggerDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blogger"));
            }

            app.UseMiddleware<Blogger.Controllers.Middleware.ExceptionHandlerMiddleware>();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            SeedData(bloggerDbContext);
        }

        private void SeedData(BloggerDbContext bloggerDbContext)
        {
            bloggerDbContext.Database.EnsureCreated();

            if (bloggerDbContext.Authors.Any())
            {
                return;
            }

            Author author1 = Author.Create("fname 1", "lname 1");
            Author author2 = Author.Create("fname 2", "lname 2");
            Author author3 = Author.Create("fname 3", "lname 3");

            bloggerDbContext.Authors.Add(author1);
            bloggerDbContext.Authors.Add(author2);
            bloggerDbContext.Authors.Add(author3);

            Article article1 = Article.Create("title-1--author-1", new DateTime(2020, 12, 03), author1);
            Article article2 = Article.Create("title-2--author-1", new DateTime(2021, 01, 12), author1);
            Article article3 = Article.Create("title-3--author-2", new DateTime(2019, 07, 31), author2);
            Article article4 = Article.Create("title-4--author-3", new DateTime(2021, 06, 22), author3);

            bloggerDbContext.Articles.Add(article1);
            bloggerDbContext.Articles.Add(article2);
            bloggerDbContext.Articles.Add(article3);
            bloggerDbContext.Articles.Add(article4);

            bloggerDbContext.SaveChanges();
        }
    }
}