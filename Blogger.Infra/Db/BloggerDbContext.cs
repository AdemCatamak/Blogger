using Blogger.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Infra.Db
{
    public class BloggerDbContext : DbContext
    {
        public BloggerDbContext(DbContextOptions<BloggerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;
    }
}