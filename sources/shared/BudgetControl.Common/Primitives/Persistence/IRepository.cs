using System.Linq.Expressions;

namespace BudgetControl.Common.Primitives.Persistence;

public interface IRepository<TEntity, TId>
    where TEntity : Entity
    where TId : StronglyTypeId<Guid>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
