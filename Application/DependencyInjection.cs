using Application.Services.AccessTokenService;
using Application.Services.DateTimeProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IAccessTokenService, AccessTokenService>();

            return services;
        }
    }
}
