
using System.Diagnostics.CodeAnalysis;

namespace BudgetControl.Domain.Categories;

public class Category : Entity, IEquatable<Category>
{
    public CategoryId Id { get; private set; } = null!;
    public Name Name { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public Category? Parent { get; private set; }
    public CategoryType Type { get; private set; } = null!;

    public Result Update(string name, string description,  CategoryType type, Category? parent)
    {
        var nameResult = Name.Create(name);
        var descriptionResult = Description.Create(description);
        var result = Result.Combine(nameResult, descriptionResult);

        if(result.IsFailure)
        {
            return Result<Category>.Failures(result.Errors);
        }

        Name = nameResult.Value;
        Type = type;
        Parent = parent;

        return Result<Category>.Success(this);
    }

    public static Result<Category> Create(string name, string description, CategoryType type, Category? parent)
    {
        var category = new Category();
        var result = category.Update(name, description, type, parent);

        return result.IsSuccess
            ? Result.Success(category)
            : Result.Failures<Category>(result.Errors);
    }

    public override string ToString() => $"{GetType().Name}({Id})";

    public bool Equals(Category? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Id.Equals(other.Id);
    }

    public static int GetHashCode([DisallowNull] Category obj) => HashCode.Combine(obj.Id, obj.Name, obj.Type, obj.Parent);
}
