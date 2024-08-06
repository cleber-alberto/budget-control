
namespace BudgetControl.Domain.ValueObjects;

public sealed class Description(string value) : ValueObject
{
    const string Name = "description";
    const int MaxLength = 1024;

    public static Result<Description> Create(string value)
    {
        var erros = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            erros.Add(Error.NullOrWhiteSpace(Name));
        }

        if (value.Length > MaxLength)
        {
            erros.Add(Error.TooLong(Name, MaxLength));
        }

        return erros.Count != 0
            ? Result.Failures<Description>(erros)
            : Result.Success(new Description(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return value;
    }
}
