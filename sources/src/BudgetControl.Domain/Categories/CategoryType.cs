namespace BudgetControl.Domain.Categories;

public class CategoryType(int id, string name) : Enumeration(id, name)
{
    public static CategoryType Income = new(1, "Income");
    public static CategoryType Expense = new(2, "Expense");
}
