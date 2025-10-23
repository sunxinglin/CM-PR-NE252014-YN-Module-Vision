namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 电芯OCV测试配置类
    /// </summary>
    public class MiCustomDCForCellConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public MiCustomDCForCellExParams InterfaceParams { get; set; } = new MiCustomDCForCellExParams();
    }

    public class MiCustomDCForCellExParams : CatlMesConfigurationBase
    {
        public string DcSequence { get; set; } = "";
        public string Multispec { get; set; } = "";
        public string Resource { get; set; } = "";
        public string Marking { get; set; } = "";
    }
}
