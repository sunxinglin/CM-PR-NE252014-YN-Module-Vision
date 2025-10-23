using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TApp.Apis.Models
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
