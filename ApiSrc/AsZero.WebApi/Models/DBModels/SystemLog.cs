using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsZero.WebApi.Models.DBModels
{
    [Table("system_log")]
    public class SystemLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EventGroup { get; set; } = null!;
        public string? EventSource { get; set; }
        public LogLevel Level { get; set; }
        public string Content { get; set; } = null!;
        public string? CreateUser { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
