using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class PasswordTooShortFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("request", out var value) && value is IPasswordRequest request)
            {
                if (request.Password.Length < 4)
                {
                    context.Result = new BadRequestObjectResult("Password too short.");
                    return;
                }
            }

            await next();
        }
    }
}
