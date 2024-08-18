using System.Linq.Expressions;
using BudgetControl.Common.Primitives.DomainObjects;
using BudgetControl.Common.Primitives.Persistence;

namespace BudgetControl.Infrastructure.Persistence.Repositories;

public class Repository<TEntity, TId>(IDbContext dbContext) : IRepository<TEntity, TId>
    where TEntity : Entity
    where TId : StronglyTypeId<Guid>
{
    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken) =>
         dbContext.Set<TEntity>().FindAsync([id], cancellationToken).AsTask();

    public Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
        dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity) =>
        dbContext.Set<TEntity>().Remove(entity);

    public void Update(TEntity entity) =>
        dbContext.Set<TEntity>().Update(entity);
}
