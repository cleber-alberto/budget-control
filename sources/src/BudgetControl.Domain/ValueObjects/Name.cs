
namespace BudgetControl.Domain.ValueObjects;

public sealed class Name(string value) : ValueObject
{
    const string ValueObjectName = "name";
    const int MaxLength = 64;

    public string Value { get; } = value;

    public static Result<Name> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(Error.NullOrWhiteSpace(ValueObjectName));
        }

        if (value.Length > MaxLength)
        {
            errors.Add(Error.TooLong(ValueObjectName, MaxLength));
        }

        return errors.Count != 0
            ? Result.Failures<Name>(errors)
            : Result.Success(new Name(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
