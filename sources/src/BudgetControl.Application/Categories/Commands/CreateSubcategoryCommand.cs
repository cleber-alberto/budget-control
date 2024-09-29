
namespace BudgetControl.Application.Categories.Commands;

[ExcludeFromCodeCoverage]
public record CreateSubcategoryCommand(
    string Title,
    string Description) : ICommand<Guid>
{
    public Guid CategoryId { get; protected set; }

    public void SetCategoryId(Guid categoryId) => CategoryId = categoryId;
}
