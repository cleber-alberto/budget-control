using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Domain.Categories;

namespace BudgetControl.Infrastructure.Persistence.Repositories;

public class CategoryRepository(IDbContext dbContext)
    : Repository<Category, CategoryId>(dbContext), ICategoryRepository
{

}
