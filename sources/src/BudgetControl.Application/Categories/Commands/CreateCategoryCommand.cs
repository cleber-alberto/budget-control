
namespace BudgetControl.Application.Categories.Commands;

[ExcludeFromCodeCoverage]
public record CreateCategoryCommand(
    string Title,
    string Description,
    string Type) : ICommand<Guid>;
