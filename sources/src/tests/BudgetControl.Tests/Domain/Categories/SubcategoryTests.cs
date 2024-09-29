using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;

namespace BudgetControl.Tests.Domain.Categories;

[Collection("Categories"), Trait(nameof(Subcategory), "Unit"), ExcludeFromCodeCoverage]
public class SubcategoryTests
{
    private readonly IFixture _fixture = new Fixture();

    public SubcategoryTests()
    {
        _fixture.Customize<Category>(c => c.FromFactory(() => new CategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType("Credit")
            .Build()));

        _fixture.Customize<Subcategory>(c => c.FromFactory(() => new SubcategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithCategory(_fixture.Create<Category>())
            .Build()));

    }

    [Fact]
    public void Create_ValidArguments_ReturnsSubcategory()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var category = _fixture.Create<Category>();

        // Act
        var result = Subcategory.Create(title, description, category);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsErrors()
    {
        // Arrange
        var title = string.Empty;
        var description = string.Empty;
        var category = _fixture.Create<Category>();

        // Act
        var result = Subcategory.Create(title, description, category);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_ValidArguments_ReturnsSubcategory()
    {
        // Arrange
        var subcategoryId = new SubcategoryId(Guid.NewGuid());
        var subcategory = _fixture.Create<Subcategory>();
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();

        // Act
        var result = subcategory.Update(subcategoryId, title, description);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Update_InvalidArguments_ReturnsErrors()
    {
        // Arrange
        var subcategoryId = new SubcategoryId(Guid.NewGuid());
        var subcategory = _fixture.Create<Subcategory>();
        var title = string.Empty;
        var description = string.Empty;

        // Act
        var result = subcategory.Update(subcategoryId, title, description);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }
}
