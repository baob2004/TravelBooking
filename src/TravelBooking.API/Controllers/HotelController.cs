using Microsoft.AspNetCore.Mvc;
using TravelBooking.Application.Abstractions.Services;
using TravelBooking.Application.DTOs.Common;             // PagedQuery, PagedResult<T>
using TravelBooking.Application.DTOs.Hotels;
using TravelBooking.Application.DTOs.RatePlans;
using TravelBooking.Application.DTOs.RoomTypes;             // HotelFilter, HotelSummaryDto

namespace TravelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController(IHotelService service) : ControllerBase
    {
        #region Hotels
        [HttpGet]
        public Task<PagedResult<HotelSummaryDto>> GetPagedAsync(
            [FromQuery] HotelFilter filter,
            [FromQuery] PagedQuery page,
            CancellationToken ct)
            => service.GetPagedAsync(filter, page, ct);

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var res = await service.GetByIdAsync(id, ct);
            return res == null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHotelRequest dto, CancellationToken ct)
        {
            var id = await service.CreateAsync(dto, ct);
            var created = await service.GetByIdAsync(id, ct);

            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateHotelRequest dto, CancellationToken ct)
        {
            await service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            await service.DeleteAsync(id, ct);
            return NoContent();
        }
        #endregion
        #region Images
        [HttpPost("{id:guid}/image")]
        public async Task<IActionResult> AddImage([FromRoute] Guid id, [FromBody] List<HotelImageCreateDto> images, CancellationToken ct)
        {
            await service.AddImagesAsync(id, images, ct);
            var hotel = await service.GetByIdAsync(id, ct);
            return hotel is null
                ? NotFound()
                : Ok(hotel.Images);
        }

        [HttpDelete("{id:guid}/image/{imageId:guid}")]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid id, [FromRoute] Guid imageId, CancellationToken ct)
        {
            await service.DeleteImageAsync(id, imageId, ct);
            return NoContent();
        }
        #endregion
        #region RoomType
        [HttpPost("{hotelId:guid}/room-types")]
        public async Task<IActionResult> CreateRoomType(
            Guid hotelId,
            [FromBody] CreateRoomTypeRequest dto,
            CancellationToken ct)
        {
            var id = await service.CreateRoomTypeAsync(hotelId, dto, ct);
            return StatusCode(StatusCodes.Status201Created, new { id });
        }

        [HttpPut("room-types/{roomTypeId:guid}")]
        public async Task<IActionResult> UpdateRoomType(
            Guid roomTypeId,
            [FromBody] UpdateRoomTypeRequest dto,
            CancellationToken ct)
        {
            await service.UpdateRoomTypeAsync(roomTypeId, dto, ct);
            return NoContent();
        }

        [HttpDelete("room-types/{roomTypeId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoomType(Guid roomTypeId, CancellationToken ct)
        {
            await service.DeleteRoomTypeAsync(roomTypeId, ct);
            return NoContent();
        }

        #endregion
        #region RatePlants
        [HttpPost("{hotelId:guid}/room-types/{roomTypeId:guid}/rate-plans")]
        public async Task<IActionResult> CreateRatePlan(
            Guid hotelId,
            Guid roomTypeId,
            [FromBody] CreateRatePlanRequest dto,
            CancellationToken ct)
        {
            dto.HotelId = hotelId;
            dto.RoomTypeId = roomTypeId;

            var id = await service.CreateRatePlanAsync(dto, ct);
            return StatusCode(StatusCodes.Status201Created, new { id });
        }

        [HttpPut("rate-plans/{ratePlanId:guid}")]
        public async Task<IActionResult> UpdateRatePlan(
            Guid ratePlanId,
            [FromBody] UpdateRatePlanRequest dto,
            CancellationToken ct)
        {
            await service.UpdateRatePlanAsync(ratePlanId, dto, ct);
            return NoContent();
        }

        [HttpDelete("rate-plans/{ratePlanId:guid}")]
        public async Task<IActionResult> DeleteRatePlan(Guid ratePlanId, CancellationToken ct)
        {
            await service.DeleteRatePlanAsync(ratePlanId, ct);
            return NoContent();
        }
        #endregion
    }
}
