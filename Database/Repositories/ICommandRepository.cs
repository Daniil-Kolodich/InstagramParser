using Database.Entities;

namespace Database.Repositories;

public interface ICommandRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
}