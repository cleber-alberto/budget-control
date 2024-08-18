
namespace BudgetControl.Domain.ValueObjects;

public sealed class Title(string Value) : ValueObject
{
    const int MaxLength = 64;

    public string Value { get; } = Value;

    public static Result<Title> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(Error.NullOrWhiteSpace(nameof(Title)));
        }

        if (value.Length > MaxLength)
        {
            errors.Add(Error.TooLong(nameof(Title), MaxLength));
        }

        return errors.Count != 0
            ? Result.Failures<Title>(errors)
            : Result.Success(new Title(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
