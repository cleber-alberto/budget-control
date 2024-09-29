
namespace BudgetControl.Application.Categories.Commands;

[ExcludeFromCodeCoverage]
public record UpdateSubcategoryCommand(
    string Title,
    string Description) : ICommand
{
    public Guid Id { get; protected set; }
    public Guid CategoryId { get; protected set; }

    public void SetId(Guid id) => Id = id;

    public void SetCategoryId(Guid categoryId) => CategoryId = categoryId;
}
