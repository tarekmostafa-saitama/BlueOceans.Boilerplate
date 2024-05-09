using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BackgroundJobs;



public static class Startup
{
    internal static IServiceCollection AddBackgroundServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseHangfire"))
        {
            if (configuration.GetValue<bool>("UseSeperateHangfireDb"))
                services.AddHangfire(x =>
                    x.UseSqlServerStorage(configuration.GetValue<string>("DefaultHangFireConnection")));
            else
                services.AddHangfire(x =>
                    x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();
        }

        return services;
    }

    internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseHangfire"))
            return app.UseHangfireDashboard("/HangFire", new DashboardOptions
            {
                Authorization = new[] { new HangFireFilter() }
            });
        return app;
    }
}