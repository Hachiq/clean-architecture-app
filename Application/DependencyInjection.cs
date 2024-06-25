using Application.Services.AccessTokenService;
using Application.Services.DateTimeProvider;
using Application.Services.PasswordService;
using Application.Services.RefreshTokenService;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IAccessTokenService, AccessTokenService>();

            services.AddScoped<IPasswordService, PasswordService>();

            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}
