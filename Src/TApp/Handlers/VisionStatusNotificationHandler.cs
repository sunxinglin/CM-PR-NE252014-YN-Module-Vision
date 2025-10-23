using MediatR;
using Splat;
using System.Threading;
using TApp.ViewModels;
using VisDummy.Protocols.Vision.Models;

namespace TApp.Handlers
{
    public class VisionStatusNotificationHandler : INotificationHandler<VisionStatusNotification>
    {
        private readonly AppViewModel _appViewModel = Locator.Current.GetService<AppViewModel>();

        public Task Handle(VisionStatusNotification notification, CancellationToken cancellationToken)
        {
            _appViewModel.VisionStatusSubject.OnNext(notification);
            return Task.CompletedTask;
        }
    }
}
