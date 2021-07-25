namespace Blogger.Domain.Exceptions
{
    public abstract class NotFoundException : CustomException
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }

    public class NotFoundException<T> : NotFoundException
    {
        public NotFoundException() : base($"{typeof(T).GetFriendlyName()} could not found")
        {
        }
    }
}