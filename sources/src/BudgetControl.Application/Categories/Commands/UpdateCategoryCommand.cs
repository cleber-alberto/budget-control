
namespace BudgetControl.Application.Categories.Commands;

public record UpdateCategoryCommand(
    string Title,
    string Description,
    string CategoryType,
    Guid? ParentId) : ICommand
{
    public Guid Id { get; private set; }

    public void SetId(Guid id)
    {
        Id = id;
    }
}
