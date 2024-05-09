using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middlewares;

public static class Startup
{
    internal static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<ExceptionMiddleware.ExceptionMiddleware>();
        return services;
    }

    internal static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware.ExceptionMiddleware>();
        return app;
    }

}