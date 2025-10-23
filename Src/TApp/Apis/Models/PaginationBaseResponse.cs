namespace TApp.Apis.Models
{
    public class PaginationBaseResponse
    {
        public int Total { get; set; }
        public int Current { get; set; }
        public int PageSize { get; set; }
    }
}
