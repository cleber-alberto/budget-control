using BudgetControl.Domain.Categories;

namespace BudgetControl.Tests.Builders;

[ExcludeFromCodeCoverage]
public class CategoryBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _type = null!;

    public CategoryBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public CategoryBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CategoryBuilder WithType(string type)
    {
        _type = type;
        return this;
    }

    public Category Build() => Category.Create(_title, _description, _type).Value;
}
