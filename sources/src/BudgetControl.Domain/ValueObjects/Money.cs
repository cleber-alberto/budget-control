using BudgetControl.Domain.Enumerations;

namespace BudgetControl.Domain.ValueObjects;

public class Money(decimal value, Currency currency) : ValueObject
{
    public decimal Value { get; } = value;
    public Currency Currency { get; } = currency;

    public static Result<Money> Create(decimal value, Currency currency)
    {
        var errors = new List<Error>();

        if (value <= 0)
        {
            errors.Add(Error.MoreThan(nameof(value), 0));
        }

        return errors.Count != 0
            ? Result.Failures<Money>(errors)
            : Result.Success(new Money(value, currency));
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Cannot add money with different currencies");
        }

        return new Money(left.Value + right.Value, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Cannot subtract money with different currencies");
        }

        return new Money(left.Value - right.Value, left.Currency);
    }

    public static Money operator *(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Cannot multiply money with different currencies");
        }

        return new Money(left.Value * right.Value, left.Currency);
    }

    public static Money operator /(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Cannot divide money with different currencies");
        }

        if (right.Value == 0)
        {
            throw new InvalidOperationException("Cannot divide by zero");
        }

        return new Money(left.Value / right.Value, left.Currency);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public bool MoreThan(Money limit) => Value > limit.Value;
}
