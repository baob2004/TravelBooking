namespace TravelBooking.Application.DTOs.Search
{
    public class SearchResponse
    {
        public List<SearchResultItem> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}