using ReactiveUI;
using Splat;
using VisDummy.Shared.Utils;

namespace TApp.ViewModels.ParamsMgmt
{
    public class ParamsMgmtViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => UrlDefines.URL_ParamsMgmt;
        public IScreen HostScreen { get; }
        public IPamsMarker PamsMarker { get; }
        public ParamsMgmtViewModel()
        {
            this.HostScreen = Locator.Current.GetRequiredService<IScreen>()!;
            this.PamsMarker = Locator.Current.GetRequiredService<IPamsMarker>();
        }
    }
}
