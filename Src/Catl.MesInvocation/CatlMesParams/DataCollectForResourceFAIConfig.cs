namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 首件接口配置参数
    /// </summary>
    public class DataCollectForResourceFAIConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public DataCollectForResourceFAIParams InterfaceParams { get; set; } = new DataCollectForResourceFAIParams();
    }

    public class DataCollectForResourceFAIParams : CatlMesConfigurationBase
    {
        public string DcGroup { get; set; } = "";
        public string DcMode { get; set; } = "";
        public string Material { get; set; } = "";
        public string MaterialRevision { get; set; } = "";
        public string DcGroupRevision { get; set; } = "";
        public string Resource { get; set; } = "";
        public string DcGroupSequence { get; set; } = "";
    }
}
