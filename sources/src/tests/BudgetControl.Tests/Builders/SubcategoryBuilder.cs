using System;
using BudgetControl.Domain.Categories;

namespace BudgetControl.Tests.Builders;

[ExcludeFromCodeCoverage]
public class SubcategoryBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private Category _category = null!;

    public SubcategoryBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public SubcategoryBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public SubcategoryBuilder WithCategory(Category category)
    {
        _category = category;
        return this;
    }

    public Subcategory Build() => Subcategory.Create(_title, _description, _category).Value;
}
