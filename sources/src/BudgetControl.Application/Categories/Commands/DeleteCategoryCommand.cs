
namespace BudgetControl.Application.Categories.Commands;

[ExcludeFromCodeCoverage]
public record DeleteCategoryCommand(Guid Id) : ICommand;
