using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Categories.Commands;

public record CreateCategoryCommand(
    string Name,
    string Description,
    string CategoryType,
    Guid? ParentId) : ICommand<CategoryId>;
