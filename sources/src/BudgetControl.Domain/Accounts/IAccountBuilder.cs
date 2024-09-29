namespace BudgetControl.Domain.Accounts;

public interface IAccountBuilder<TAccount> where TAccount : Account
{
    IAccountBuilder<TAccount> WithTitle(string title);
    IAccountBuilder<TAccount> WithDescription(string description);
    IAccountBuilder<TAccount> WithBalance(Money balance);
    Result<TAccount> Build();
}
