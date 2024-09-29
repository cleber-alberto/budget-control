namespace BudgetControl.Domain.Accounts;

public class AccountType(string id, string name) : Enumeration<string>(id, name)
{
    public static AccountType None = new("N/A", "None");
    public static AccountType Checking = new("Checking", "Checking");
    public static AccountType Savings = new("Savings", "Savings");
    public static AccountType CreditCard = new("CreditCard", "CreditCard");
}
