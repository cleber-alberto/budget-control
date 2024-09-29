using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace BudgetControl.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class InfrastructureServiceExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BudgetControlDbContext>();

            if (!context.Database.CanConnect())
            {
                // TODO: Log the message
                Console.WriteLine("Database is not available. Waiting for handshake with the database...");
                Thread.Sleep(5000);

                if (!context.Database.CanConnect())
                {
                    throw new Exception("Database is not available");
                }
            }

            context.Database.EnsureCreated();
            context.Database.Migrate();

            // var migratorService = context.GetInfrastructure().GetRequiredService<IMigrator>();
            // migratorService.Migrate();
        }

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

}
