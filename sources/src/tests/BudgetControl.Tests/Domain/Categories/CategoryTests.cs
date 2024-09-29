using AutoFixture;
using BudgetControl.Domain.Categories;
using FluentAssertions;

namespace BudgetControl.Tests.Domain.Categories;

public class CategoryTests
{
    private readonly IFixture _fixture = new Fixture();

    public CategoryTests()
    {

        _fixture.Customize<Category>(c => c.FromFactory(() => new CategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType(_fixture.Create<CategoryType>())
            .Build()));
    }

    [Fact]
    public void Create_ValidArguments_ReturnsCategory()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = _fixture.Create<CategoryType>();

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
        var type = _fixture.Create<CategoryType>();

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
        var type = _fixture.Create<CategoryType>();

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
        var type = _fixture.Create<CategoryType>();

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
    public void Equals_NullCategory_ReturnsFalse()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        var result = category.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentCategory_ReturnsFalse()
    {
        // Arrange
        var category1 = _fixture.Create<Category>();
        var category2 = _fixture.Create<Category>();

        // Act
        var result = category1.Equals(category2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ReturnsHashCode()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        var result = Category.GetHashCode(category);

        // Assert
        result.Should().Be(HashCode.Combine(category.Id, category.Title, category.CategoryType));
    }

    [Fact]
    public void Delete_SetsIsDeletedToTrue()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        // Act
        category.Delete();

        // Assert
        category.IsDeleted.Should().BeTrue();
    }
}
