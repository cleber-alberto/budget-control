namespace BudgetControl.Domain.Categories;

public class Category : Entity, IAggregateRoot
{
    public CategoryId Id { get; protected set; } = null!;
    public Title Title { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public CategoryType Type { get; private set; } = null!;
    public ICollection<Subcategory> Subcategories { get; private set; } = [];

    public static Result<Category> Create(string title, string description, string type)
    {
        var category = new Category();
        var result = category.Update(new CategoryId(Guid.NewGuid()), title, description, type);

        if (result.IsFailure)
        {
            return Result.Failures<Category>(result.Errors);
        }

        return Result.Success(result.Value);
    }

    public Result<Category> Update(CategoryId categoryId, string title, string description, string type)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);
        var typeResult = CategoryType.FromId<CategoryType>(type);

        var result = Result.Combine(titleResult, descriptionResult, typeResult);
        if(result.IsFailure)
        {
            return Result<Category>.Failures(result.Errors);
        }

        Id = categoryId;
        Title = titleResult.Value;
        Description = descriptionResult.Value;
        Type = typeResult.Value;

        return Result<Category>.Success(this);
    }

    public void AddSubcategory(Subcategory subcategory)
    {
        Subcategories.Add(subcategory);
    }

    public override string ToString() => $"{GetType().Name}({Id})";

    public override int GetHashCode() => HashCode.Combine(Id, Title, Type);
}
