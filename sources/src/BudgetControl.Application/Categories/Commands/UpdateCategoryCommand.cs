
using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Categories.Commands;

public record UpdateCategoryCommand(
    string Title,
    string Description,
    string CategoryType,
    IEnumerable<UpdateSubcategoryCommand> Subcategories) : ICommand
{
    public Guid Id { get; private set; }

    public void SetId(Guid id)
    {
        Id = id;
    }
}
