namespace Blogger.Domain.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message) : base(message)
        {
            
        }
    }
}