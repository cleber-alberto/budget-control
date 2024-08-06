namespace BudgetControl.Common.Primitives.DomainObjects;

public class EntityId : StronglyTypeId<Guid>
{
    public EntityId(Guid value) : base(value)
    {

    }

}
