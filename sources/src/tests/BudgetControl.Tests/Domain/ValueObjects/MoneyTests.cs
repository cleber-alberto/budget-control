
using BudgetControl.Domain.Enumerations;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Tests.Domain.ValueObjects;

[Collection("ValueObjects"), Trait(nameof(Money), "Unit"), ExcludeFromCodeCoverage]
public class MoneyTests
{
    private readonly IFixture _fixture = new Fixture();

    public MoneyTests()
    {
        _fixture.Customize<Money>(c => c.FromFactory(() => new Money(_fixture.Create<decimal>(), _fixture.Create<Currency>())));
    }

    [Fact]
    public void Create_ValidArguments_ReturnsMoney()
    {
        // Arrange
        var value = _fixture.Create<decimal>();
        var currency = _fixture.Create<Currency>();

        // Act
        var result = Money.Create(value, currency);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsErrors()
    {
        // Arrange
        var value = 0m;
        var currency = _fixture.Create<Currency>();

        // Act
        var result = Money.Create(value, currency);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Equals_ValidArguments_ReturnsTrue()
    {
        // Arrange
        var money = _fixture.Create<Money>();

        // Act
        var result = money.Equals(money);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var money1 = Money.Create(1, Currency.CAD).Value;
        var money2 = new Money(2, Currency.CAD);

        // Act
        var result = money1.Equals(money2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SomeOperator_ValidArguments_ReturnsMoney()
    {
        // Arrange
        var money1 = Money.Create(1, Currency.CAD).Value;
        var money2 = Money.Create(2, Currency.CAD).Value;

        // Act
        var result = money1 + money2;

        // Assert
        result.Value.Should().Be(3);
    }

    [Fact]
    public void SubstractionOperator_ValidArguments_ReturnsMoney()
    {
        // Arrange
        var money1 = Money.Create(1, Currency.CAD).Value;
        var money2 = Money.Create(2, Currency.CAD).Value;

        // Act
        var result = money1 - money2;

        // Assert
        result.Value.Should().Be(-1);
    }

    [Fact]
    public void MultiplicationOperator_ValidArguments_ReturnsMoney()
    {
        // Arrange
        var money1 = Money.Create(1, Currency.CAD).Value;
        var money2 = Money.Create(2, Currency.CAD).Value;

        // Act
        var result = money1 * money2;

        // Assert
        result.Value.Should().Be(2);
    }

    [Fact]
    public void DivisionOperator_ValidArguments_ReturnsMoney()
    {
        // Arrange
        var money1 = Money.Create(1, Currency.CAD).Value;
        var money2 = Money.Create(2, Currency.CAD).Value;

        // Act
        var result = money1 / money2;

        // Assert
        result.Value.Should().Be(0.5m);
    }

    [Fact]
    public void MoreThan_ValidArguments_ReturnsTrue()
    {
        // Arrange
        var money1 = Money.Create(2, Currency.CAD).Value;
        var money2 = Money.Create(1, Currency.CAD).Value;

        // Act
        var result = money1.MoreThan(money2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MoreThan_InvalidArguments_ReturnsFalse()
    {
        // Arrange
        var money1 = Money.Create(1, Currency.CAD).Value;
        var money2 = Money.Create(2, Currency.CAD).Value;

        // Act
        var result = money1.MoreThan(money2);

        // Assert
        result.Should().BeFalse();
    }
}
