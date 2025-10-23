namespace TApp.Apis.Models
{
    public class LogPaginationResponse : PaginationBaseResponse
    {

        public List<LogItemResponse> List { get; set; }
    }
}
