using MediatR;
using System;

namespace VisDummy.MKVMs.Messages
{

    public enum Way
    {
        Send,
        Receive,
        Error,
        Info,
    }
    public class Vision3DMessage
    {
        public Way Way { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
    public class Vision3DNotification : INotification
    {
        public string ProcName { get; set; }
        public Vision3DMessage Message { get; set; }
    }
}
