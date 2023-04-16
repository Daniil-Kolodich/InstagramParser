using Database.Entities;
using Database.Specification;

namespace Database.Repository;

public interface IQueryRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetAsync(Specification<TEntity> spec);
    Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> spec);
}