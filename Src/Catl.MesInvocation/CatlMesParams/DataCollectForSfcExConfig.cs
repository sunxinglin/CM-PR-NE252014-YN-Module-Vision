using Catl.WebServices.MachineIntegrationServiceService;

namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 收数出站配置参数
    /// </summary>
    public class DataCollectForSfcExConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public DataCollectForSfcExParams InterfaceParams { get; set; } = new DataCollectForSfcExParams();
    }

    public class DataCollectForSfcExParams : CatlMesConfigurationBase
    {
        public string DcGroup { get; set; } = "";
        public string DcGroupRevision { get; set; } = "";
        public ModeProcessSfc Mode { get; set; }
        public string Resource { get; set; } = "";
        public string ActivityId { get; set; } = "";
    }
}
