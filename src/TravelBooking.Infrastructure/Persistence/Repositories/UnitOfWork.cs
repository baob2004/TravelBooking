using TravelBooking.Application.Abstractions.Repositories;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _context;
        private readonly IGenericRepository<Hotel> _hotelRepo;
        private readonly IGenericRepository<HotelImage> _hotelImageRepo;
        private readonly IGenericRepository<RoomType> _roomTypeRepo;
        private readonly IGenericRepository<RatePlan> _ratePlantRepo;
        private readonly IGenericRepository<BookingItem> _bookingItemRepo;
        private readonly IGenericRepository<Inventory> _inventoryRepo;
        private readonly IGenericRepository<Amenity> _amenityRepo;
        private readonly IGenericRepository<RoomTypeAmenity> _roomTypeAmenityRepo;

        public UnitOfWork
        (
            AppDbContext context,
            IGenericRepository<Hotel> hotelRepo,
            IGenericRepository<HotelImage> hotelImageRepo,
            IGenericRepository<RoomType> roomTypeRepo,
            IGenericRepository<RatePlan> ratePlantRepo,
            IGenericRepository<BookingItem> bookingItemRepo,
            IGenericRepository<Inventory> inventoryRepo,
            IGenericRepository<Amenity> amenityRepo,
            IGenericRepository<RoomTypeAmenity> roomTypeAmenityRepo
        )
        {
            _context = context;
            _hotelRepo = hotelRepo;
            _hotelImageRepo = hotelImageRepo;
            _roomTypeRepo = roomTypeRepo;
            _ratePlantRepo = ratePlantRepo;
            _bookingItemRepo = bookingItemRepo;
            _inventoryRepo = inventoryRepo;
            _amenityRepo = amenityRepo;
            _roomTypeAmenityRepo = roomTypeAmenityRepo;
        }

        public IGenericRepository<Hotel> Hotels => _hotelRepo;

        public IGenericRepository<HotelImage> HotelImages => _hotelImageRepo;

        public IGenericRepository<RoomType> RoomTypes => _roomTypeRepo;

        public IGenericRepository<RatePlan> RatePlans => _ratePlantRepo;

        public IGenericRepository<BookingItem> BookingItems => _bookingItemRepo;

        public IGenericRepository<Inventory> Inventories => _inventoryRepo;

        public IGenericRepository<Amenity> Amenities => _amenityRepo;

        public IGenericRepository<RoomTypeAmenity> RoomTypeAmenities => _roomTypeAmenityRepo;

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