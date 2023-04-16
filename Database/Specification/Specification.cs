using System.Linq.Expressions;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Specification;

public class Specification<TEntity> where TEntity : Entity
{
    private readonly List<Expression<Func<TEntity, IEnumerable<Entity>>>> _collectionIncludes = new();

    private readonly Expression<Func<TEntity, bool>> _criteria = entity => true;
    private readonly List<Expression<Func<TEntity, Entity>>> _navigationIncludes = new();

    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        _criteria = criteria;
    }

    internal IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query)
    {
        if (_navigationIncludes.Any())
            query = _navigationIncludes.Aggregate(query, (current, include) => current.Include(include));

        if (_collectionIncludes.Any())
            query = _collectionIncludes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification)
    {
        return specification._criteria;
    }

    protected void Include(Expression<Func<TEntity, Entity>> navigationProperty)
    {
        _navigationIncludes.Add(navigationProperty);
    }

    protected void Include(Expression<Func<TEntity, IEnumerable<Entity>>> collectionProperty)
    {
        _collectionIncludes.Add(collectionProperty);
    }
}