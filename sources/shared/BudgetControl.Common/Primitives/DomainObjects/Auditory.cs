
namespace BudgetControl.Common.Primitives.DomainObjects;

public class Auditory
{
    public DateTimeOffset Created { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastUpdated { get; protected set; }

    public void SetLastUpdate() => this.LastUpdated = DateTimeOffset.UtcNow;
}
