using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Ratings
{
    public class RatingDto
    {
        public Guid Id { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int Score { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}