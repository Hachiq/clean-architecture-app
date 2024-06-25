using Application.Services.AccessTokenService;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccessTokenService, AccessTokenService>();

            return services;
        }
    }
}
