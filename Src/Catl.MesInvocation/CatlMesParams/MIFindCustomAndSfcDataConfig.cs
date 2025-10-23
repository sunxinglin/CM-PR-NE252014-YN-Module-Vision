using Catl.WebServices.MiFindCustomAndSfcDataServiceService;

namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 进站接口配置参数
    /// </summary>
    public class MiFindCustomAndSfcDataConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public MiFindCustomAndSfcDataParamers InterfaceParams { get; set; } = new MiFindCustomAndSfcDataParamers();
    }
    public class MiFindCustomAndSfcDataParamers : CatlMesConfigurationBase
    {
        public string Resource { get; set; } = "";
        public string Activity { get; set; } = "";
        public ObjectAliasEnum MasterData { get; set; }
        public ObjectAliasEnum CategoryData { get; set; }
        public string DataField { get; set; } = "";
        public modeProcessSFC Mode { get; set; }
    }
}
