using Application.Interfaces.Authentication;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class EmailTakenFilter : IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;

        public EmailTakenFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("request", out var value) && value is RegisterRequest request)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user is not null)
                {
                    context.Result = new BadRequestObjectResult("User with such email already exists.");
                    return;
                }
            }
        }
    }
}
