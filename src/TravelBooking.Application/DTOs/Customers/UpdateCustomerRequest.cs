using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Customers
{
    public class UpdateCustomerRequest
    {

        [StringLength(200)]
        public string? FullName { get; set; }

        [Phone, StringLength(50)]
        public string? Phone { get; set; }
    }
}