using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TravelBooking.Application.DTOs.Amenity;
using TravelBooking.Application.DTOs.Booking;
using TravelBooking.Application.DTOs.Hotels;
using TravelBooking.Application.DTOs.RatePlans;
using TravelBooking.Application.DTOs.Ratings;
using TravelBooking.Application.DTOs.RoomTypes;
using TravelBooking.Domain.Entities;
using TravelBooking.Domain.ValueObjects;

namespace TravelBooking.Application.Mapping
{
    public class TravelBookingProfile : Profile
    {
        public TravelBookingProfile()
        {
            // ===== Hotel =====
            CreateMap<Hotel, HotelSummaryDto>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address != null ? s.Address.City : null));

            CreateMap<Hotel, HotelDetailDto>()
                .ForMember(d => d.AddressLine1, o => o.MapFrom(s => s.Address != null ? s.Address.Line1 : null))
                .ForMember(d => d.AddressLine2, o => o.MapFrom(s => s.Address != null ? s.Address.Line2 : null))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address != null ? s.Address.City : null))
                .ForMember(d => d.State, o => o.MapFrom(s => s.Address != null ? s.Address.State : null))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.Address != null ? s.Address.PostalCode : null))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Address != null ? s.Address.Country : null))
                .ForMember(d => d.Images, o => o.MapFrom(s => s.Images ?? Array.Empty<HotelImage>()));

            // ➜ BỔ SUNG map từ entity ảnh sang DTO ảnh
            CreateMap<HotelImage, HotelImageDto>();

            // Ảnh: DTO tạo -> entity
            CreateMap<HotelImageCreateDto, HotelImage>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.HotelId, o => o.Ignore())
                .ForMember(d => d.SortOrder, o => o.Ignore())
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url.Trim()));

            // RoomType
            CreateMap<RoomType, RoomTypeDto>();

            // Create/Update Hotel (DTO -> Entity)
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
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            // ========== RoomType ==========
            CreateMap<CreateRoomTypeRequest, RoomType>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateRoomTypeRequest, RoomType>()
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            // ========== RatePlan ==========
            CreateMap<RatePlan, RatePlanDto>();
            CreateMap<RatePlan, RatePlanListItem>();

            CreateMap<CreateRatePlanRequest, RatePlan>()
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<UpdateRatePlanRequest, RatePlan>();

            // ========== Booking ==========
            CreateMap<BookingItem, BookingItemResponse>()
                .ForMember(d => d.RoomTypeName, o => o.Ignore())
                .ForMember(d => d.RatePlanName, o => o.Ignore());
            // ========== Rating ==========
            CreateMap<Rating, RatingDto>();
            // ========== Amenity ==========
            CreateMap<Amenity, AmenityDto>();
            CreateMap<CreateAmenityRequest, Amenity>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));

            CreateMap<UpdateAmenityRequest, Amenity>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));
        }
    }
}