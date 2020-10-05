using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Application.Extensions
{
    public static class ApplicationServiceCollectionExtension
    {
        /// <summary>
        /// Dependency Injection.
        /// </summary>
        /// <param name="services"> Service collection.</param>
        /// <param name="configuration"> Configuration.</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MyProjectApp");
            services.AddDbContext<MyProjectContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyProjectContext>();

            return services;
        }
    }
}
