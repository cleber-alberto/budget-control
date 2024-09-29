
namespace BudgetControl.Application.Categories.Commands;

[ExcludeFromCodeCoverage]
public record UpdateCategoryCommand(
    string Title,
    string Description,
    string Type) : ICommand
{
    public Guid Id { get; private set; }

    public void SetId(Guid id)
    {
        Id = id;
    }
}
