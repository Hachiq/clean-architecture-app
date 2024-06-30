using Application.Interfaces;
using Application.Repositories;
using Infrastructure.Data;
using Infrastructure.EmailService;
using Infrastructure.RefreshTokens;
using Infrastructure.Roles;
using Infrastructure.Security.TokenGenerator;
using Infrastructure.Security.TokenValidator;
using Infrastructure.Services;
using Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServices()
                .AddRepositories()
                .AddEmailSender(configuration)
                .AddAuthentication(configuration)
                .AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));

            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddSingleton<IPasswordService, PasswordService>();

            services.AddSingleton<IRefreshTokenService, RefreshTokenService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

            services.AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();

            services.ConfigureOptions<JwtBearerTokenValidationConfiguration>()
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}