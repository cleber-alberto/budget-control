
namespace BudgetControl.Application.Categories.Queries;

[ExcludeFromCodeCoverage]
public record GetAllSubcategoriesQuery(Guid CategoryId) : IQuery<IEnumerable<SubcategoryResponse>>;
