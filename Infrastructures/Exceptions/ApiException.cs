using System;

namespace Infrastructures.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public object? ErrorDetails { get; set; }

        public ApiException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(string message, Exception innerException, int statusCode = 500) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public ApiException(string message, object errorDetails, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
            ErrorDetails = errorDetails;
        }
    }
}