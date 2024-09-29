using System.Data;

namespace BudgetControl.Domain.Categories;

public class Subcategory : Entity
{
    public SubcategoryId Id { get; protected set; } = null!;
    public Title Title { get; protected set; } = null!;
    public Description Description { get; protected set; } = null!;
    public Category Category { get;  protected set; } = null!;

    public static Result<Subcategory> Create(string title, string description, Category category)
    {
        var subcategory = new Subcategory
        {
            Category = category
        };

        var result = subcategory.Update(new SubcategoryId(Guid.NewGuid()), title, description);

        if (result.IsFailure)
        {
            return Result.Failures<Subcategory>(result.Errors);
        }

        return Result.Success(result.Value);
    }

    public Result<Subcategory> Update(SubcategoryId id, string title, string description)
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

        Id = id;
        Title = titleResult.Value;
        Description = descriptionResult.Value;

        return Result.Success(this);
    }
}
