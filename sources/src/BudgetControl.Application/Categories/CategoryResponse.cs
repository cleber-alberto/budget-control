namespace BudgetControl.Application.Categories;

[ExcludeFromCodeCoverage]
public record CategoryResponse(
    Guid Id,
    string Title,
    string Description,
    string Type,
    IEnumerable<SubcategoryResponse> Subcategories);
