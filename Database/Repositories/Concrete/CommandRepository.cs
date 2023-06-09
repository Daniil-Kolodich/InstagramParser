using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Concrete;
public sealed class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : Entity
{
    private readonly DbSet<TEntity> _entities;

    public CommandRepository(InstagramContext context)
    {
        _entities = context.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedWhen = DateTime.Now;

        return (await _entities.AddAsync(entity)).Entity;
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _entities.UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
    }
}