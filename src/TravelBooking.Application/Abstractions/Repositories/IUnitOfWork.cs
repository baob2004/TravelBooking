using TravelBooking.Domain.Entities;

namespace TravelBooking.Application.Abstractions.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<Hotel> Hotels { get; }
        IGenericRepository<HotelImage> HotelImages { get; }
        IGenericRepository<RoomType> RoomTypes { get; }
        IGenericRepository<RatePlan> RatePlans { get; }
        IGenericRepository<BookingItem> BookingItems { get; }
        IGenericRepository<Inventory> Inventories { get; }
        IGenericRepository<Amenity> Amenities { get; }
        IGenericRepository<RoomTypeAmenity> RoomTypeAmenities { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}