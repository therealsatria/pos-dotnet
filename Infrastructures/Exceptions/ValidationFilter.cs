using Infrastructures.ResponseBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructures.Exceptions
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = ResponseBuilder.ResponseBuilder.ValidationError(context.ModelState);
            }
        }
    }
} 