using VisDummy.Shared.Utils;
using TApp.ViewModels.Realtime;
using ReactiveUI;
using Splat;

namespace TApp.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            this.AppViewModel = Locator.Current.GetRequiredService<AppViewModel>();
            this.RealtimeVM = Locator.Current.GetRequiredService<RealtimeViewModel>();
        }

        public AppViewModel AppViewModel { get; }
        public RealtimeViewModel RealtimeVM { get; }

    }
}
