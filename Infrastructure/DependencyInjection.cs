using Application.Interfaces;
using Application.Repositories;
using Infrastructure.AccessTokens;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();

            services.AddScoped<IPasswordService, PasswordService>();

            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}