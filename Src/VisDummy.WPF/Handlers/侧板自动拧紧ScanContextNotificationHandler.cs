using Newtonsoft.Json;
using MediatR;
using VisDummy.Protocols.侧板自动拧紧;
using VisDummy.Protocols.侧板自动拧紧.Middlewares.Common.PublishNotification;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Handlers
{
	internal class 侧板自动拧紧ScanContextNotificationHandler : INotificationHandler<ScanContextNotification>
	{
		private readonly 侧板自动拧紧MonitorViewModel _loadingMonitorViewModel;

		public 侧板自动拧紧ScanContextNotificationHandler()
		{
			_loadingMonitorViewModel = Locator.Current.GetRequiredService<侧板自动拧紧MonitorViewModel>();
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
