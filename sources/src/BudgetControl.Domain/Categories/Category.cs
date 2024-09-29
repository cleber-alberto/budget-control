using System.Diagnostics.CodeAnalysis;

namespace BudgetControl.Domain.Categories;

public class Category : Entity, IEquatable<Category>
{
    public CategoryId Id { get; protected set; } = null!;
    public Title Title { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public CategoryType CategoryType { get; private set; } = null!;
    public ICollection<Subcategory> Subcategories { get; private set; } = [];

    public static Result<Category> Create(string title, string description, CategoryType categoryType)
    {
        var category = new Category();
        var result = category.Update(new CategoryId(Guid.NewGuid()), title, description, categoryType);

        if (result.IsFailure)
        {
            return Result.Failures<Category>(result.Errors);
        }

        return Result.Success(result.Value);
    }

    public Result<Category> Update(CategoryId categoryId, string title, string description,  CategoryType categoryType)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);

        var result = Result.Combine(titleResult, descriptionResult);
        if(result.IsFailure)
        {
            return Result<Category>.Failures(result.Errors);
        }

        Id = categoryId;
        Title = titleResult.Value;
        Description = descriptionResult.Value;
        CategoryType = categoryType;

        return Result<Category>.Success(this);
    }

    public Result<Subcategory> CreateSubcategory(string title, string description)
    {
        return UpdateSubcategory(new SubcategoryId(Guid.NewGuid()), title, description);
    }

    public Result<Subcategory> UpdateSubcategory(SubcategoryId id, string title, string description)
    {
        Result<Subcategory> subcategoryResult = null!;
        var subcategory = Subcategories.FirstOrDefault(x => x.Id.Equals(id));

        if (subcategory is null)
        {
            subcategoryResult = Subcategory.Create(title, description, this);
            if (subcategoryResult.IsSuccess)
            {
                Subcategories.Add(subcategoryResult.Value);
            }
        }
        else
        {
            subcategoryResult = subcategory.Update(title, description);
        }

        return subcategoryResult.IsFailure
            ? Result<Subcategory>.Failures(subcategoryResult.Errors)
            : Result<Subcategory>.Success(subcategoryResult.Value);
    }

    public override string ToString() => $"{GetType().Name}({Id})";

    public bool Equals(Category? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Id.Equals(other.Id);
    }

    public static int GetHashCode([DisallowNull] Category obj) => HashCode.Combine(obj.Id, obj.Title, obj.CategoryType);
}
