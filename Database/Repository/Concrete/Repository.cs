using Database.Context;
using Database.Entities;
using Database.Specification;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Concrete;

public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly DbSet<TEntity> _entities;
    
    public Repository(InstagramContext context)
    {
        _entities = context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Specification<TEntity> spec)
    {
        var query = _entities.AsQueryable();

        query = query.Where(spec);

        return await spec.ApplySpecification(query).ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedWhen = DateTime.Now;
        
        return (await _entities.AddAsync(entity)).Entity;
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}