using Splat;
using VisDummy.MtMesp.CatlMes.ViewModel;

namespace VisDummy.MtMes.CatlMes.ViewModel
{
    public class CatlMesSettingEditVM()
    {
        public DataCollectForResourceFAIVM DataCollectForResourceFAIVM { get; } = Locator.Current.GetService<DataCollectForResourceFAIVM>();
    }
}
