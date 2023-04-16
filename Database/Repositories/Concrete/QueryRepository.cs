using Database.Context;
using Database.Entities;
using Database.Specification;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Concrete;

public sealed class QueryRepository<TEntity> : IQueryRepository<TEntity>
    where TEntity : Entity
{
    // TODO: decide what to do with ef core Tracking system
    private readonly DbSet<TEntity> _entities;

    public QueryRepository(InstagramContext context)
    {
        _entities = context.Set<TEntity>();
    }

    public Task<TEntity?> GetAsync(Specification<TEntity> spec)
    {
        return _entities.AsNoTracking().ApplySpecification(spec).Where(e => !e.IsDeleted).FirstOrDefaultAsync(spec);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> spec)
    {
        return await _entities.AsNoTracking().ApplySpecification(spec).Where(e => !e.IsDeleted).Where(spec).ToListAsync();
    }
}