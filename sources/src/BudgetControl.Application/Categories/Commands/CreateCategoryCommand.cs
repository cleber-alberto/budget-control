
namespace BudgetControl.Application.Categories.Commands;

public record CreateCategoryCommand(
    string Title,
    string Description,
    string CategoryType,
    IEnumerable<CreateSubcategoryCommand> Subcategories) : ICommand<Guid>;
