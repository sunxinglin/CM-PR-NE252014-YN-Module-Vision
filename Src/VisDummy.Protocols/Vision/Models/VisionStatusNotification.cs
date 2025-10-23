using MediatR;

namespace VisDummy.Protocols.Vision.Models
{
    public class VisionStatusNotification : INotification
    {
        public Dictionary<string, bool> CameraStatus { get; set; } = null!;
    }
}
