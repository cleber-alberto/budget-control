
namespace BudgetControl.Domain.ValueObjects;

public sealed class Description(string Value) : ValueObject
{
    const int MaxLength = 1024;

    public string Value { get; } = Value;

    public static Result<Description> Create(string value)
    {
        var erros = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            erros.Add(Error.NullOrWhiteSpace(nameof(Description)));
        }

        if (value.Length > MaxLength)
        {
            erros.Add(Error.TooLong(nameof(Description), MaxLength));
        }

        return erros.Count != 0
            ? Result.Failures<Description>(erros)
            : Result.Success(new Description(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
