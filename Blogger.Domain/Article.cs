using System;
using System.Linq;
using Blogger.Domain.Exceptions;

namespace Blogger.Domain
{
    public class Article
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public Guid AuthorId { get; private set; }
        public DateTime PublishedOn { get; private set; }

        public DateTime CreatedOn { get; private set; }

        private Article(Guid id, string title, Guid authorId, DateTime publishedOn, DateTime createdOn)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
            PublishedOn = publishedOn;
            CreatedOn = createdOn;
        }

        public static Article Create(string title, DateTime publishedOn, Author author)
        {
            title = title?.Trim() ?? string.Empty;

            if (!title.Any())
            {
                throw new ValidationException("Title should not be empty");
            }

            var article = new Article(Guid.NewGuid(), title, author.Id, publishedOn, DateTime.UtcNow);
            return article;
        }
    }
}