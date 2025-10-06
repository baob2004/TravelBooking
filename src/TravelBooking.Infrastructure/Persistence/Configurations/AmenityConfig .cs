using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public class AmenityConfig : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.HasIndex(x => x.Name).IsUnique(); // nếu muốn unique theo tên
        }
    }
}