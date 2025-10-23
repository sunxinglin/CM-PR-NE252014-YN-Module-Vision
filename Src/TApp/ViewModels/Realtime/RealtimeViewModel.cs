using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using VisDummy.Shared.Utils;

namespace TApp.ViewModels.Realtime
{

    public class RealtimeViewModel : ReactiveObject, IRoutableViewModel
    {
 

        public RealtimeViewModel()
        {
            this.UIlogsViewModel = Locator.Current.GetRequiredService<UILogsViewModel>();
            this.HostScreen = Locator.Current.GetRequiredService<IScreen>();

            this.RtMarker = Locator.Current.GetRequiredService<IRtMarker>();
        }

        public UILogsViewModel UIlogsViewModel { get; }
        #region Routing
        public string UrlPathSegment => UrlDefines.URL_Realtime;
        public IScreen HostScreen { get; }

        public IRtMarker RtMarker { get; }
        #endregion


        #region Communication
        #endregion
    }
}
