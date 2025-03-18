using Microsoft.AspNetCore.Mvc;

namespace Infrastructures.ResponseBuilder
{
    public class ResponseBuilder
    {
        public static IActionResult Success<T>(T data, string message = "Success")
        {
            return new OkObjectResult(new ApiResponse<T> { Success = true, Message = message, Data = data });
        }

        public static IActionResult Error(string message, int statusCode = 400)
        {
            return new ObjectResult(new ApiResponse<object> { Success = false, Message = message }) { StatusCode = statusCode };
        }

        public static IActionResult NotFound(string message = "Not Found")
        {
            return new NotFoundObjectResult(new ApiResponse<object> { Success = false, Message = message });
        }      
    }
}
