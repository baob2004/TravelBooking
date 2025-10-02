using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Inventory
{
    public class UpsertInventoryRequest
    {
        [Required] public Guid HotelId { get; set; }
        [Required] public Guid RoomTypeId { get; set; }
        [Required] public DateOnly Date { get; set; }
        [Range(0, 10000)] public int Allotment { get; set; }
    }
}