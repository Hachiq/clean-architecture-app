using Application.Interfaces.Authentication;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class UsernameTakenFilter : IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;

        public UsernameTakenFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("request", out var value) && value is RegisterRequest request)
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user is not null)
                {
                    context.Result = new ConflictObjectResult(new { reason = "UsernameTaken" });
                    return;
                }
            }

            await next();
        }
    }
}
