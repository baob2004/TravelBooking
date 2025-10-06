using TravelBooking.Application.DTOs.Common;
using TravelBooking.Application.DTOs.Hotels;
using TravelBooking.Application.DTOs.RatePlans;
using TravelBooking.Application.DTOs.RoomTypes;

namespace TravelBooking.Application.Abstractions.Services
{
    public interface IHotelService
    {
        // ===== Hotel =====
        Task<PagedResult<HotelSummaryDto>> GetPagedAsync(HotelFilter filter, PagedQuery page, CancellationToken ct);
        Task<HotelDetailDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Guid> CreateAsync(CreateHotelRequest dto, CancellationToken ct);
        Task UpdateAsync(Guid id, UpdateHotelRequest dto, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);

        // ===== Hotel Images =====
        Task AddImagesAsync(Guid hotelId, IReadOnlyCollection<HotelImageCreateDto> images, CancellationToken ct);
        Task SetCoverImageAsync(Guid hotelId, Guid imageId, CancellationToken ct);
        Task DeleteImageAsync(Guid hotelId, Guid imageId, CancellationToken ct);

        // ===== Room Types (thuần CRUD / upsert nhẹ) =====
        Task<Guid> CreateRoomTypeAsync(Guid hotelId, CreateRoomTypeRequest dto, CancellationToken ct);
        Task UpdateRoomTypeAsync(Guid roomTypeId, UpdateRoomTypeRequest dto, CancellationToken ct);
        Task DeleteRoomTypeAsync(Guid roomTypeId, CancellationToken ct);

        // ===== Rate Plans =====
        Task<Guid> CreateRatePlanAsync(CreateRatePlanRequest dto, CancellationToken ct);
        Task UpdateRatePlanAsync(Guid ratePlanId, UpdateRatePlanRequest dto, CancellationToken ct);
        Task DeleteRatePlanAsync(Guid ratePlanId, CancellationToken ct);
    }
}