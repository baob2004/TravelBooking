using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomTypeId { get; set; }
        public DateOnly Date { get; set; }
        public int Allotment { get; set; }   // quota bán theo ngày
        public int Sold { get; set; }        // đã bán/giữ
        public int Available { get; set; }   // có thể tính = Allotment - Sold

        // Nav
        public Hotel? Hotel { get; set; }
        public RoomType? RoomType { get; set; }
    }
}