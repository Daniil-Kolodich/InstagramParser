using System.Linq.Expressions;
using Database.Entities;

namespace Database.Specification;

public class Specification<TEntity> where TEntity : Entity
{
    public Specification()
    {
    }
    
    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        _criteria = criteria;
    }

    public virtual IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query) => query;
    
    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification)
    {
        return specification._criteria;
    }

    private readonly Expression<Func<TEntity, bool>> _criteria = entity => true;
}
