using Itminus.Protocols.Loading;
using MediatR;
using Newtonsoft.Json;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Handlers
{
    internal class LoadingScanContextNotificationHandler : INotificationHandler<ScanContextNotification>
    {
        private readonly LoadingMonitorViewModel _loadingMonitorViewModel;

        public LoadingScanContextNotificationHandler()
        {
            _loadingMonitorViewModel = Locator.Current.GetRequiredService<LoadingMonitorViewModel>();
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
