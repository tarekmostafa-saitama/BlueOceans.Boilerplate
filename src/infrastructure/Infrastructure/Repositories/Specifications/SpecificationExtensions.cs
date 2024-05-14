using Application.Repositories.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Specifications;

public static class SpecificationExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class
    {

        query = spec.OrderBy != null
            ? spec.OrderBy(query)
            : query;

        query = spec.IsPagingEnabled
            ? query.Skip(spec.Skip).Take(spec.Take)
            : query;

        query = spec.IsTrackingEnabled ? query : query.AsNoTracking();

        query = spec.Criteria == null ? query : query.Where(spec.Criteria);
        return query;
    }
}