using Database.Entities;

namespace Database.Specification;

public static class SpecificationExtension
{
    public static IQueryable<TEntity> ApplySpecification<TEntity>(this IQueryable<TEntity> query,
        Specification<TEntity> spec) where TEntity : Entity => spec.ApplySpecification(query);
}