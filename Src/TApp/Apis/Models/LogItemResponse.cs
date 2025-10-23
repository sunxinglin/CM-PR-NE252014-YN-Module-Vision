namespace TApp.Apis.Models
{
    public class LogItemResponse
    {
        public int Id { get; set; }
        public string EventGroup { get; set; }
        public string EventSource { get; set; }
        public LogLevel Level { get; set; }
        public string Content { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
