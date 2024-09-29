using BudgetControl.Domain.Enumerations;

namespace BudgetControl.Domain.Accounts;

public class AccountBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private AccountType _type = AccountType.Checking;
    private Money _balance = Money.Create(decimal.Zero, Currency.None).Value;
    private Money _limit = Money.Create(decimal.Zero, Currency.None).Value;

    public AccountBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public AccountBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public AccountBuilder WithType(AccountType type)
    {
        _type = type;
        return this;
    }

    public AccountBuilder WithBalance(decimal value, Currency currency)
    {
        _balance = Money.Create(value, currency).Value;
        return this;
    }

    public AccountBuilder WithLimit(decimal value, Currency currency)
    {
        _limit = Money.Create(value, currency).Value;
        return this;
    }

    public Account Build() => Account.Create(_title, _description, _type, _balance, _limit).Value;
}
