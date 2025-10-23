using VisDummy.Shared.Utils;
using Splat;
using VisDummy.VMs.ViewModels;

namespace VisDummy.WPF.ViewModels
{
    public class DbgViewModel : ReactiveObject, IDbgMarker
    {
        public ManualVisCalibrationViewModel CalibVM { get; } = Locator.Current.GetService<ManualVisCalibrationViewModel>();
    }
}
