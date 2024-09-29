namespace BudgetControl.Common.Primitives.Persistence;

public interface IUnitOfWork
{
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}
