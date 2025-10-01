using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;
using TravelBooking.Infrastructure.Identity;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> b)
        {
            b.HasKey(x => x.Id);

            // Identity mặc định key string nvarchar(450) trên SQL Server
            b.Property(x => x.UserId).HasMaxLength(450);

            // Quan hệ FK -> AppUser.Id nhưng KHÔNG cần navigation trong Customer
            b.HasOne<AppUser>()                // không chỉ ra property
             .WithMany()                       // không cần collection bên AppUser
             .HasForeignKey(x => x.UserId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}