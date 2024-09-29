using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Common.Primitives.Results;

namespace BudgetControl.Infrastructure.Persistence;

public class UnitOfWork(IDbContext DbContext) : IUnitOfWork
{
    public async Task<Result> CommitAsync(CancellationToken cancellationToken = default)
    {
        var result = await DbContext.SaveChangesAsync(cancellationToken);
        return result > 0
            ? Result.Success(true)
            : Result.Failures([Error.DatabaseError("Failed to commit changes")]);
    }
}
