using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Categories.Queries;

[ExcludeFromCodeCoverage]
public record FindCategoryQuery(CategoryId CategoryId) : IQuery<CategoryResponse>;
