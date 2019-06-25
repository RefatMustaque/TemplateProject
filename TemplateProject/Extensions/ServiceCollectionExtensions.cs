using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateProject.Data;
using TemplateProject.Helpers;
using TemplateProject.Manager.Managers;
using TemplateProject.Repository.Repositories;
using TemplateProject.UnitOfWorks;

namespace TemplateProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlServerConnectionService(this IServiceCollection services, IConfiguration configuration)
        {
            NetworkHelper networkHelper = new NetworkHelper();

            string localPcIpAddress = networkHelper.GetLocalIpAddress();

            string connectionString = string.Empty;

            //Refat Mustaque PC IP
            switch (localPcIpAddress)
            {
                case "192.168.0.149":
                    connectionString = configuration.GetConnectionString("RefatMustaqueConnection");
                    break;
                case "192.168.0.103":
                    connectionString = configuration.GetConnectionString("RefatMustaqueHomeConnection");
                    break;
                default:
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                    break;
            }

            services.AddDbContext<TemplateProjectDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static void AddIdentityOptionService(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            });
        }

        public static void AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                ClaimHelper claimHelper = new ClaimHelper();

                var claimListForRole = claimHelper.GetAllAvailableClaims();

                foreach (var claim in claimListForRole)
                {
                    option.AddPolicy(claim.Value, policy => policy.RequireClaim(claim.Value));
                }

                var claimListForUser = claimHelper.GetAllAvailableClaims();

                foreach (var claim in claimListForUser)
                {
                    option.AddPolicy(claim.Value, policy => policy.RequireClaim(claim.Value));
                }
            });
        }

        public static void AddHelperDependancyService(this IServiceCollection services)
        {
            services.AddTransient<DateTimeHelper>();

            services.AddTransient<FileHelper>();

            services.AddTransient<DropdownHelper>();

            services.AddTransient<NetworkHelper>();

            services.AddTransient<StringGeneratorHelper>();

            services.AddTransient<ClaimHelper>();
        }

        public static void AddRepositoryDependancyService(this IServiceCollection services)
        {
            services.AddScoped<LoginHistoryRepository>();

            services.AddScoped<BranchRepository>();

            services.AddScoped<EmployeeRepository>();

            services.AddScoped<UnitOfWork>();

        }

        public static void AddManagerDependancyService(this IServiceCollection services)
        {
            services.AddScoped<LoginHistoryManager>();

            services.AddScoped<BranchManager>();

            services.AddScoped<EmployeeManager>();
        }

    }
}
