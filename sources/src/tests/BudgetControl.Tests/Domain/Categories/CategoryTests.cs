using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;

namespace BudgetControl.Tests.Domain.Categories;

[Collection("Categories"), Trait(nameof(Category), "Unit"), ExcludeFromCodeCoverage]
public class CategoryTests
{
    private readonly IFixture _fixture = new Fixture();

    public CategoryTests()
    {

        _fixture.Customize<Category>(c => c.FromFactory(() => new CategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType("Credit")
            .Build()));
    }

    [Fact]
    public void Create_ValidArguments_ReturnsCategory()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = "Credit";

        // Act
        var result = Category.Create(title, description, type);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsErrors()
    {
        // Arrange
        var title = string.Empty;
        var description = string.Empty;
        var type = "InvalidType";

        // Act
        var result = Category.Create(title, description, type);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_ValidArguments_ReturnsCategory()
    {
        // Arrange
        var categoryId = new CategoryId(Guid.NewGuid());
        var category = _fixture.Create<Category>();
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = "Credit";

        // Act
        var result = category.Update(categoryId, title, description, type);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Update_InvalidArguments_ReturnsErrors()
    {
        // Arrange
        var categoryId = new CategoryId(Guid.NewGuid());
        var category = _fixture.Create<Category>();
        var title = string.Empty;
        var description = string.Empty;
        var type = "InvalidType";

        // Act
        var result = category.Update(categoryId, title, description, type);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void ToString_ReturnsCategoryName()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        var result = category.ToString();

        // Assert
        result.Should().Be($"{category.GetType().Name}({category.Id})");
    }

    [Fact]
    public void Equals_SameCategory_ReturnsTrue()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        var result = category.Equals(category);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ReturnsHashCode()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        var result = category.GetHashCode();

        // Assert
        result.Should().Be(HashCode.Combine(category.Id, category.Title, category.Type));
    }

    [Fact]
    public void Delete_SetsIsDeletedToTrue()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        category.SetDeleted();

        // Assert
        category.IsDeleted.Should().BeTrue();
    }
}
