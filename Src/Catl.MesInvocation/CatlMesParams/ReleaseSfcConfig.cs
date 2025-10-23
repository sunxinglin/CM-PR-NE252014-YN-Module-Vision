using Catl.WebServices.MiReleaseSfcWithActivityServiceService;

namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 释放模组码配置参数
    /// </summary>
    public class ReleaseSfcConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public ReleaseSfcParams InterfaceParams { get; set; } = new ReleaseSfcParams();
    }
    public class ReleaseSfcParams : CatlMesConfigurationBase
    {
        public decimal SfcQty { get; set; }
        public string Processlot { get; set; } = "";
        public modeProcessSFC Mode { get; set; }
        public string Activity { get; set; } = "";
        public string Resource { get; set; } = "";
    }
}
