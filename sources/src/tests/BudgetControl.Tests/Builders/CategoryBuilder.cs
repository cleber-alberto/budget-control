using BudgetControl.Domain.Categories;

namespace BudgetControl.Tests.Builders;

[ExcludeFromCodeCoverage]
internal class CategoryBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _type = null!;
    private readonly List<Subcategory> _subcategories = new();

    internal CategoryBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    internal CategoryBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    internal CategoryBuilder WithType(string type)
    {
        _type = type;
        return this;
    }

    internal CategoryBuilder WithSubcategories(Subcategory subcategory)
    {
        _subcategories.Add(subcategory);
        return this;
    }

    internal Category Build()
    {
        var category = Category.Create(_title, _description, _type).Value;
        _subcategories.ForEach(category.AddSubcategory);

        return category;
    }
}
