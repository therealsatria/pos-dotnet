using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructures.ResponseBuilder
{
    public static class ResponseBuilder
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
        
        public static IActionResult ValidationError(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return new BadRequestObjectResult(new ApiResponse<object> 
            { 
                Success = false, 
                Message = "Validation Failed", 
                Errors = errors 
            });
        }
    }
}
