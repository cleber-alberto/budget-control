using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetControl.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class InfrastructureServiceExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddDbContext<BudgetControlDbContext>(options =>
        //     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped<IBudgetControlDbContext>(provider => provider.GetService<BudgetControlDbContext>());

        return services;
    }

}
