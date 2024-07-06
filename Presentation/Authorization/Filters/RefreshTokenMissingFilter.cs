using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class RefreshTokenMissingFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Cookies.ContainsKey("refreshToken"))
            {
                context.Result = new BadRequestObjectResult("Refresh token is missing.");
                return;
            }
            await next();
        }
    }
}
