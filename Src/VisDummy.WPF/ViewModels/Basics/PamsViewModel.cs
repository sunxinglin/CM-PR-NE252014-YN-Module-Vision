using VisDummy.Shared.Utils;
using VisDummy.MtMes.CatlMes.ViewModel;
using VisDummy.MtMes.MtMes;
using VisDummy.VMs.ViewModels;

namespace VisDummy.WPF.ViewModels
{
    public class PamsViewModel : ReactiveObject, IPamsMarker
    {
        public CatlMesSettingEditVM MesEditVM { get; } = Locator.Current.GetService<CatlMesSettingEditVM>();
        public MtMesCtrlViewModel MesTestVM { get; } = Locator.Current.GetService<MtMesCtrlViewModel>();
        public GlobalParamsViewModel GlobalParamsVM { get; } = Locator.Current.GetService<GlobalParamsViewModel>();
    }
}
