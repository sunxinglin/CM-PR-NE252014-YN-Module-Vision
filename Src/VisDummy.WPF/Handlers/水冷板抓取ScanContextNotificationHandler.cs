using MediatR;
using Newtonsoft.Json;
using VisDummy.Protocols.水冷板抓取;
using VisDummy.Protocols.水冷板抓取.Middlewares.Common.PublishNotification;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Handlers
{
    internal class 水冷板抓取ScanContextNotificationHandler : INotificationHandler<ScanContextNotification>
    {
        private readonly 水冷板抓取MonitorViewModel _loadingMonitorViewModel;

        public 水冷板抓取ScanContextNotificationHandler()
        {
            _loadingMonitorViewModel = Locator.Current.GetRequiredService<水冷板抓取MonitorViewModel>();
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
