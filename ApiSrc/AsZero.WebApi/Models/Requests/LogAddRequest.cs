namespace AsZero.WebApi.Models.Requests
{
    public class LogAddRequest
    {
        public string EventSource { get; set; }
        public string EventGroup { get; set; }
        public LogLevel Level { get; set; }
        public string Content { get; set; }
        public string Operator { get; set; }
    }
}
