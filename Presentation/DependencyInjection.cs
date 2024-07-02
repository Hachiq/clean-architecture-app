using Microsoft.Extensions.DependencyInjection;
using Presentation.Authorization.Filters;

namespace Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthorizationFilters();

            return services;
        }

        private static IServiceCollection AddAuthorizationFilters(this IServiceCollection services)
        {
            services.AddTransient<UsernameTakenFilter>();
            services.AddTransient<EmailTakenFilter>();
            services.AddTransient<UsernameInvalidFilter>();
            services.AddTransient<PasswordTooShortFilter>();

            services.AddTransient<UserNotFoundOrWrongPasswordFilter>();

            services.AddTransient<RefreshTokenInvalidFilter>();

            return services;
        }
    }
}
