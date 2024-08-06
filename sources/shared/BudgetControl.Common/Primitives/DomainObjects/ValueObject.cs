namespace BudgetControl.Common.Primitives.DomainObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetAtomicValues();

    public virtual bool Equals(ValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && ValuesAreEqual(other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals(obj as ValueObject);
    }

    public override int GetHashCode()
        => GetAtomicValues().Aggregate(default(int), HashCode.Combine);

    private bool ValuesAreEqual(ValueObject? other)
    {
        if (other is null)
            return false;

        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }
}
