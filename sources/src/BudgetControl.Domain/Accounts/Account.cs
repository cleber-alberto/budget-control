
namespace BudgetControl.Domain.Accounts;

public class Account : Entity, IAggregateRoot
{
    public AccountId Id { get; protected set; } = null!;
    public Title Title { get; protected set; } = null!;
    public Description Description { get; protected set; } = null!;
    public AccountType Type { get; protected set; } = null!;
    public Money Balance { get; protected set; } = null!;
    public Money Limit { get; protected set; } = null!;

    public static Result<Account> Create(string title, string description, AccountType type, Money balance, Money limit)
    {
        var account = new Account();
        var result = account.Update(new AccountId(Guid.NewGuid()), title, description, type, balance, limit);

        if (result.IsFailure)
        {
            return Result.Failures<Account>(result.Errors);
        }

        return Result<Account>.Success(account);
    }

    public Result<Account> Update(AccountId accountId, string title, string description, AccountType type,  Money balance, Money limit)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);
        var balanceResult = Money.Create(balance.Value, balance.Currency);
        var limitResult = Money.Create(limit.Value, limit.Currency);

        var typeResult = type == AccountType.None
            ? Result.Failures<AccountType>([Error.Required(nameof(type))])
            : Result.Success(type);

        var result = Result.Combine(titleResult, descriptionResult, balanceResult, limitResult,  typeResult);

        if (result.IsFailure)
        {
            return Result<Account>.Failures(result.Errors);
        }

        Id = accountId;
        Title = titleResult.Value;
        Description = descriptionResult.Value;
        Type = type;
        Balance = balance;
        Limit = limit;

        return Result<Account>.Success(this);
    }

    public Result Deposit(Money amount)
    {
        Balance += amount;
        return Result.Success(true);
    }

    public Result Withdraw(Money amount)
    {

        if ((Balance + amount).MoreThan(Limit))
        {
            return Result.Failures([Error.LimitExceeded]);
        }

        Balance -= amount;
        return Result.Success(true);
    }

    public Result Transfer(Account destination, Money amount)
    {
        var withdrawResult = Withdraw(amount);
        if (withdrawResult.IsFailure)
        {
            return Result.Failures(withdrawResult.Errors);
        }

        destination.Deposit(amount);

        return Result.Success(true);
    }

    public override string ToString() => $"{GetType().Name}({Id})";
}
