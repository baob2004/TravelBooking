using TravelBooking.Application.Abstractions.Repositories;

namespace TravelBooking.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _context;
        public UnitOfWork
        (
            AppDbContext context
        )
        {
            _context = context;
        }
        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}