namespace CinemaAPI.Helpers
{ 
    public class Pagination
    {
        public Pagination()
        {
            page = 1;
            rowsPerPage = 10;
            sortBy = null;
            descending = true;
            includeEntities = false;
        }
        public int page { get; set; }
        public int rowsPerPage { get; set; }
        public int? records { get; set; }
        public int? totalItems { get; set; }
        public int? totalPages { get; set; }
        public string? sortBy { get; set; }
        public bool descending { get; set; }
        public bool includeEntities { get; set; }
    }
}
