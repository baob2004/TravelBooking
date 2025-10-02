using TravelBooking.Application.DTOs.Common;
using TravelBooking.Application.DTOs.Hotels;

namespace TravelBooking.Application.Abstractions.Services
{
    public interface IHotelService
    {
        Task<PagedResult<HotelSummaryDto>> GetPagedAsync(HotelFilter filter, PagedQuery page, CancellationToken ct);
        Task<HotelDetailDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Guid> CreateAsync(CreateHotelRequest dto, CancellationToken ct);
        Task UpdateAsync(Guid id, UpdateHotelRequest dto, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);


        Task SetAmenitiesAsync(Guid hotelId, IReadOnlyCollection<Guid> amenityIds, CancellationToken ct);
        Task AddImagesAsync(Guid hotelId, IReadOnlyCollection<HotelImageCreateDto> images, CancellationToken ct);
        Task SetCoverImageAsync(Guid hotelId, Guid imageId, CancellationToken ct);
        Task DeleteImageAsync(Guid hotelId, Guid imageId, CancellationToken ct);
    }
}