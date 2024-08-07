﻿using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class UserNotFoundOrWrongPasswordFilter : IAsyncActionFilter
    {
        private readonly IUsersRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserNotFoundOrWrongPasswordFilter(
            IUsersRepository userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("request", out var value) && value is LoginRequest request)
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user is null || !_passwordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    context.Result = new BadRequestObjectResult(new { reason = "NoMatch" });
                    return;
                }
            }

            await next();
        }
    }
}
