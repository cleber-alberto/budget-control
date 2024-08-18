using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetControl.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class BudgedControlContextFactory : IDesignTimeDbContextFactory<BudgetControlDbContext>
{
    public BudgetControlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BudgetControlDbContext>();
        optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=BudgetControl;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=sa;Password=~Fn8ScPVbRVrU6eZD2.Rez");

        return new BudgetControlDbContext(optionsBuilder.Options);
    }
}
