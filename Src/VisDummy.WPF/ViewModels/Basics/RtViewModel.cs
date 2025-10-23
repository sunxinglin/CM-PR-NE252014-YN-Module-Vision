using ReactiveUI.Fody.Helpers;
using VisDummy.Shared.Utils;

namespace VisDummy.WPF.ViewModels
{
    public class RtViewModel : ReactiveObject, IRtMarker
    {
        public RtViewModel()
        {
            DynamicViewModel = Locator.Current.GetServices<IVisionMarker>();
        }

        [Reactive]
        public IEnumerable<IVisionMarker> DynamicViewModel { get; set; }
    }
}
