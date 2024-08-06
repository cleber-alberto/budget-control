using System.Collections.Generic;

namespace BudgetControl.Common.Primitives.Results;

public class Result
{
    public bool IsSuccess { get; }
    public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();
    public bool IsFailure => !IsSuccess;

    public Result(bool isSuccess, IEnumerable<Error> errors)
    {

        if ((isSuccess && errors.Any()) || (!isSuccess && !errors.Any()))
        {
            throw new ArgumentException("Inconsistent behaviour between isSuccess and errors", nameof(errors));
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(true, Enumerable.Empty<Error>(), value);

    private static Result Success() =>
        new(true, Enumerable.Empty<Error>());

    public static Result<TValue> Create<TValue>(TValue value, IEnumerable<Error> errors) where TValue : class =>
        value is null ? Failures<TValue>(errors) : Success(value);

    public static Result Failures(IEnumerable<Error> errors)
    {
        var errorList = errors.ToList();
        if (errorList.Count == 0)
        {
            throw new ArgumentException("Must have at least one error", nameof(errors));
        }

        return new Result(false, errors);
    }

    public static Result<TValue> Failures<TValue>(IEnumerable<Error> errors)
        => new (false, errors, default!);

    public static Result Combine(params Result[] results)
    {
        var errors = new List<Error>();
        if (results.Any(x => x.IsFailure))
        {
            errors.AddRange(results.Where(x => x.IsFailure).SelectMany(s => s.Errors).ToList());
        }

        return errors.Count != 0 ? Failures(errors) : Success();
    }
}
