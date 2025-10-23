using MediatR;
using Newtonsoft.Json;
using VisDummy.Protocols.模组入箱;
using VisDummy.Protocols.模组入箱.Middlewares.Common.PublishNotification;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Handlers
{
    internal class 模组入箱ScanContextNotificationHandler : INotificationHandler<ScanContextNotification>
    {
        private readonly 模组入箱MonitorViewModel _loadingMonitorViewModel;

        public 模组入箱ScanContextNotificationHandler()
        {
            _loadingMonitorViewModel = Locator.Current.GetRequiredService<模组入箱MonitorViewModel>();
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
