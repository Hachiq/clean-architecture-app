using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class UsernameInvalid : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("request", out var value) && value is RegisterRequest request)
            {
                if (request.Username.Length < 4 || request.Username.Length > 50)
                {
                    context.Result = new BadRequestObjectResult("Invalid username.");
                    return;
                }
            }

            await next();
        }
    }
}
