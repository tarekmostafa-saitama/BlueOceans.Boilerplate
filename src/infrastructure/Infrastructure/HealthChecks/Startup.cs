using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks;

internal static class Startup
{
    internal static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddHealthChecksUI().AddInMemoryStorage();

        var healthBuilder = services.AddHealthChecks();

        healthBuilder.AddCheck("Api Health Check", () => HealthCheckResult.Healthy(), new List<string> { "app" });

        healthBuilder.AddSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is missing"),
            tags: new List<string> { "database", "rational", "ado" }, name: $"Database Health Check");


        healthBuilder.AddDbContextCheck<ApplicationDbContext>("Default DbContext Health Check", null,
            new List<string> { "database", "rational", "orm" });

        return services;
    }
}