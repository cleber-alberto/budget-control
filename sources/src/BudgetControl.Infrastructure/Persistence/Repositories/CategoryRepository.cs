using BudgetControl.Domain.Categories;

namespace BudgetControl.Infrastructure.Persistence.Repositories;

public class CategoryRepository(DbContext dbContext)
    : Repository<Category, CategoryId>(dbContext), ICategoryRepository
{

}
