using AutoMapper;
using TravelBooking.Application.Abstractions.Repositories;
using TravelBooking.Application.Abstractions.Services;
using TravelBooking.Application.DTOs.Common;
using TravelBooking.Application.DTOs.Hotels;
using TravelBooking.Application.DTOs.RatePlans;
using TravelBooking.Application.DTOs.RoomTypes;
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

        #region Hotels
        public async Task<HotelDetailDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _uow.Hotels.GetByIdAsync(
                id,
                asNoTracking: true,
                ct: ct,
                includes: [
                    h => h.Images,
                    h => h.RoomTypes,
                    h => h.RatePlans,
                    h => h.Ratings
                ]);

            if (entity is null) return null;

            var dto = _mapper.Map<HotelDetailDto>(entity);

            dto.Images = dto.Images?.OrderBy(i => i.SortOrder).ToList() ?? new();
            dto.RoomTypes = dto.RoomTypes?.OrderBy(rt => rt.Name).ToList() ?? new();
            dto.RatePlans = dto.RatePlans.OrderBy(rp => rp.Name).ToList();
            dto.Ratings = dto.Ratings.OrderByDescending(r => r.CreatedAt).ToList();

            return dto;
        }

        public async Task<PagedResult<HotelSummaryDto>> GetPagedAsync(
            HotelFilter filter, PagedQuery page, CancellationToken ct)
        {
            filter ??= new HotelFilter();
            page ??= new PagedQuery();

            var pageNumber = page.Page <= 0 ? 1 : page.Page;
            var pageSize = page.PageSize <= 0 ? 10 : page.PageSize;

            IQueryable<Hotel> q = _uow.Hotels.Query()
                                             .Where(h => !h.IsDeleted);

            // ---- filters ----
            if (!string.IsNullOrWhiteSpace(filter.City))
                q = q.Where(h => h.Address.City == filter.City);

            if (!string.IsNullOrWhiteSpace(filter.Country))
            {
                q = q.Where(h => h.Address.Country == filter.Country);
            }

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

            // ---- page + projection cơ bản (chưa có CoverImageUrl) ----
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
                    City = h.Address.City,
                    CoverImageUrl = null // sẽ gán ở bước 2
                })
                .ToList();

            if (items.Count > 0)
            {
                var hotelIds = items.Select(i => i.Id).ToList();

                // Lấy toàn bộ ảnh của các khách sạn trong page (1 query)
                var images = await _uow.HotelImages.GetAllAsync(
                    filter: x => hotelIds.Contains(x.HotelId),
                    orderBy: qimg => qimg.OrderBy(i => i.SortOrder),
                    asNoTracking: true,
                    ct: ct
                );

                // Chọn cover nếu có; nếu không, lấy ảnh đầu tiên theo SortOrder
                var coverByHotel = images
                    .GroupBy(i => i.HotelId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.FirstOrDefault(i => i.IsCover)?.Url ?? g.First().Url
                    );

                // Gán vào DTO
                foreach (var dto in items)
                    if (coverByHotel.TryGetValue(dto.Id, out var url))
                        dto.CoverImageUrl = url;
            }

            return new PagedResult<HotelSummaryDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total
            };
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
        #endregion
        #region Images
        public async Task AddImagesAsync(Guid hotelId, IReadOnlyCollection<HotelImageCreateDto> images, CancellationToken ct)
        {
            if (hotelId == Guid.Empty) throw new ArgumentException("Invalid hotelId.", nameof(hotelId));
            if (images is null || images.Count == 0) return;

            // 1) Tồn tại & chưa soft-delete
            var hotel = await _uow.Hotels.GetByIdAsync(hotelId, asNoTracking: false, ct: ct)
                        ?? throw new KeyNotFoundException("Hotel not found.");
            if (hotel.IsDeleted) throw new InvalidOperationException("Hotel was deleted.");

            // 2) Lọc DTO trống & map sang entity
            var validDtos = images
                .Where(i => !string.IsNullOrWhiteSpace(i.Url))
                .ToList();
            if (validDtos.Count == 0) return;

            var newEntities = _mapper.Map<List<HotelImage>>(validDtos);

            // 3) Lấy ảnh hiện có để tính SortOrder & cover
            var existing = await _uow.HotelImages.GetAllAsync(
                filter: x => x.HotelId == hotelId,
                orderBy: q => q.OrderBy(x => x.SortOrder),
                asNoTracking: false,
                ct: ct);

            var nextSort = existing.Count > 0 ? existing.Max(x => x.SortOrder) + 1 : 1;

            // Bảo đảm chỉ 1 cover sau khi thêm
            var anyNewCover = validDtos.Any(d => d.IsCover);
            if (anyNewCover)
            {
                foreach (var e in existing.Where(x => x.IsCover))
                {
                    e.IsCover = false;
                    _uow.HotelImages.Update(e);
                }
            }
            else if (!existing.Any(x => x.IsCover) && newEntities.Count > 0)
            {
                // Nếu chưa có cover cũ & batch mới không set cover → đặt tấm đầu làm cover
                newEntities[0].IsCover = true;
            }

            // 4) Gán HotelId / Id / SortOrder, xử lý duy nhất 1 cover trong batch
            var usedCover = false;
            foreach (var (entity, dto) in newEntities.Zip(validDtos))
            {
                entity.Id = Guid.NewGuid();
                entity.HotelId = hotelId;

                // sort
                entity.SortOrder = dto.SortOrder.HasValue && dto.SortOrder.Value > 0
                    ? dto.SortOrder.Value
                    : nextSort++;

                // cover
                if (dto.IsCover && !usedCover)
                {
                    entity.IsCover = true;
                    usedCover = true;
                }
                else if (dto.IsCover && usedCover)
                {
                    entity.IsCover = false; // chỉ để 1 cover trong batch
                }
                // nếu dto.IsCover == false → giữ false
            }

            // 5) Lưu
            await _uow.HotelImages.AddRangeAsync(newEntities, ct);
            await _uow.SaveChangesAsync(ct); if (hotelId == Guid.Empty) throw new ArgumentException("Invalid hotelId.", nameof(hotelId));
            if (images is null || images.Count == 0) return;
        }

        public async Task DeleteImageAsync(Guid hotelId, Guid imageId, CancellationToken ct)
        {
            if (hotelId == Guid.Empty) throw new ArgumentException("Invalid hotelId.", nameof(hotelId));
            if (imageId == Guid.Empty) throw new ArgumentException("Invalid imageId.", nameof(imageId));

            // 1) Hotel tồn tại & chưa bị xóa mềm
            var hotel = await _uow.Hotels.GetByIdAsync(hotelId, asNoTracking: false, ct: ct)
                        ?? throw new KeyNotFoundException("Hotel not found.");
            if (hotel.IsDeleted) throw new InvalidOperationException("Hotel was deleted.");

            // 2) Lấy ảnh cần xóa, đảm bảo thuộc về hotelId
            var image = await _uow.HotelImages.GetByIdAsync(imageId, asNoTracking: false, ct: ct)
                        ?? throw new KeyNotFoundException("Image not found.");

            if (image.HotelId != hotelId)
                throw new InvalidOperationException("Image does not belong to the specified hotel.");

            // 3) Lấy các ảnh còn lại (để xử lý cover & sort)
            var others = await _uow.HotelImages.GetAllAsync(
                filter: x => x.HotelId == hotelId && x.Id != imageId,
                orderBy: q => q.OrderBy(x => x.SortOrder),
                asNoTracking: false,
                ct: ct
            );

            // 4) Nếu ảnh bị xóa là cover => chuyển cover sang ảnh đầu tiên còn lại (nếu có)
            if (image.IsCover && others.Count > 0)
            {
                // bỏ cover ở ảnh cũ (không bắt buộc nhưng rõ ràng)
                image.IsCover = false;

                var newCover = others[0];
                if (!newCover.IsCover)
                {
                    newCover.IsCover = true;
                    _uow.HotelImages.Update(newCover);
                }
            }

            // 5) Xoá ảnh
            _uow.HotelImages.Remove(image);

            // 6) (Tuỳ chọn) Normalize SortOrder cho các ảnh còn lại: 1,2,3,...
            if (others.Count > 0)
            {
                for (int i = 0; i < others.Count; i++)
                {
                    var desired = i + 1;
                    if (others[i].SortOrder != desired)
                    {
                        others[i].SortOrder = desired;
                        _uow.HotelImages.Update(others[i]);
                    }
                }
            }

            await _uow.SaveChangesAsync(ct);
        }

        public async Task SetCoverImageAsync(Guid hotelId, Guid imageId, CancellationToken ct)
        {
            if (hotelId == Guid.Empty) throw new ArgumentException("Invalid hotelId.", nameof(hotelId));
            if (imageId == Guid.Empty) throw new ArgumentException("Invalid imageId.", nameof(imageId));

            // 1) Hotel tồn tại & chưa bị soft delete
            var hotel = await _uow.Hotels.GetByIdAsync(hotelId, asNoTracking: false, ct: ct)
                        ?? throw new KeyNotFoundException("Hotel not found.");
            if (hotel.IsDeleted) throw new InvalidOperationException("Hotel was deleted.");

            // 2) Ảnh cần đặt cover phải tồn tại và thuộc đúng hotel
            var img = await _uow.HotelImages.GetByIdAsync(imageId, asNoTracking: false, ct: ct)
                      ?? throw new KeyNotFoundException("Image not found.");
            if (img.HotelId != hotelId)
                throw new InvalidOperationException("Image does not belong to the specified hotel.");

            // Nếu đã là cover thì thôi
            if (img.IsCover) return;

            // 3) Bỏ cờ cover ở các ảnh khác cùng khách sạn (nếu có)
            var currentCovers = await _uow.HotelImages.GetAllAsync(
                filter: x => x.HotelId == hotelId && x.IsCover && x.Id != imageId,
                asNoTracking: false,
                ct: ct
            );
            foreach (var c in currentCovers)
            {
                c.IsCover = false;
                _uow.HotelImages.Update(c);
            }

            // 4) Đặt cover cho ảnh được chọn
            img.IsCover = true;
            _uow.HotelImages.Update(img);

            await _uow.SaveChangesAsync(ct);
        }
        #endregion
        #region RoomTypes
        public async Task<Guid> CreateRoomTypeAsync(Guid hotelId, CreateRoomTypeRequest dto, CancellationToken ct)
        {
            if (hotelId == Guid.Empty) throw new ArgumentException("Invalid hotelId.", nameof(hotelId));
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var hotel = await _uow.Hotels.GetByIdAsync(hotelId, asNoTracking: true, ct: ct)
                        ?? throw new KeyNotFoundException("Hotel not found.");
            if (hotel.IsDeleted) throw new InvalidOperationException("Hotel was deleted.");

            var name = (dto.Name ?? string.Empty).Trim();
            var dup = await _uow.RoomTypes.AnyAsync(rt => rt.HotelId == hotelId && rt.Name == name, ct);
            if (dup) throw new InvalidOperationException("A room type with the same name already exists in this hotel.");

            var entity = _mapper.Map<RoomType>(dto);
            entity.Id = Guid.NewGuid();
            entity.HotelId = hotelId;
            entity.Name = name;
            entity.AverageRating = 0m;
            entity.RatingCount = 0;
            entity.CreatedAt = DateTime.UtcNow;

            await _uow.RoomTypes.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task UpdateRoomTypeAsync(Guid roomTypeId, UpdateRoomTypeRequest dto, CancellationToken ct)
        {
            if (roomTypeId == Guid.Empty) throw new ArgumentException("Invalid roomTypeId.", nameof(roomTypeId));
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // 1) Lấy entity (tracking)
            var entity = await _uow.RoomTypes.GetByIdAsync(roomTypeId, asNoTracking: false, ct: ct)
                         ?? throw new KeyNotFoundException("Room type not found.");

            // 2) Chống trùng tên trong cùng hotel (trừ chính nó)
            var name = (dto.Name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(dto.Name));

            var dup = await _uow.RoomTypes.AnyAsync(
                rt => rt.HotelId == entity.HotelId &&
                      rt.Id != roomTypeId &&
                      rt.Name == name, ct);
            if (dup)
                throw new InvalidOperationException("Another room type with the same name already exists in this hotel.");

            // 3) Map DTO -> entity
            _mapper.Map(dto, entity);
            entity.Name = name; // bảo đảm đã trim

            _uow.RoomTypes.Update(entity);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task DeleteRoomTypeAsync(Guid roomTypeId, CancellationToken ct)
        {
            if (roomTypeId == Guid.Empty) throw new ArgumentException("Invalid roomTypeId.", nameof(roomTypeId));

            var entity = await _uow.RoomTypes.GetByIdAsync(roomTypeId, asNoTracking: false, ct: ct)
                         ?? throw new KeyNotFoundException("Room type not found.");

            // Ràng buộc an toàn: không xoá nếu đang được tham chiếu
            // 1) RatePlans
            var hasPlans = await _uow.RatePlans.AnyAsync(rp => rp.RoomTypeId == roomTypeId, ct);
            if (hasPlans)
                throw new InvalidOperationException("Cannot delete this room type because it has rate plans.");

            // 2) Inventory
            var hasInventories = await _uow.Inventories.AnyAsync(inv => inv.RoomTypeId == roomTypeId, ct);
            if (hasInventories)
                throw new InvalidOperationException("Cannot delete this room type because it has inventories.");

            // 3) BookingItem
            var usedInBookings = await _uow.BookingItems.AnyAsync(bi => bi.RoomTypeId == roomTypeId, ct);
            if (usedInBookings)
                throw new InvalidOperationException("Cannot delete this room type because it is referenced by bookings.");

            _uow.RoomTypes.Remove(entity);
            await _uow.SaveChangesAsync(ct);
        }
        #endregion
        #region RatePlans
        public async Task<Guid> CreateRatePlanAsync(CreateRatePlanRequest dto, CancellationToken ct)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.HotelId == Guid.Empty) throw new ArgumentException("HotelId is required.", nameof(dto.HotelId));
            if (dto.RoomTypeId == Guid.Empty) throw new ArgumentException("RoomTypeId is required.", nameof(dto.RoomTypeId));

            // 1) Hotel hợp lệ
            var hotel = await _uow.Hotels.GetByIdAsync(dto.HotelId, asNoTracking: true, ct: ct)
                        ?? throw new KeyNotFoundException("Hotel not found.");
            if (hotel.IsDeleted) throw new InvalidOperationException("Hotel was deleted.");

            // 2) RoomType hợp lệ & thuộc hotel
            var roomType = await _uow.RoomTypes.GetByIdAsync(dto.RoomTypeId, asNoTracking: true, ct: ct)
                           ?? throw new KeyNotFoundException("Room type not found.");
            if (roomType.HotelId != dto.HotelId)
                throw new InvalidOperationException("Room type does not belong to the specified hotel.");

            // Chuẩn hóa input
            var name = (dto.Name ?? string.Empty).Trim();
            var currency = (dto.Currency ?? "VND").ToUpperInvariant();
            var start = dto.StartDate.Date;
            var end = dto.EndDate.Date;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(dto.Name));
            if (end < start)
                throw new ArgumentException("EndDate must be >= StartDate.", nameof(dto.EndDate));

            // 3) Không trùng tên trong cùng RoomType
            var dupName = await _uow.RatePlans.AnyAsync(
                rp => rp.HotelId == dto.HotelId
                   && rp.RoomTypeId == dto.RoomTypeId
                   && rp.Name == name, ct);
            if (dupName)
                throw new InvalidOperationException("A rate plan with the same name already exists for this room type.");

            // 4) Không chồng ngày với plan khác của cùng RoomType
            // overlap khi: existing.End >= new.Start && existing.Start <= new.End
            var overlaps = await _uow.RatePlans.AnyAsync(
                rp => rp.HotelId == dto.HotelId
                   && rp.RoomTypeId == dto.RoomTypeId
                   && rp.EndDate.Date >= start
                   && rp.StartDate.Date <= end, ct);
            if (overlaps)
                throw new InvalidOperationException("Date range overlaps with an existing rate plan.");

            // 5) Tạo entity
            var entity = _mapper.Map<RatePlan>(dto);
            entity.Id = Guid.NewGuid();

            await _uow.RatePlans.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.Id;
        }


        public async Task UpdateRatePlanAsync(Guid ratePlanId, UpdateRatePlanRequest dto, CancellationToken ct)
        {
            if (ratePlanId == Guid.Empty) throw new ArgumentException("Invalid ratePlanId.", nameof(ratePlanId));
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var entity = await _uow.RatePlans.GetByIdAsync(ratePlanId, asNoTracking: false, ct: ct)
                         ?? throw new KeyNotFoundException("Rate plan not found.");

            // Lấy các giá trị đã chuẩn hóa theo profile để dùng trong validate
            var normalizedName = (dto.Name ?? string.Empty).Trim();
            var startDate = dto.StartDate.Date;
            var endDate = dto.EndDate.Date;

            if (string.IsNullOrWhiteSpace(normalizedName))
                throw new ArgumentException("Name is required.", nameof(dto.Name));
            if (endDate < startDate)
                throw new InvalidOperationException("EndDate must be >= StartDate.");

            // Validate: trùng tên (trừ chính nó)
            var dupName = await _uow.RatePlans.AnyAsync(rp =>
                rp.HotelId == entity.HotelId &&
                rp.RoomTypeId == entity.RoomTypeId &&
                rp.Id != ratePlanId &&
                rp.Name == normalizedName, ct);
            if (dupName)
                throw new InvalidOperationException("A rate plan with the same name already exists for this room type.");

            // Validate: overlap ngày (trừ chính nó)
            var overlap = await _uow.RatePlans.AnyAsync(rp =>
                rp.HotelId == entity.HotelId &&
                rp.RoomTypeId == entity.RoomTypeId &&
                rp.Id != ratePlanId &&
                rp.EndDate.Date >= startDate &&
                rp.StartDate.Date <= endDate, ct);
            if (overlap)
                throw new InvalidOperationException("Date range overlaps with another rate plan.");

            // Map DTO -> entity (AutoMapper sẽ set các field theo profile)
            _mapper.Map(dto, entity);

            _uow.RatePlans.Update(entity);
            await _uow.SaveChangesAsync(ct);
        }


        public async Task DeleteRatePlanAsync(Guid ratePlanId, CancellationToken ct)
        {
            if (ratePlanId == Guid.Empty) throw new ArgumentException("Invalid ratePlanId.", nameof(ratePlanId));

            var entity = await _uow.RatePlans.GetByIdAsync(ratePlanId, asNoTracking: false, ct: ct)
                         ?? throw new KeyNotFoundException("Rate plan not found.");

            // (Khuyến nghị) chặn xoá nếu đang được dùng bởi BookingItem
            // Nếu bạn KHÔNG có repo BookingItems thì bỏ khối check này.
            var inUse = await _uow.BookingItems.AnyAsync(bi => bi.RatePlanId == ratePlanId, ct);
            if (inUse)
                throw new InvalidOperationException("Cannot delete this rate plan because it is referenced by bookings.");

            _uow.RatePlans.Remove(entity);
            await _uow.SaveChangesAsync(ct);
        }

        #endregion
    }
}