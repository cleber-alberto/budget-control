using BudgetControl.Domain.Categories;

namespace BudgetControl.Infrastructure.Persistence.Repositories;

public class CategoryRepository(IDbContext dbContext)
        : Repository<Category, CategoryId>(dbContext), ICategoryRepository
{
    private readonly IDbContext _dbContext = dbContext;

    public Task<Category?> GetByIdWithReferencesAsync(CategoryId id, CancellationToken cancellationToken) =>
         _dbContext.Set<Category>()
            .Include(x => x.Subcategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Subcategory?> GetSubcategoryByIdAsync(SubcategoryId id, string reference, CancellationToken cancellationToken) =>
        _dbContext.Set<Subcategory>()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddSubcategoryAsync(Subcategory entity, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Subcategory>().AddAsync(entity, cancellationToken);

    public void DeleteSubcategory(Subcategory entity) =>
        _dbContext.Set<Subcategory>().Remove(entity);

    public void UpdateSubcategory(Subcategory entity) =>
        _dbContext.Set<Subcategory>().Update(entity);
}
