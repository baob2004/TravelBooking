using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TravelBooking.Application.DTOs.Booking;
using TravelBooking.Application.DTOs.Hotels;
using TravelBooking.Application.DTOs.RatePlans;
using TravelBooking.Application.DTOs.RoomTypes;
using TravelBooking.Domain.Entities;
using TravelBooking.Domain.ValueObjects;

namespace TravelBooking.Application.Mapping
{
    public class TravelBookingProfile : Profile
    {
        public TravelBookingProfile()
        {
            // ========== Hotel ==========
            CreateMap<Hotel, HotelSummaryDto>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<Hotel, HotelDetailDto>()
                .ForMember(d => d.AddressLine1, o => o.MapFrom(s => s.Address.Line1))
                .ForMember(d => d.AddressLine2, o => o.MapFrom(s => s.Address.Line2))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.State, o => o.MapFrom(s => s.Address.State))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.Address.PostalCode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Address.Country));

            CreateMap<HotelImage, HotelImageDto>();
            CreateMap<RoomType, RoomTypeDto>();

            CreateMap<CreateHotelRequest, Hotel>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForPath(d => d.Address.Line1, o => o.MapFrom(s => s.AddressLine1))
                .ForPath(d => d.Address.Line2, o => o.MapFrom(s => s.AddressLine2))
                .ForPath(d => d.Address.City, o => o.MapFrom(s => s.City))
                .ForPath(d => d.Address.State, o => o.MapFrom(s => s.State))
                .ForPath(d => d.Address.PostalCode, o => o.MapFrom(s => s.PostalCode))
                .ForPath(d => d.Address.Country, o => o.MapFrom(s => s.Country))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateHotelRequest, Hotel>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForPath(d => d.Address.Line1, o => o.MapFrom(s => s.AddressLine1))
                .ForPath(d => d.Address.Line2, o => o.MapFrom(s => s.AddressLine2))
                .ForPath(d => d.Address.City, o => o.MapFrom(s => s.City))
                .ForPath(d => d.Address.State, o => o.MapFrom(s => s.State))
                .ForPath(d => d.Address.PostalCode, o => o.MapFrom(s => s.PostalCode))
                .ForPath(d => d.Address.Country, o => o.MapFrom(s => s.Country))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            // ========== RoomType ==========
            CreateMap<CreateRoomTypeRequest, RoomType>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateRoomTypeRequest, RoomType>()
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            // ========== RatePlan ==========
            CreateMap<RatePlan, RatePlanListItem>();

            CreateMap<CreateRatePlanRequest, RatePlan>()
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<UpdateRatePlanRequest, RatePlan>();

            // ========== Booking ==========
            CreateMap<BookingItem, BookingItemResponse>()
                .ForMember(d => d.RoomTypeName, o => o.Ignore())
                .ForMember(d => d.RatePlanName, o => o.Ignore());

        }
    }
}