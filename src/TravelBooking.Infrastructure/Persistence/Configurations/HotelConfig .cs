using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public class HotelConfig : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> b)
        {
            b.HasKey(x => x.Id);

            // Address là owned type bên trong Hotel
            b.OwnsOne(x => x.Address, a =>
            {
                a.Property(p => p.Line1).HasMaxLength(200).HasColumnName("Address_Line1");
                a.Property(p => p.Line2).HasMaxLength(200).HasColumnName("Address_Line2");
                a.Property(p => p.City).HasMaxLength(100).HasColumnName("Address_City");
                a.Property(p => p.State).HasMaxLength(100).HasColumnName("Address_State");
                a.Property(p => p.PostalCode).HasMaxLength(20).HasColumnName("Address_PostalCode");
                a.Property(p => p.Country).HasMaxLength(100).HasColumnName("Address_Country");
            });
        }
    }
}