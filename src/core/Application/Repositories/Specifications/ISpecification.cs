using System.Linq.Expressions;

namespace Application.Repositories.Specifications;

public interface ISpecification<T>
{
    // Where Setup
    Expression<Func<T, bool>> Criteria { get; }


    // Order Setup
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; }


    // Pagination Setup
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }


    // Tracking Setup
    bool IsTrackingEnabled { get; }

}