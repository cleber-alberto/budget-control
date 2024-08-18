namespace BudgetControl.Application.Categories;

public record CategoryResponse(
    string Title,
    string Description,
    Guid? ParentCategoryId);
