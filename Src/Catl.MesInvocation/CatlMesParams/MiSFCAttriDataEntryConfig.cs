namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 装配检查配置类
    /// </summary>
    public class MiSFCAttriDataEntryConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public MiSFCAttriDataEntryParams InterfaceParams { get; set; } = new MiSFCAttriDataEntryParams();
    }

    public class MiSFCAttriDataEntryParams : CatlMesConfigurationBase
    {
        public string DcGroup { get; set; } = "";
        public string DcGroupRevision { get; set; } = "";
        public string SfcMode { get; set; } = "";
        public string ItemGroup { get; set; } = "";
        public string IsCheckSequence { get; set; } = "";
        public string Attributes { get; set; } = "";
    }
}
