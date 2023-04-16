using Database.Entities;

namespace Database.Repository;

public interface ICommandRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    void UpdateAsync(TEntity entity);
    void DeleteAsync(TEntity entity);
}