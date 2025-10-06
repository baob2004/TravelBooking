using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Domain.Entities;
using TravelBooking.Infrastructure.Identity;

namespace TravelBooking.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<RatePlan> RatePlans => Set<RatePlan>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingItem> BookingItems => Set<BookingItem>();
        public DbSet<HotelImage> HotelImages => Set<HotelImage>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<RoomTypeAmenity> RoomTypeAmenities => Set<RoomTypeAmenity>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            // === Role ===
            List<IdentityRole> roles =
            [
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" },
            ];
            b.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}