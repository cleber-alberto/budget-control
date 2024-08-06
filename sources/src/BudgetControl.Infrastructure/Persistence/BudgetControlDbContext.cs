using BudgetControl.Domain.Categories;

namespace BudgetControl.Infrastructure.Persistence;

public class BudgetControlDbContext(DbContextOptions<BudgetControlDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetControlDbContext).Assembly);
    }
}
