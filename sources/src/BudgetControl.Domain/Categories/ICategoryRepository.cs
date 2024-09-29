namespace BudgetControl.Domain.Categories;

public interface ICategoryRepository : IRepository<Category, CategoryId>
{
    Task<Category> GetByIdAsync(CategoryId id, string reference, CancellationToken cancellationToken);
    Task<Subcategory> GetSubcategoryByIdAsync(SubcategoryId id, string reference, CancellationToken cancellationToken);
}
