using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskTrackerWebAPI.Exceptions;

namespace TaskTrackerWebAPI.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly Dictionary<Type, Func<IActionResult>> _resultFactories = new()
        {
            { typeof(NotFoundException), () => new NotFoundResult() },
            { typeof(ForbiddenAccessException), () => new ForbidResult() }
        };
        
        public override void OnException(ExceptionContext context)
        {
            if (!_resultFactories.TryGetValue(context.Exception.GetType(), out var resultFactory)) 
                return;
            
            context.Result = resultFactory.Invoke();
            context.ExceptionHandled = true;
        }
    }
}