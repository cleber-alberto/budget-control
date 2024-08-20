namespace BudgetControl.Domain.Categories;

public class CategoryType(int id, string name) : Enumeration(id, name)
{
    public static CategoryType Credit = new(1, "Credit");
    public static CategoryType Debit = new(2, "Debit");
}
