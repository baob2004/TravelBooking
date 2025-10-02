using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Customers
{
    public class CreateCustomerRequest
    {
        public string? UserId { get; set; }

        [Required, StringLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(320)]
        public string Email { get; set; } = string.Empty;

        [Phone, StringLength(50)]
        public string Phone { get; set; } = string.Empty;
    }
}