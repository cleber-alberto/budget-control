namespace BudgetControl.Domain.Accounts;

public class AccountId(Guid value) : StronglyTypeId<Guid>(value);
