using System.Linq.Expressions;

namespace TravelBooking.Application.Abstractions.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query();

    Task<T?> GetByIdAsync(
        object id,
        bool asNoTracking = true,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes
    );

    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes
    );

    Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
}
