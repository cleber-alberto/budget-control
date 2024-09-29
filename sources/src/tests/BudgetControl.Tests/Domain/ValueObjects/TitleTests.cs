
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Tests.Domain.ValueObjects;

[Collection("ValueObjects"), Trait(nameof(Title), "Unit"), ExcludeFromCodeCoverage]
public class TitleTests
{
    const string MaxLenght = "MaxLenght";
    private readonly IFixture _fixture = new Fixture();


    public TitleTests()
    {
        _fixture.Customize<Title>(c => c.FromFactory(() => new Title(_fixture.Create<string>())));
    }

    [Fact]
    public void Create_ValidArguments_ReturnsTitle()
    {
        // Arrange
        var value = _fixture.Create<string>();

        // Act
        var result = Title.Create(value);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null!)]
    [InlineData(MaxLenght)]
    public void Create_InvalidArguments_ReturnsErrors(string value)
    {
        // Arrange
        if (value == MaxLenght)
        {
            value = new string('a', 256);
        }

        // Act
        var result = Title.Create(value);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Equals_ValidArguments_ReturnsTrue()
    {
        // Arrange
        var title = _fixture.Create<Title>();

        // Act
        var result = title.Equals(title);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_InvalidArguments_ReturnsFalse()
    {
        // Arrange
        var title = _fixture.Create<Title>();
        var other = _fixture.Create<Title>();

        // Act
        var result = title.Equals(other);

        // Assert
        result.Should().BeFalse();
    }
}
