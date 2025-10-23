using MediatR;
using Newtonsoft.Json;
using VisDummy.Protocols.模组检测;
using VisDummy.Protocols.模组检测.Middlewares.Common.PublishNotification;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Handlers
{
    internal class 模组检测ScanContextNotificationHandler : INotificationHandler<ScanContextNotification>
    {
        private readonly 模组检测MonitorViewModel _loadingMonitorViewModel;

        public 模组检测ScanContextNotificationHandler()
        {
            _loadingMonitorViewModel = Locator.Current.GetRequiredService<模组检测MonitorViewModel>();
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
