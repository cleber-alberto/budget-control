using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Categories.Queries;

public record FindCategoryQuery(CategoryId CategoryId) : IQuery<CategoryResponse>;
