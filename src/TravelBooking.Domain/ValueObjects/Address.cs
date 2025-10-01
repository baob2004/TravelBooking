using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.ValueObjects
{
    public class Address
    {
        public string Line1 { get; private set; } = default!;
        public string? Line2 { get; private set; }
        public string City { get; private set; } = default!;
        public string State { get; private set; } = default!;
        public string PostalCode { get; private set; } = default!;
        public string Country { get; private set; } = default!;

        private Address() { } // EF Core
        public Address(string line1, string? line2, string city, string state, string postalCode, string country)
        {
            Line1 = line1; Line2 = line2; City = city; State = state; PostalCode = postalCode; Country = country;
        }
    }
}