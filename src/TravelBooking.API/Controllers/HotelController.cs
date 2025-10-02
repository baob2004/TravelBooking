using Microsoft.AspNetCore.Mvc;
using TravelBooking.Application.Abstractions.Services;
using TravelBooking.Application.DTOs.Common;             // PagedQuery, PagedResult<T>
using TravelBooking.Application.DTOs.Hotels;             // HotelFilter, HotelSummaryDto

namespace TravelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController(IHotelService service) : ControllerBase
    {
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
    }
}
