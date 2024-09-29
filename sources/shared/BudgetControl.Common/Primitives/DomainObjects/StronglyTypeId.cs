namespace BudgetControl.Common.Primitives.DomainObjects;

public abstract class StronglyTypeId<TValue>(TValue value) : IComparable<StronglyTypeId<TValue>>
    where TValue : notnull
{
    public TValue Value => value;

    public int CompareTo(StronglyTypeId<TValue>? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Comparer<TValue>.Default.Compare(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is StronglyTypeId<TValue> other)
        {
            return Value.Equals(other.Value);
        }

        return false;
    }

    public static bool operator ==(StronglyTypeId<TValue> left, StronglyTypeId<TValue> right) => left.Equals(right);

    public static bool operator !=(StronglyTypeId<TValue> left, StronglyTypeId<TValue> right) => !left.Equals(right);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString()!;
}
