namespace CinemaAPI.Helpers
{
    public class PaginatedResponse<T>
    {
        public Pagination Pagination { get; set; }
        public List<T> Data { get; set; }
    }
}
