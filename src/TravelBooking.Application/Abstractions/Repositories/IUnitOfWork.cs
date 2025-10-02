using TravelBooking.Domain.Entities;

namespace TravelBooking.Application.Abstractions.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<Hotel> Hotels { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}