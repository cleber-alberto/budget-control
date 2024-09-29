

using System.Data;

namespace BudgetControl.Domain.Categories;

public class Subcategory(Title title, Description description, Category category)
    : Entity
{
    protected Subcategory() : this(null!, null!, null!) { }

    public SubcategoryId Id { get; protected set; } = null!;
    public Title Title { get; protected set; } = title;
    public Description Description { get; protected set; } = description;
    public Category Category { get;  protected set; } = category;

    public static Result<Subcategory> Create(string title, string description, Category category)
    {

        return Result.Success(new Subcategory(Title.Create(title).Value, Description.Create(description).Value, category));
    }

    public Result<Subcategory> Update(string title, string description)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);

        var result = Result.Combine(
            titleResult,
            descriptionResult);

        if (result.IsFailure)
        {
            return Result.Failures<Subcategory>(result.Errors);
        }

        Title = titleResult.Value;
        Description = descriptionResult.Value;

        return Result.Success(this);
    }
}
