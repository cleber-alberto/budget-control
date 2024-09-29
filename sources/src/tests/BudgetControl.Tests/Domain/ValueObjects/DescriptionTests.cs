
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Tests.Domain.ValueObjects;

[Collection("ValueObjects"), Trait(nameof(Description), "Unit"), ExcludeFromCodeCoverage]
public class DescriptionTests
{
    const string MaxLenght = "MaxLenght";
    private readonly IFixture _fixture = new Fixture();

    public DescriptionTests()
    {
        _fixture.Customize<Description>(c => c.FromFactory(() => new Description(_fixture.Create<string>())));
    }

    [Fact]
    public void Create_ValidArguments_ReturnsDescription()
    {
        // Arrange
        var value = _fixture.Create<string>();

        // Act
        var result = Description.Create(value);

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
            value = new string('a', 1048);
        }

        // Act
        var result = Description.Create(value);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Equals_ValidArguments_ReturnsTrue()
    {
        // Arrange
        var description = _fixture.Create<Description>();

        // Act
        var result = description.Equals(description);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ValidArguments_ReturnsFalse()
    {
        // Arrange
        var description = _fixture.Create<Description>();
        var other = _fixture.Create<Description>();

        // Act
        var result = description.Equals(other);

        // Assert
        result.Should().BeFalse();
    }
}
