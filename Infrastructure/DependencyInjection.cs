﻿using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Users;
using Application.Repositories;
using Infrastructure.Data;
using Infrastructure.EmailService;
using Infrastructure.Persistence.Ads;
using Infrastructure.Persistence.Categories;
using Infrastructure.Persistence.RefreshTokens;
using Infrastructure.Persistence.Roles;
using Infrastructure.Persistence.Sections;
using Infrastructure.Persistence.SubCategories;
using Infrastructure.Persistence.UserRoles;
using Infrastructure.Persistence.Users;
using Infrastructure.Security.TokenGenerator;
using Infrastructure.Security.TokenValidator;
using Infrastructure.Services;
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

            services.AddScoped<IUserRoleService, UserRoleService>();

            services.AddScoped<IUserService, UserService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAdsRepository, AdsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<ISectionsRepository, SectionsRepository>();
            services.AddScoped<ISubCategoriesRepository, SubCategoriesRepository>();
            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

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

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.ConfigureOptions<JwtBearerTokenValidationConfiguration>()
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}