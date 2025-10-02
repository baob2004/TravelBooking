using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelBooking.Application.Abstractions.Repositories;

namespace TravelBooking.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _db;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _db = context.Set<T>();
    }
    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await _db.AddAsync(entity, ct).AsTask();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        await _db.AddRangeAsync(entities, ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
    {
        return predicate is null ? await _db.AnyAsync(ct) : await _db.AnyAsync(predicate, ct);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
    {
        return predicate is null ? await _db.CountAsync(ct) : await _db.CountAsync(predicate, ct);
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool asNoTracking = true, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> q = _db;

        if (asNoTracking) q = q.AsNoTracking();

        if (includes is { Length: > 0 })
        {
            foreach (var inc in includes)
                q = q.Include(inc);
        }

        if (filter != null) q = q.Where(filter);
        if (orderBy != null) q = orderBy(q);

        return await q.ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(object id, bool asNoTracking = true, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _db;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
        {
            foreach (var inc in includes)
                query = query.Include(inc);
        }

        // Use EF Core's FindAsync only when no includes needed
        if (includes == null || includes.Length == 0)
            return await _db.FindAsync(new[] { id }, ct);

        // Otherwise query by key
        var keyName = _context.Model.FindEntityType(typeof(T))?
            .FindPrimaryKey()?.Properties.First().Name;

        if (keyName == null)
            throw new InvalidOperationException($"No key defined for {typeof(T).Name}");

        return await query.FirstOrDefaultAsync(
            e => EF.Property<object>(e, keyName) == id, ct);
    }

    public IQueryable<T> Query(bool asNoTracking = true)
        => asNoTracking ? _db.AsNoTracking() : _db;

    public void Remove(T entity)
    {
        _db.Remove(entity);
    }

    public void Update(T entity)
    {
        _db.Update(entity);
    }
}
