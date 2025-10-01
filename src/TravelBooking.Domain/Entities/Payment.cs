using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBooking.Domain.Enums;

namespace TravelBooking.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string Method { get; set; } = default!;      // Card, VNPay, Momo...
        public DateTime PaidAt { get; set; }
        public string TransactionRef { get; set; } = default!;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Nav
        public Booking? Booking { get; set; }
    }
}