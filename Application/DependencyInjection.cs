using Application.Services.PasswordService;
using Application.Services.RefreshTokenService;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}
