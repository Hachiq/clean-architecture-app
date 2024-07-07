using Application.Interfaces;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Authorization.Filters
{
    public class RefreshTokenInvalidFilter : IAsyncActionFilter
    {
        private readonly IRefreshTokensRepository _refreshTokenRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshTokenInvalidFilter(
            IRefreshTokensRepository refreshTokenRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("refreshToken", out var token))
            {
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new BadRequestObjectResult("Refresh token is invalid.");
                    return;
                }
                var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
                if (refreshToken is null)
                {
                    context.Result = new BadRequestObjectResult("Refresh token does not exists in the database.");
                    return;
                }
                if (refreshToken.ExpiresAt < _dateTimeProvider.UtcNow)
                {
                    context.Result = new BadRequestObjectResult("Refresh token is expired.");
                    return;
                }
            }
            await next();
        }
    }
}
