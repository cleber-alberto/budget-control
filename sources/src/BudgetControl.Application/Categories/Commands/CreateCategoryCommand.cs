using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Categories.Commands;

public record CreateCategoryCommand(
    string Title,
    string Description,
    string CategoryType,
    Guid? ParentId) : ICommand<Guid>;
