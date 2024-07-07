using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class EmailAlreadyConfirmedFilter : IAsyncActionFilter
    {
        private readonly IUsersRepository _userRepository;

        public EmailAlreadyConfirmedFilter(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Query.TryGetValue("userId", out var id))
            {
                var user = await _userRepository.GetByIdAsync(Guid.Parse(id));
                if (user is null)
                {
                    context.Result = new BadRequestObjectResult("User not found.");
                    return;
                }
                if (user.EmailConfirmed)
                {
                    context.Result = new BadRequestObjectResult("Email was already confirmed");
                    return;
                }
            }

            await next();
        }
    }
}
