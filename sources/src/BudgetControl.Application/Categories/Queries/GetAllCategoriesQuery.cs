namespace BudgetControl.Application.Categories.Queries;

[ExcludeFromCodeCoverage]
public record GetAllCategoriesQuery : IQuery<IEnumerable<CategoryResponse>>;
