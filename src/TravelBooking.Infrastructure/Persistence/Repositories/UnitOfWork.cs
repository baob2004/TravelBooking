using TravelBooking.Application.Abstractions.Repositories;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _context;
        private readonly IGenericRepository<Hotel> _hotelRepo;
        public UnitOfWork
        (
            AppDbContext context,
            IGenericRepository<Hotel> hotelRepo
        )
        {
            _context = context;
            _hotelRepo = hotelRepo;
        }

        public IGenericRepository<Hotel> Hotels => _hotelRepo;

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