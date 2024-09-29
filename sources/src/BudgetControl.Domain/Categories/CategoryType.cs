namespace BudgetControl.Domain.Categories;

public class CategoryType(string id, string name) : Enumeration<string>(id, name)
{
    public static CategoryType Credit = new("Credit", "Credit");
    public static CategoryType Debit = new("Debit", "Debit");
}
