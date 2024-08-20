using System;

namespace BudgetControl.Domain.Categories;

public class CategoryBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private CategoryType _type = null!;

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

    public CategoryBuilder WithType(CategoryType type)
    {
        _type = type;
        return this;
    }

    public Category Build()
    {
        return Category.Create(_title, _description, _type).Value;
    }
}
