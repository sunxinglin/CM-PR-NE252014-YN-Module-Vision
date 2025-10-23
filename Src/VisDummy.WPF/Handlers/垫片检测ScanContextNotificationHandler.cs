using MediatR;
using Newtonsoft.Json;
using VisDummy.Protocols.垫片检测;
using VisDummy.Protocols.垫片检测.Middlewares.Common.PublishNotification;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Handlers
{
    internal class 垫片检测ScanContextNotificationHandler : INotificationHandler<ScanContextNotification>
    {
        private readonly 垫片检测MonitorViewModel _loadingMonitorViewModel;

        public 垫片检测ScanContextNotificationHandler()
        {
            _loadingMonitorViewModel = Locator.Current.GetRequiredService<垫片检测MonitorViewModel>();
        }
        public Task Handle(ScanContextNotification notification, CancellationToken cancellationToken)
        {
            var scan = JsonConvert.DeserializeObject<ScanContextNotification>(JsonConvert.SerializeObject(notification));
            var ctx = new ScanContext(null, scan.DevMsg, scan.MstMsg, scan.CreatedAt);
            _loadingMonitorViewModel.ScanContextSubject.OnNext(ctx);
            return Task.CompletedTask;
        }
    }
}
