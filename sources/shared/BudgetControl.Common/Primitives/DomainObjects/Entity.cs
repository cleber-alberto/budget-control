namespace BudgetControl.Common.Primitives.DomainObjects;

public abstract class Entity : ICloneable
{
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return Equals((Entity)obj);
    }

    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);

    public object Clone() => MemberwiseClone();

    public override int GetHashCode() => base.GetHashCode();
}
