using BudgetControl.Common.Primitives.DomainObjects;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Options;

namespace BudgetControl.Infrastructure.Persistence;

public class BudgetControlDbContext(DbContextOptions<BudgetControlDbContext> options)
    : DbContext(options), IDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetControlDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.ComplexProperties<StronglyTypeId<Guid>>();
    }
}
