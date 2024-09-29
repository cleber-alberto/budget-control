using BudgetControl.Domain.Accounts;
using BudgetControl.Domain.Enumerations;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Tests.Domain.Accounts;

[Collection("Accounts"), Trait(nameof(Account), "Unit"), ExcludeFromCodeCoverage]
public class AccountTests
{
    private const string MaxLenght = "MaxLenght";
    private readonly IFixture _fixture = null!;

    public AccountTests()
    {
        _fixture = new Fixture();

        var currency = _fixture.Create<Currency>();
        var balance = _fixture.Create<decimal>();
        var limit = balance * 2;

        _fixture.Customize<Account>(c => c.FromFactory(() => new AccountBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType(_fixture.Create<AccountType>())
            .WithBalance(balance, currency)
            .WithLimit(limit, currency)
            .Build()));
    }

    [Fact]
    public void Create_ValidArguments_ReturnsAccount()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = _fixture.Create<AccountType>();
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        // Act
        var result = Account.Create(title, description, type, balance, limit);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxLenght)]
    public void Create_InvalidTitle_ReturnsErrors(string title)
    {
        // Arrange
        var description = _fixture.Create<string>();
        var type = _fixture.Create<AccountType>();
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        if (title == MaxLenght)
        {
            title = new string('a', 256);
        }

        // Act
        var result = Account.Create(title, description, type, balance, limit);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxLenght)]
    public void Create_InvalidDescription_ReturnsErrors(string description)
    {
        // Arrange
        var title = _fixture.Create<string>();
        var type = _fixture.Create<AccountType>();
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        if (description == MaxLenght)
        {
            description = new string('a', 2048);
        }

        // Act
        var result = Account.Create(title, description, type, balance, limit);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_InvalidType_ReturnsErrors()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = AccountType.None;
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        // Act
        var result = Account.Create(title, description, type, balance, limit);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_ValidArguments_ReturnsAccount()
    {
        // Arrange
        var accountId = new AccountId(Guid.NewGuid());
        var account = _fixture.Create<Account>();
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = _fixture.Create<AccountType>();
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        // Act
        var result = account.Update(accountId, title, description, type, balance, limit);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxLenght)]
    public void Update_InvalidTitle_ReturnsErrors(string title)
    {
        // Arrange
        var accountId = new AccountId(Guid.NewGuid());
        var account = _fixture.Create<Account>();
        var description = _fixture.Create<string>();
        var type = _fixture.Create<AccountType>();
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        if (title == MaxLenght)
        {
            title = new string('a', 256);
        }

        // Act
        var result = account.Update(accountId, title, description, type, balance, limit);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxLenght)]
    public void Update_InvalidDescription_ReturnsErrors(string description)
    {
        // Arrange
        var accountId = new AccountId(Guid.NewGuid());
        var account = _fixture.Create<Account>();
        var title = _fixture.Create<string>();
        var type = _fixture.Create<AccountType>();
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        if (description == MaxLenght)
        {
            description = new string('a', 2048);
        }

        // Act
        var result = account.Update(accountId, title, description, type, balance, limit);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_InvalidType_ReturnsErrors()
    {
        // Arrange
        var accountId = new AccountId(Guid.NewGuid());
        var account = _fixture.Create<Account>();
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var type = AccountType.None;
        var balance = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;
        var limit = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

        // Act
        var result = account.Update(accountId, title, description, type, balance, limit);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    // [Fact]
    // public void Deposit_ValidArguments_ReturnsSuccess()
    // {
    //     // Arrange
    //     var account = _fixture.Create<Account>();
    //     var amount = Money.Create(_fixture.Create<decimal>(), _fixture.Create<Currency>()).Value;

    //     // Act
    //     var result = account.Deposit(amount);

    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    // }

    // [Fact]
    // public void Withdraw_ValidArguments_ReturnsSuccess()
    // {
    //     // Arrange
    //     var account = _fixture.Create<Account>();
    //     var amount = Money.Create(account.Balance.Value, _fixture.Create<Currency>()).Value;

    //     // Act
    //     var result = account.Withdraw(amount);

    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    // }

    [Fact]
    public void Withdraw_NotEnoughBalance_ReturnsErrors()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        var amount = account.Balance + account.Limit;

        // Act
        var result = account.Withdraw(amount);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Transfer_ValidArguments_ReturnsSuccess()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        var destination = _fixture.Create<Account>();
        var amount = account.Balance;

        // Act
        var result = account.Transfer(destination, amount);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Transfer_NotEnoughBalance_ReturnsErrors()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        var destination = _fixture.Create<Account>();
        var amount = account.Balance + account.Limit;

        // Act
        var result = account.Transfer(destination, amount);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void ToString_ReturnsAccountName()
    {
        // Arrange
        var account = _fixture.Create<Account>();

        // Act
        var result = account.ToString();

        // Assert
        result.Should().Be($"{account.GetType().Name}({account.Id})");
    }
}
