
namespace BudgetControl.Domain.Enumerations;

public class Currency(string code, string symbol) : Enumeration<string>(code, symbol)
{
    public string Code { get; } = "None";
    public string Symbol { get; } = "N/A";

    public static Currency None = new("None", "N/A");
    public static Currency USD = new("USD", "$");
    public static Currency CAD = new("CAD", "$");
    public static Currency BRL = new("BRL", "R$");
}
