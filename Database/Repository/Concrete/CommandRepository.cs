using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Concrete;

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

    public void UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
    }

    public void DeleteAsync(TEntity entity)
    {
        // TODO: add support of soft delete as well
        _entities.Remove(entity);
    }
}