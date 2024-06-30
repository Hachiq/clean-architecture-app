﻿using Microsoft.Extensions.DependencyInjection;

namespace Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }
    }
}
