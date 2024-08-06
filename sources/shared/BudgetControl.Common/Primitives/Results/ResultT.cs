namespace BudgetControl.Common.Primitives.Results;

public class Result<TValue>(bool isSuccess, IEnumerable<Error> errors, TValue value)
    : Result(isSuccess, errors)
{
    public TValue Value { get; } = value;

    public static Result<TValue> Success(TValue value)
        => new(true, Enumerable.Empty<Error>(), value);

    public new static Result<TValue> Failures(IEnumerable<Error> errors)
        => new(false, errors, default!);
}
