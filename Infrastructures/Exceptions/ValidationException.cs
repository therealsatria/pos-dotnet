namespace Infrastructures.Exceptions
{
    public class ValidationException : ApiException
    {
        public ValidationException(string message) : base(message, 400) { }
    }
}