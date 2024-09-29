namespace BudgetControl.Domain.Categories;

public interface ICategoryRepository : IRepository<Category, CategoryId>
{
    Task<Category?> GetByIdWithReferencesAsync(CategoryId id, CancellationToken cancellationToken);
    Task AddSubcategoryAsync(Subcategory entity, CancellationToken cancellationToken = default);
    void UpdateSubcategory(Subcategory entity);
    void DeleteSubcategory(Subcategory entity);

}
