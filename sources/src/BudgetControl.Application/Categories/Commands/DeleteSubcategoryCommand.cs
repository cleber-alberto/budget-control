
namespace BudgetControl.Application.Categories.Commands;

[ExcludeFromCodeCoverage]
public record DeleteSubcategoryCommand(Guid Id) : ICommand
{
    public Guid CategoryId { get; protected set; }

    public void SetCategoryId(Guid categoryId) => CategoryId = categoryId;
}
