using System;
using BudgetControl.Domain.Categories;

namespace BudgetControl.Tests.Builders;

[ExcludeFromCodeCoverage]
internal class SubcategoryBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private Category _category = null!;

    internal SubcategoryBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    internal SubcategoryBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    internal SubcategoryBuilder WithCategory(Category category)
    {
        _category = category;
        return this;
    }

    internal Subcategory Build() => Subcategory.Create(_title, _description, _category).Value;
}
