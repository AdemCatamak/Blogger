using System;
using System.Linq;
using Blogger.Domain.Exceptions;

namespace Blogger.Domain
{
    public class Author
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private Author(Guid id, string firstName, string lastName, DateTime createdOn)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            CreatedOn = createdOn;
        }

        public static Author Create(string firstName, string lastName)
        {
            firstName = firstName?.Trim() ?? string.Empty;
            lastName = lastName?.Trim() ?? string.Empty;

            if (!firstName.Any())
            {
                throw new ValidationException("First name should not be empty");
            }

            if (!lastName.Any())
            {
                throw new ValidationException("Last name should not be empty");
            }

            var author = new Author(Guid.NewGuid(), firstName, lastName, DateTime.UtcNow);
            return author;
        }
    }
}