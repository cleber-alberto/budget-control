
namespace BudgetControl.Application.Categories.Commands;

public record UpdateSubcategoryCommand(
    Guid Id, 
    string Title, 
    string Description);

