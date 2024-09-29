using BudgetControl.Common.Primitives.Persistence;

namespace BudgetControl.Infrastructure.Persistence;

public class UnitOfWork(IDbContext DbContext) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
        DbContext.SaveChangesAsync(cancellationToken);
}
