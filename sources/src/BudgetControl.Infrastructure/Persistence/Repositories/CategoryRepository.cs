using BudgetControl.Domain.Categories;

namespace BudgetControl.Infrastructure.Persistence.Repositories;

public class CategoryRepository(IDbContext dbContext)
        : Repository<Category, CategoryId>(dbContext), ICategoryRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<Category> GetByIdAsync(CategoryId id, string reference, CancellationToken cancellationToken) =>
         _dbContext.Set<Category>()
            .Include(reference)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Subcategory> GetSubcategoryByIdAsync(SubcategoryId id, string reference, CancellationToken cancellationToken) =>
        _dbContext.Set<Subcategory>()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
