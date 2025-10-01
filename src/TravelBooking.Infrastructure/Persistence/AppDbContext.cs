using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Domain.Entities;
using TravelBooking.Infrastructure.Identity;
using TravelBooking.Infrastructure.Persistence.Configurations;

namespace TravelBooking.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<AppUser>(options) // <- kế thừa IdentityDbContext
    {
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<RatePlan> RatePlans => Set<RatePlan>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingItem> BookingItems => Set<BookingItem>();
        public DbSet<HotelImage> HotelImages => Set<HotelImage>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b); // rất quan trọng cho Identity
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            // ví dụ: ValueObject Address (nếu bạn dùng Address VO thay vì string)
            // b.Owned<Hotel>(h => h.Address); // nếu bạn dùng owned type, cấu hình chi tiết ở đây
        }
    }
}