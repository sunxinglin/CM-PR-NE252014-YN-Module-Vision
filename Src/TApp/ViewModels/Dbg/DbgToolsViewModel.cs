using ReactiveUI;
using Splat;
using VisDummy.Shared.Utils;

namespace TApp.ViewModels.Dbg
{
    public class DbgToolsViewModel: ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => UrlDefines.URL_ParamsMgmt;
        public IScreen HostScreen { get; }
        public IDbgMarker DbgMarker { get; }
        public DbgToolsViewModel()
        {
            this.HostScreen = Locator.Current.GetRequiredService<IScreen>();
            this.DbgMarker = Locator.Current.GetRequiredService<IDbgMarker>();
        }
    }
}
