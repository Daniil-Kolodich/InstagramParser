using System.Linq.Expressions;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Specification;

public abstract class Specification<TEntity> where TEntity : Entity
{
    private readonly Expression<Func<TEntity, bool>> _criteria;
    private readonly List<Expression<Func<TEntity, object>>> _includes = new();
    private readonly Expression<Func<TEntity, object>>? _orderBy;
    private readonly bool _isDescending;

    protected Specification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>>? orderBy = null, bool isDescending = false)
    {
        _criteria = criteria;
        _orderBy = orderBy;
        _isDescending = isDescending;
    }

    internal IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query)
    {
        if (_orderBy is not null)
            query = _isDescending ? query.OrderBy(_orderBy) : query.OrderByDescending(_orderBy);

        if (!_includes.Any())
            return query;
                    
        return _includes.Aggregate(query, (current, include) => current.Include(include));
    }

    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification)
    {
        return specification._criteria;
    }

    protected void Include(Expression<Func<TEntity, object>> expression)
    {
        _includes.Add(expression);
    }
}