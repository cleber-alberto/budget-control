using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Scrutor;

namespace BudgetControl.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class InfrastructureServiceExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        DatabaseConfiguration(services, loggerFactory);

        services.AddScoped<IDbContext, BudgetControlDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

    private static void DatabaseConfiguration(IServiceCollection services, ILoggerFactory loggerFactory)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var pollyRetry = Policy.Handle<Exception>()
            .WaitAndRetry([TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15)]);
                pollyRetry.Execute(() => DatabaseEnsureCreated(scope, loggerFactory));
    }

    private static void DatabaseEnsureCreated(IServiceScope scope, ILoggerFactory loggerFactory)
    {
        var context = scope.ServiceProvider.GetRequiredService<BudgetControlDbContext>();
        context.Database.EnsureCreated();

        if (!context.Database.CanConnect())
        {
            loggerFactory.CreateLogger("InfrastructureServiceExtension").LogInformation("Database is not available. Waiting for handshake with the database...");
        }
        else
        {
            //Console.WriteLine("Database is available");
            loggerFactory.CreateLogger("InfrastructureServiceExtension").LogInformation("Database is available");
            context.Database.Migrate();
        }

        // var migratorService = context.GetInfrastructure().GetRequiredService<IMigrator>();
        // migratorService.Migrate();
    }
}
