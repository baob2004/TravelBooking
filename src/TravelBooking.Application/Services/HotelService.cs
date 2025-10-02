using AutoMapper;
using TravelBooking.Application.Abstractions.Repositories;
using TravelBooking.Application.Abstractions.Services;
using TravelBooking.Application.DTOs.Common;
using TravelBooking.Application.DTOs.Hotels;
using TravelBooking.Domain.Entities;
using TravelBooking.Domain.ValueObjects;

namespace TravelBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public HotelService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public Task AddImagesAsync(Guid hotelId, IReadOnlyCollection<HotelImageCreateDto> images, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateAsync(CreateHotelRequest dto, CancellationToken ct)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var name = dto.Name?.Trim() ?? string.Empty;
            var exists = await _uow.Hotels.AnyAsync(h => h.Name == name && h.Address.City == dto.City, ct);
            if (exists) throw new InvalidOperationException("Hotel with the same name already exists in this city.");

            var entity = _mapper.Map<Hotel>(dto);
            entity.Id = Guid.NewGuid();
            entity.AverageRating = 0m;
            entity.RatingCount = 0;
            entity.CreatedAt = DateTime.UtcNow;

            await _uow.Hotels.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid id.", nameof(id));

            var entity = await _uow.Hotels.GetByIdAsync(id, asNoTracking: false, ct: ct);
            if (entity is null) return;

            if (!entity.IsDeleted)
            {
                entity.IsDeleted = true;
                _uow.Hotels.Update(entity);
                await _uow.SaveChangesAsync(ct);
            }
        }

        public Task DeleteImageAsync(Guid hotelId, Guid imageId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<HotelDetailDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _uow.Hotels.GetByIdAsync(
                id,
                asNoTracking: true,
                ct: ct,
                includes: [
                    h => h.Images,
            h => h.RoomTypes
                ]);

            if (entity is null) return null;

            var dto = _mapper.Map<HotelDetailDto>(entity);

            dto.Images = dto.Images?.OrderBy(i => i.SortOrder).ToList() ?? new();
            dto.RoomTypes = dto.RoomTypes?.OrderBy(rt => rt.Name).ToList() ?? new();

            return dto;
        }

        public async Task<PagedResult<HotelSummaryDto>> GetPagedAsync(HotelFilter filter, PagedQuery page, CancellationToken ct)
        {
            filter ??= new HotelFilter();
            page ??= new PagedQuery();

            var pageNumber = page.Page <= 0 ? 1 : page.Page;
            var pageSize = page.PageSize <= 0 ? 10 : page.PageSize;

            IQueryable<Hotel> q = _uow.Hotels.Query();

            // ---- filters ----
            if (!string.IsNullOrWhiteSpace(filter.City))
                q = q.Where(h => h.Address.City == filter.City);

            if (!string.IsNullOrWhiteSpace(filter.Country))
                q = q.Where(h => h.Address.Country == filter.Country);

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim();
                q = q.Where(h =>
                    h.Name.Contains(kw) ||
                    (h.Description != null && h.Description.Contains(kw)));
            }

            if (filter.MinStar.HasValue)
                q = q.Where(h => h.StarRating >= filter.MinStar.Value);

            if (filter.MaxStar.HasValue)
                q = q.Where(h => h.StarRating <= filter.MaxStar.Value);

            if (filter.MinAverageRating.HasValue)
                q = q.Where(h => h.AverageRating >= filter.MinAverageRating.Value);

            // ---- total count ----
            var total = q.Count();

            // ---- sort mặc định ----
            q = q.OrderByDescending(h => h.AverageRating)
                 .ThenByDescending(h => h.RatingCount)
                 .ThenBy(h => h.Name);

            // ---- page + projection ----
            var items = q
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new HotelSummaryDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    StarRating = h.StarRating,
                    AverageRating = h.AverageRating,
                    RatingCount = h.RatingCount,
                    City = h.Address.City
                })
                .ToList();

            // Không còn await thực sự; giữ signature async để tương thích
            return await Task.FromResult(new PagedResult<HotelSummaryDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total
            });
        }

        public Task SetAmenitiesAsync(Guid hotelId, IReadOnlyCollection<Guid> amenityIds, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task SetCoverImageAsync(Guid hotelId, Guid imageId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Guid id, UpdateHotelRequest dto, CancellationToken ct)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (id == Guid.Empty) throw new ArgumentException("Invalid id.", nameof(id));

            var entity = await _uow.Hotels.GetByIdAsync(id, asNoTracking: false, ct: ct)
                         ?? throw new KeyNotFoundException("Hotel not found.");

            // (Tuỳ chọn) Kiểm tra trùng tên + city (trừ chính nó)
            var normalizedName = (dto.Name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedName))
                throw new ArgumentException("Hotel name is required.", nameof(dto.Name));

            var duplicate = await _uow.Hotels.AnyAsync(
                h => h.Id != id && h.Name == normalizedName && h.Address.City == dto.City, ct);
            if (duplicate)
                throw new InvalidOperationException("Another hotel with the same name already exists in this city.");

            _mapper.Map(dto, entity);

            _uow.Hotels.Update(entity);
            await _uow.SaveChangesAsync(ct);
        }
    }
}