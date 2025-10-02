using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TravelBooking.Application.Abstractions.Services;
using TravelBooking.Application.Mapping;
using TravelBooking.Application.Services;

namespace TravelBooking.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(TravelBookingProfile).Assembly);

            // Service
            services.AddScoped<IHotelService, HotelService>();

            return services;
        }
    }
}